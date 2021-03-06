﻿/*
    _                _      _  ____   _                           _____
   / \    _ __  ___ | |__  (_)/ ___| | |_  ___   __ _  _ __ ___  |  ___|__ _  _ __  _ __ ___
  / _ \  | '__|/ __|| '_ \ | |\___ \ | __|/ _ \ / _` || '_ ` _ \ | |_  / _` || '__|| '_ ` _ \
 / ___ \ | |  | (__ | | | || | ___) || |_|  __/| (_| || | | | | ||  _|| (_| || |   | | | | | |
/_/   \_\|_|   \___||_| |_||_||____/  \__|\___| \__,_||_| |_| |_||_|   \__,_||_|   |_| |_| |_|

 Copyright 2015-2017 Łukasz "JustArchi" Domeradzki
 Contact: JustArchi@JustArchi.net

 Licensed under the Apache License, Version 2.0 (the "License");
 you may not use this file except in compliance with the License.
 You may obtain a copy of the License at

 http://www.apache.org/licenses/LICENSE-2.0
					
 Unless required by applicable law or agreed to in writing, software
 distributed under the License is distributed on an "AS IS" BASIS,
 WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 See the License for the specific language governing permissions and
 limitations under the License.

*/

using System;
using System.Collections.Generic;
using System.Reflection;
using ArchiSteamFarm.JSON;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArchiSteamFarm.Tests {
	[TestClass]
	public sealed class Trading {
		[TestMethod]
		public void TradingMultiGameBadReject() {
			Steam.Item item1Game1 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 1, 1, 570, Steam.Item.EType.TradingCard);
			Steam.Item item1Game1X9 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 1, 9, 570, Steam.Item.EType.TradingCard);
			Steam.Item item2Game1 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 2, 1, 570, Steam.Item.EType.TradingCard);

			Steam.Item item1Game2 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 3, 1, 730, Steam.Item.EType.TradingCard);
			Steam.Item item2Game2 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 4, 1, 730, Steam.Item.EType.TradingCard);

			HashSet<Steam.Item> inventory = new HashSet<Steam.Item> { item1Game1X9, item1Game2, item2Game2 };
			HashSet<Steam.Item> itemsToGive = new HashSet<Steam.Item> { item1Game1, item1Game2 };
			HashSet<Steam.Item> itemsToReceive = new HashSet<Steam.Item> { item2Game1, item2Game2 };

			Assert.IsFalse(AcceptsTrade(inventory, itemsToGive, itemsToReceive));
		}

		[TestMethod]
		public void TradingMultiGameMultiTypeBadReject() {
			Steam.Item item1Type1Game1 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 1, 1, 570, Steam.Item.EType.TradingCard);
			Steam.Item item1Type1Game1X9 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 1, 9, 570, Steam.Item.EType.TradingCard);
			Steam.Item item2Type1Game1 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 2, 1, 570, Steam.Item.EType.TradingCard);

			Steam.Item item3Type2Game2 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 3, 1, 730, Steam.Item.EType.Emoticon);
			Steam.Item item3Type2Game2X9 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 3, 9, 730, Steam.Item.EType.Emoticon);
			Steam.Item item4Type2Game2 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 4, 1, 730, Steam.Item.EType.Emoticon);

			HashSet<Steam.Item> inventory = new HashSet<Steam.Item> { item1Type1Game1X9, item3Type2Game2X9, item4Type2Game2 };
			HashSet<Steam.Item> itemsToGive = new HashSet<Steam.Item> { item1Type1Game1, item4Type2Game2 };
			HashSet<Steam.Item> itemsToReceive = new HashSet<Steam.Item> { item2Type1Game1, item3Type2Game2 };

			Assert.IsFalse(AcceptsTrade(inventory, itemsToGive, itemsToReceive));
		}

		[TestMethod]
		public void TradingMultiGameMultiTypeNeutralAccept() {
			Steam.Item item1Type1Game1 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 1, 1, 570, Steam.Item.EType.TradingCard);
			Steam.Item item1Type1Game1X9 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 1, 9, 570, Steam.Item.EType.TradingCard);
			Steam.Item item2Type1Game1 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 2, 1, 570, Steam.Item.EType.TradingCard);

			Steam.Item item3Type2Game2 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 3, 1, 730, Steam.Item.EType.Emoticon);
			Steam.Item item4Type2Game2 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 4, 1, 730, Steam.Item.EType.Emoticon);

			HashSet<Steam.Item> inventory = new HashSet<Steam.Item> { item1Type1Game1X9, item3Type2Game2 };
			HashSet<Steam.Item> itemsToGive = new HashSet<Steam.Item> { item1Type1Game1, item3Type2Game2 };
			HashSet<Steam.Item> itemsToReceive = new HashSet<Steam.Item> { item2Type1Game1, item4Type2Game2 };

			Assert.IsTrue(AcceptsTrade(inventory, itemsToGive, itemsToReceive));
		}

		[TestMethod]
		public void TradingMultiGameNeutralAccept() {
			Steam.Item item1Game1 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 1, 1, 570, Steam.Item.EType.TradingCard);
			Steam.Item item1Game1X2 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 1, 2, 570, Steam.Item.EType.TradingCard);
			Steam.Item item2Game1 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 2, 1, 570, Steam.Item.EType.TradingCard);

			Steam.Item item1Game2 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 1, 1, 730, Steam.Item.EType.TradingCard);
			Steam.Item item2Game2 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 2, 1, 730, Steam.Item.EType.TradingCard);

			HashSet<Steam.Item> inventory = new HashSet<Steam.Item> { item1Game1X2, item1Game2 };
			HashSet<Steam.Item> itemsToGive = new HashSet<Steam.Item> { item1Game1, item1Game2 };
			HashSet<Steam.Item> itemsToReceive = new HashSet<Steam.Item> { item2Game1, item2Game2 };

			Assert.IsTrue(AcceptsTrade(inventory, itemsToGive, itemsToReceive));
		}

		[TestMethod]
		public void TradingSingleGameBadReject() {
			Steam.Item item1 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 1, 1, 570, Steam.Item.EType.TradingCard);
			Steam.Item item2 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 2, 1, 570, Steam.Item.EType.TradingCard);

			HashSet<Steam.Item> inventory = new HashSet<Steam.Item> { item1, item2 };
			HashSet<Steam.Item> itemsToGive = new HashSet<Steam.Item> { item1 };
			HashSet<Steam.Item> itemsToReceive = new HashSet<Steam.Item> { item2 };

			Assert.IsFalse(AcceptsTrade(inventory, itemsToGive, itemsToReceive));
		}

		[TestMethod]
		public void TradingSingleGameGoodAccept() {
			Steam.Item item1 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 1, 1, 570, Steam.Item.EType.TradingCard);
			Steam.Item item1X2 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 1, 2, 570, Steam.Item.EType.TradingCard);
			Steam.Item item2 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 2, 1, 570, Steam.Item.EType.TradingCard);

			HashSet<Steam.Item> inventory = new HashSet<Steam.Item> { item1X2 };
			HashSet<Steam.Item> itemsToGive = new HashSet<Steam.Item> { item1 };
			HashSet<Steam.Item> itemsToReceive = new HashSet<Steam.Item> { item2 };

			Assert.IsTrue(AcceptsTrade(inventory, itemsToGive, itemsToReceive));
		}

		[TestMethod]
		public void TradingSingleGameMultiTypeBadReject() {
			Steam.Item item1Type1 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 1, 1, 570, Steam.Item.EType.TradingCard);
			Steam.Item item1Type1X9 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 1, 9, 570, Steam.Item.EType.TradingCard);
			Steam.Item item2Type1 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 2, 1, 570, Steam.Item.EType.TradingCard);

			Steam.Item item3Type2 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 3, 1, 570, Steam.Item.EType.Emoticon);
			Steam.Item item3Type2X9 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 3, 9, 570, Steam.Item.EType.Emoticon);
			Steam.Item item4Type2 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 4, 1, 570, Steam.Item.EType.Emoticon);

			HashSet<Steam.Item> inventory = new HashSet<Steam.Item> { item1Type1X9, item3Type2X9, item4Type2 };
			HashSet<Steam.Item> itemsToGive = new HashSet<Steam.Item> { item1Type1, item4Type2 };
			HashSet<Steam.Item> itemsToReceive = new HashSet<Steam.Item> { item2Type1, item3Type2 };

			Assert.IsFalse(AcceptsTrade(inventory, itemsToGive, itemsToReceive));
		}

		[TestMethod]
		public void TradingSingleGameMultiTypeNeutralAccept() {
			Steam.Item item1Type1 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 1, 1, 570, Steam.Item.EType.TradingCard);
			Steam.Item item1Type1X9 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 1, 9, 570, Steam.Item.EType.TradingCard);
			Steam.Item item2Type1 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 2, 1, 570, Steam.Item.EType.TradingCard);

			Steam.Item item3Type2 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 3, 1, 570, Steam.Item.EType.Emoticon);
			Steam.Item item4Type2 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 4, 1, 570, Steam.Item.EType.Emoticon);

			HashSet<Steam.Item> inventory = new HashSet<Steam.Item> { item1Type1X9, item3Type2 };
			HashSet<Steam.Item> itemsToGive = new HashSet<Steam.Item> { item1Type1, item3Type2 };
			HashSet<Steam.Item> itemsToReceive = new HashSet<Steam.Item> { item2Type1, item4Type2 };

			Assert.IsTrue(AcceptsTrade(inventory, itemsToGive, itemsToReceive));
		}

		[TestMethod]
		public void TradingSingleGameNeutralAccept() {
			Steam.Item item1 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 1, 1, 570, Steam.Item.EType.TradingCard);
			Steam.Item item2 = new Steam.Item(Steam.Item.SteamAppID, Steam.Item.SteamCommunityContextID, 2, 1, 570, Steam.Item.EType.TradingCard);

			HashSet<Steam.Item> inventory = new HashSet<Steam.Item> { item1 };
			HashSet<Steam.Item> itemsToGive = new HashSet<Steam.Item> { item1 };
			HashSet<Steam.Item> itemsToReceive = new HashSet<Steam.Item> { item2 };

			Assert.IsTrue(AcceptsTrade(inventory, itemsToGive, itemsToReceive));
		}

		private static bool AcceptsTrade(HashSet<Steam.Item> inventory, HashSet<Steam.Item> itemsToGive, HashSet<Steam.Item> itemsToReceive) {
			Type trading = typeof(ArchiSteamFarm.Trading);
			MethodInfo method = trading.GetMethod("IsTradeNeutralOrBetter", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
			return (bool) method.Invoke(null, new object[] { inventory, itemsToGive, itemsToReceive });
		}
	}
}