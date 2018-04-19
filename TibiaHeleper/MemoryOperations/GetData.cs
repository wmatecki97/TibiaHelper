using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TibiaHeleper.Storage;
using System.Linq;
using TibiaHeleper.Simulators;
using System.Threading;

namespace TibiaHeleper.MemoryOperations
{
    class GetData
    {
        static Process Tibia;
        public static IntPtr Handle { get; set; }
        static uint Base;
        private static List<Creature> allSpottedCreaturesList;
        private static uint lastSpottedCreatureAddress;
        public static Creature Me;

        static GetData()
        {
            allSpottedCreaturesList = new List<Creature>();
            lastSpottedCreatureAddress = Addresses.InformationsOfSpottedCreaturesAndPlayersSartAddress;
        }

        #region MemoryOperations
        /// <summary>
        /// looking for tibia.exe returns false when Tibia.exe not found
        /// </summary>
        /// <returns></returns>
        public static bool inject()
        {
            Process[] processes = Process.GetProcessesByName("Tibia");
            if (processes.Length > 0)
            {
                Tibia = processes[0];
            }
            else return false;
            Handle = Tibia.Handle;
            Base = (uint)Tibia.MainModule.BaseAddress;
            return true;
        }
        public static bool isGameOpened()
        {
            return !Tibia.HasExited;
        }
        /// <summary>
        /// returns 32bit integer from Address given
        /// </summary>
        /// <param name="Address"></param>
        /// <returns></returns>
        public static int getIntegerDataFromAddress(uint Address)
        {
            return ReadMemory.ReadInt32(Base + Address, Handle);
        }
        public static int getByteAsIntegerFromAddress(uint Address)
        {
            return (int)ReadMemory.ReadBytes(Handle, (long)(Base + Address), 1).FirstOrDefault();
        }
        public static int getIntegerDataFromDynamicAddress(uint Address)
        {
            return ReadMemory.ReadInt32(Address, Handle);
        }
        public static string getStringFromAddress(uint Address, uint length = 32)
        {
            return ReadMemory.ReadString(Base + Address, Handle, length);
        }
        public static string getStringFromDynamicAddress(uint Address, uint length = 32)
        {
            return ReadMemory.ReadString(Address, Handle, length);
        }
        public static void WriteString(string inputString, uint address)
        {
            string temp = ActualInput;
            byte[] bytes = Encoding.ASCII.GetBytes(inputString);
            int bufferLength = bytes.Length;
            int numberOfBytesWritten = 0;

            ReadMemory.WriteString(Base + address, Handle, bytes, bufferLength, ref numberOfBytesWritten);
        }
        public static void writeInt32(uint Address, int toWrite)
        {
            byte[] lpBuffer = BitConverter.GetBytes(toWrite);
            int bytesWritten = 0;
            ReadMemory.WriteInt32(Address + Base, Handle, lpBuffer, 4, ref bytesWritten);
        }
        public static Process getProcess()
        {
            return Tibia;
        }
        public static int getDynamicAddress(List<uint> list)
        {
            int address = getIntegerDataFromAddress(list[0]);
            int temp = address;
            for (int i = 1; i < list.Count; i++)
            {
                address = temp + (int)list[i];
                temp = getIntegerDataFromDynamicAddress((uint)address);
            }
            return address;
        }
        #endregion

        #region LoggedInPlayerInformations
        /// <summary>
        /// checks who is logged in and sets Me. Returns null if nobody is logged in.
        /// </summary>
        public static void WhoAmI()
        {
            Me = null;
            int PlayersOnScreenMiddleCount = 0;

            while (Me == null)
            {
                lock (allSpottedCreaturesList)
                {
                    foreach (Creature creature in allSpottedCreaturesList)
                    {
                        if (creature.XPosition == MyXPosition && creature.YPosition == MyYPosition && creature.Floor == MyFloor)
                        {
                            PlayersOnScreenMiddleCount++;
                            if (PlayersOnScreenMiddleCount == 1) Me = creature;
                        }
                    }
                }
                if (PlayersOnScreenMiddleCount > 1) Me = null;
                if (PlayersOnScreenMiddleCount == 0) break;//player is not logged in

            }
        }
        public static int MyID { get { return Me.id; } }
        public static int XOR { get { return getIntegerDataFromAddress(Addresses.XORAdr); } }
        public static int MyHP
        {
            get
            {
                int xor = XOR;
                int hp = getIntegerDataFromAddress(Addresses.HPAdr);
                return xor ^ hp;
            }
        }
        public static int MyMana
        {
            get
            {
                int xor = XOR;
                int mana = getIntegerDataFromAddress(Addresses.ManaAdr);
                return xor ^ mana;
            }
        }
        public static bool AmIHasted
        {
            get
            {
                int x = getIntegerDataFromAddress(Addresses.MyFlags);
                return (getIntegerDataFromAddress(Addresses.MyFlags) & Flags.AmIHasted) == Flags.AmIHasted;
            }
        }
        public static bool AmInPZ
        {
            get
            {
                int x = getIntegerDataFromAddress(Addresses.MyFlags);
                return (getIntegerDataFromAddress(Addresses.MyFlags) & Flags.AmIInPZ) == Flags.AmIInPZ;
            }
        }
        public static int MyActualSpeed { get { return getIntegerDataFromAddress(Addresses.ActualSpeed); } }
        public static int MyNormalSpeed { get { return getIntegerDataFromAddress(Addresses.NormalSpeed); } }
        public static int MyXPosition { get { return getIntegerDataFromAddress(Addresses.MyXPosition); } }
        public static int MyYPosition { get { return getIntegerDataFromAddress(Addresses.MyYPosition); } }
        public static int MyFloor { get { return getByteAsIntegerFromAddress(Addresses.MyFloorByteAddress); } }
        public static int Cap { get { return (int)((getIntegerDataFromAddress(Addresses.XORAdr) ^ getIntegerDataFromAddress(Addresses.CapXor)) / 100); } }
        public static int HungryTime { get { return getIntegerDataFromAddress(Addresses.AmIHungry); } }
        #endregion

        #region CreaturesAndPlayers
        public static Creature getPlayer(string playerName)
        {
            ActualizeAllSpottedCreaturesList();
            lock (allSpottedCreaturesList)
            {
                foreach (Creature creature in allSpottedCreaturesList)
                {
                    if (creature.name == playerName)
                        return creature;
                }
            }
            return null;
        }

        public static string GetCreatureName(uint Address)
        {
            return getStringFromAddress(Address + Addresses.CreatureNameShift);
        }
        public static bool isCreatureOnScreen(uint playerAddress)
        {
            return (getIntegerDataFromAddress(playerAddress + Addresses.CreatureOnScreenShift) == 1);
        }
        public static int getCreatureHPPercent(uint CreatureAddress)
        {
            return getIntegerDataFromAddress(CreatureAddress + Addresses.CreatureHpShift);
        }
        public static int getCreatureXPosition(uint CreatureAddress)
        {
            return getIntegerDataFromAddress(CreatureAddress + Addresses.CreatureXPositionShift);
        }
        public static int getCreatureYPosition(uint CreatureAddress)
        {
            return getIntegerDataFromAddress(CreatureAddress + Addresses.CreatureYPositionShift);
        }
        public static int getCreatureFloor(uint CreatureAddress)
        {
            return getByteAsIntegerFromAddress(CreatureAddress + Addresses.CreatureFloorShift);

        }


        public static int GetDistance(Creature creature) //TO IMPLEMENT
        {
            if (creature.Floor != MyFloor) return 100;
            return Math.Abs(creature.XPosition - MyXPosition) + Math.Abs(creature.YPosition - MyYPosition);
        }
        public static int GetDistance(int xPosition, int yPosition)
        {
            return Math.Abs(xPosition - MyXPosition) + Math.Abs(yPosition - MyYPosition);
        }
        public static bool isOnScreen(int xPosition, int yPosition, int floor = 100)
        {
            bool isFloorGood = floor == MyFloor || floor == 100;
            bool isXGood = Math.Abs(GetData.MyXPosition - xPosition) <= (Constants.GameWindowWidthSquares - 1) / 2;
            bool isYGood = Math.Abs(GetData.MyYPosition - yPosition) <= (Constants.GameWindowHeightSquares - 1) / 2;
            return isXGood && isYGood && isFloorGood;
        }



        public static void ActualizeAllSpottedCreaturesList()
        {
            bool wasCreatureSpotted = true;
            int id;
            uint CreatureInformationBlockSize = Addresses.CreatureInformationBlockSize;
            while (wasCreatureSpotted)
            {
                if ((id = getIntegerDataFromAddress(lastSpottedCreatureAddress)) != 0)
                {
                    Creature creature = new Creature(id, lastSpottedCreatureAddress);
                    lock (allSpottedCreaturesList)
                    {
                        allSpottedCreaturesList.Insert(0, creature);
                    }
                    lastSpottedCreatureAddress += CreatureInformationBlockSize;
                }
                else wasCreatureSpotted = false;
            }
            lock (allSpottedCreaturesList) //delete from list dead creatures
            {
                for (int i = 0; i < allSpottedCreaturesList.Count(); i++)
                {
                    if (allSpottedCreaturesList[i].HPPercent == 0)
                    {
                        allSpottedCreaturesList.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
        public static void ResetAllSpottedCreatureList()
        {
            lastSpottedCreatureAddress = Addresses.InformationsOfSpottedCreaturesAndPlayersSartAddress;
            lock (allSpottedCreaturesList)
            {
                allSpottedCreaturesList = new List<Creature>();
            }
        }
        public static List<Creature> GetBattleList()
        {
            List<Creature> battleList = new List<Creature>();
            ActualizeAllSpottedCreaturesList();
            lock (allSpottedCreaturesList)
            {
                foreach (Creature creature in allSpottedCreaturesList)
                {
                    if (creature.onScreen && creature.Floor == MyFloor)
                        battleList.Add(creature);
                }
            }
            return battleList;
        }
        public static int getTargetID
        {
            get
            {
                return getIntegerDataFromAddress(Addresses.Target);
            }
        }
        public static bool FollowTarget
        {
            get { return getIntegerDataFromAddress(Addresses.FollowTargetAddress) == 1; }
            set { writeInt32(Addresses.FollowTargetAddress, value == false ? 0 : 1); }
        }
        #endregion


        #region GameWindowParameters
        public static int GameWindowHeight { get { return getIntegerDataFromDynamicAddress((uint)getDynamicAddress(Addresses.GameWindowHeight)); } }
        public static int GameWindowDistanceFromLeft { get { return getIntegerDataFromDynamicAddress((uint)getDynamicAddress(Addresses.GameWindowFromLeftDistance)); } }
        public static string LastServerInfo { get { return getStringFromAddress(Addresses.LastServerInfoMessage); } }
        public static int LastClickedObjectID { get { return getIntegerDataFromAddress(Addresses.LastClickedObject); } }
        public static void clearLastServerInfo()
        {
            string toClear = LastServerInfo, blank = "";
            foreach (char c in toClear)
                blank += ' ';
            WriteString(blank, Addresses.LastServerInfoMessage);
        }
        public static int gameWindowWidth { get { return getIntegerDataFromAddress(Addresses.GameWindowWidth); } }
        public static int tradeItemID { get { return getIntegerDataFromDynamicAddress((uint)getDynamicAddress(Addresses.tradeWindowSelectedItem)); } }
        #endregion

        public static string ActualInput { get { return getStringFromDynamicAddress((uint)getDynamicAddress(Addresses.ActualInput)); } }
        /// <summary>
        /// returns count of items from serverInfo for example using one of 20 mana potions returns 20.
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static int getCountFromServerInfo()
        {
            string info = getStringFromAddress(Addresses.LastServerInfoMessage);
            if (info.IndexOf("the last") != -1) return 1;
            int index = info.IndexOf("of ") + 3;
            int number = 0;

            try
            {
                number = int.Parse(info[index].ToString()) + number * 10;
            }
            catch (Exception)
            {
                return -1;
            }

            return number;
        }

        #region RightSideWindows
        public static void openEqWindow()
        {
            uint secondWindowAddress = (uint)getDynamicAddress(Addresses.SecondWindowFromTop);
            uint thirdWindowAddress = (uint)getDynamicAddress(Addresses.ThirdWindowFromTop);
            if ((getIntegerDataFromDynamicAddress(secondWindowAddress + Addresses.WindowIDOffset)) != Flags.EQWindowHidden) // if eq windown is hidden
            {
                if ((getIntegerDataFromDynamicAddress(thirdWindowAddress + Addresses.WindowIDOffset)) == Flags.EQWindowHidden)
                {
                    int y = getIntegerDataFromDynamicAddress(thirdWindowAddress) + Constants.MaximizeEQButtonYOffset;
                    int x = gameWindowWidth + Constants.MaximizeEQButtonXOffset;
                    MouseSimulator.click(x, y);
                }
            }
            else
            {
                int y = getIntegerDataFromDynamicAddress(secondWindowAddress) + Constants.MaximizeEQButtonYOffset;
                int x = gameWindowWidth + Constants.MaximizeEQButtonXOffset;
                MouseSimulator.click(x, y);
            }
        }
        public static void openBackpack()
        {
            int x, y;
            getItemFromEQWindowPosition(out x, out y, Constants.ShieldXOffset, Constants.BackpackYOffset);
            MouseSimulator.click(x, y, true);
        }
        public static void closeAllOpenedWindows()
        {
            openEqWindow();
            int x = gameWindowWidth + Constants.OpenedWindowCloseButtonFromLeftXOffset;
            int y = Constants.FirstOpenedWindowYOffset + Constants.OpenedWindowCloseButtonYOffset;
            while (firstOpenedWindowHeight > 60)
            {
                MouseSimulator.click(x, y);
            }
        }
        public static void resizeFirstOpenedWindow()
        {
            int x = gameWindowWidth - 50;
            int y = Constants.FirstOpenedWindowYOffset + getIntegerDataFromDynamicAddress((uint)getDynamicAddress(Addresses.FirstOpenedWindowHeight)) + Constants.OpenedWindowResizeFromBottomYOffset;
            int toX = x;
            int toY = Constants.FirstOpenedWindowYOffset + Constants.OpenedWindowMinimumHeight;
            MouseSimulator.drag(x, y, toX, toY);
        }

        public static int firstOpenedWindowHeight { get { return getIntegerDataFromDynamicAddress((uint)getDynamicAddress(Addresses.FirstOpenedWindowHeight)); } }
        public static bool getItemFromEQWindowPosition(out int x, out int y, int xOffset, int yOffset)
        {

            uint secondWindowAddress = (uint)getDynamicAddress(Addresses.SecondWindowFromTop);
            uint thirdWindowAddress = (uint)getDynamicAddress(Addresses.ThirdWindowFromTop);
            openEqWindow();

            x = gameWindowWidth + xOffset;
            y = -1;
            if ((getIntegerDataFromDynamicAddress(secondWindowAddress + Addresses.WindowIDOffset)) != Flags.EQWindowID) // if eq windown is not on second position
            {
                if (getIntegerDataFromDynamicAddress(thirdWindowAddress + Addresses.WindowIDOffset) != Flags.EQWindowID) //if eq window is not in second or third position
                {
                    return false;
                }
                else
                {
                    y = getIntegerDataFromDynamicAddress(thirdWindowAddress) + yOffset;
                }
            }
            else
            {
                y = getIntegerDataFromDynamicAddress(secondWindowAddress) + yOffset;
            }
            return true;
        }
        public static bool getItemCoordinatesFromFirstOpenedWindow(out int x, out int y, int item)
        {
            List<int> list = new List<int>();
            list.Add(item);
            return getItemCoordinatesFromFirstOpenedWindow(out x, out y, list);
        }
        public static bool getItemCoordinatesFromFirstOpenedWindow(out int x, out int y, List<int> items)
        {
            if (waitForWindowOpen())
            {
                y = Constants.FirstOpenedWindowYOffset + Constants.ItemInOpenedWindowYOffset;
                int yScroll = Constants.OpenedWindowScrollDownButtonFromBottomYOffset + Constants.FirstOpenedWindowYOffset + firstOpenedWindowHeight;
                int xScroll = Constants.OpenedWindowScrollFromRightXOffset + gameWindowWidth;

                for (int j = 0; j < 5; j++) // for all lines in backpack
                {
                    x = gameWindowWidth + Constants.ItemInOpenedWindowFromRightOffset;
                    for (int i = 0; i < 4; i++) // check every 4 items
                    {
                        MouseSimulator.click(x, y);
                        if (items.Any(item => item == LastClickedObjectID))
                        {
                            return true;
                        }
                        x -= Constants.ItemInOpenWindowWidth;
                    }
                    for (int i = 0; i < Constants.FullLineScrollClickCount; i++)
                    {
                        MouseSimulator.click(xScroll, yScroll);
                    }
                }
            }

            x = y = -1;
            return false;
        }

        public static void UseItemFromBackpack(int itemID, int TimesToUse = 1)
        {
            List<int> list = new List<int>();
            list.Add(itemID);
            UseItemsFromBackpack(list, TimesToUse);
        }
        public static void UseItemsFromBackpack(List<int> items, int timeToUse = 1)
        {
            openEqWindow();
            closeAllOpenedWindows();
            openBackpack();

            UseItemsFromFrstOpenedWindow(items, timeToUse);
        }
        /// <summary>
        /// drags from given coordinates to backpack
        /// </summary>
        /// <param name="xItemPosition"></param>
        /// <param name="yItemPosition"></param>
        public static void DragToBackpack(int xItemPosition, int yItemPosition)
        {
            int xBackpack, yBackapck;
            openEqWindow();
            getItemFromEQWindowPosition(out xBackpack, out yBackapck, Constants.ShieldXOffset, Constants.BackpackYOffset);
            MouseSimulator.drag(xItemPosition, yItemPosition, xBackpack, yBackapck);
            //MouseSimulator.drag(xItemPosition, yItemPosition, xItemPosition+35, yItemPosition);
            Thread.Sleep(200);
            KeyboardSimulator.Press("Enter");
        }
        /// <summary>
        /// Find in first opened window item with given ID and right click on it
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="timeToUse"></param>
        public static void UseItemFromFrstOpenedWindow(int itemId, int timeToUse = 1)
        {
            List<int> list = new List<int>();
            list.Add(itemId);
            UseItemsFromFrstOpenedWindow(list, timeToUse);
        }
        /// <summary>
        /// Find in first opened window items with given ID and right click on it
        /// </summary>
        /// <param name="items"></param>
        /// <param name="timeToUse"></param>
        public static void UseItemsFromFrstOpenedWindow(List<int> items, int timeToUse = 1)
        {
            int x, y;

            if (waitForWindowOpen())
            {
                resizeFirstOpenedWindow();

                if (getItemCoordinatesFromFirstOpenedWindow(out x, out y, items))
                {
                    for (int i = 0; i < timeToUse; i++)
                    {
                        MouseSimulator.click(x, y, true);
                        if (timeToUse > 1)
                            Thread.Sleep(100);
                    }
                }
            }
            
        }
        /// <summary>
        /// wait until container is opened or 2 seconds
        /// </summary>
        private static bool waitForWindowOpen()
        {
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (firstOpenedWindowHeight == 0 && sw.ElapsedMilliseconds < 2000) ;
            }
            return firstOpenedWindowHeight > 0;
        }

        private static void TradeItems(Dictionary<int, int> items)
        {
            int x = gameWindowWidth -50;

            int y = Constants.FirstOpenedWindowYOffset + Constants.TradeWindowFirstItemYOffset;
            ClickAndTradeItem(x, y, items);

            y = Constants.FirstOpenedWindowYOffset + Constants.TradeWindowSecondItemYOffset;
            ClickAndTradeItem(x, y, items);

            y = Constants.FirstOpenedWindowYOffset + Constants.ThirdItemFromListYOffset;
            ClickAndTradeItem(x, y, items);


            int LastTradeItemID = -1;

            while (LastTradeItemID != tradeItemID)
            {
                LastTradeItemID = tradeItemID;

                int scrollY = Constants.TradeWindowScrollDownButtonYOffset + Constants.FirstOpenedWindowYOffset;
                int scrollX = Constants.OpenedWindowScrollFromRightXOffset + gameWindowWidth;
                MouseSimulator.click(scrollX, scrollY);

                ClickAndTradeItem(x, y, items);
            }
        }
        public static void OpenTradeWindow()
        {
            openEqWindow();
            closeAllOpenedWindows();
            for (int i = 0; i < 5; i++)
            {
                KeyboardSimulator.Message("Hi");
                Thread.Sleep(1000);
                KeyboardSimulator.Message("Trade");
                if (waitForWindowOpen())
                    break;
            }
            if (!waitForWindowOpen())
                return;
            Thread.Sleep(1000);

            while (firstOpenedWindowHeight > 200)
                resizeFirstOpenedWindow();
        }
        public static void SellItems(List<int> items)
        {
            Dictionary<int, int> dict = new Dictionary<int, int>();
            foreach (int item in items)
            {
                dict.Add(item, 0);
            }

            OpenTradeWindow();
            int x, y;

            Thread.Sleep(200);

            // Click sell button
            y = Constants.FirstOpenedWindowYOffset + Constants.TradeWindowSellButtonYOffset;
            x = Constants.TradeWindowSellOrOKButtonXFromRightOffset + gameWindowWidth;
            MouseSimulator.click(x, y);
            
            TradeItems(dict);

        }
        public static void BuyItems(Dictionary<int,int> ItemsAndAmounts)
        {
            OpenTradeWindow();
            TradeItems(ItemsAndAmounts);
        }

        private static void ClickAndTradeItem(int x, int y, Dictionary<int,int> items)
        {
            MouseSimulator.click(x, y);
            if(items.Any(item => item.Key == tradeItemID))
            {
                TradeIt(items[tradeItemID]);
            }
        }
        /// <summary>
        /// Moves the slider of item count and clicks OK
        /// </summary>
        private static void TradeIt(int count = 0)
        {
            int y = Constants.TradeWindowItemCountSliderYPosition + Constants.FirstOpenedWindowYOffset;
            int x = Constants.TradeWindowItemCountMoreButtonXPosition + gameWindowWidth;

           
            for(int i=0; i<count-1; i++)
            {
                MouseSimulator.click(x, y);
            }

           

            x = Constants.TradeWindowSellOrOKButtonXFromRightOffset + gameWindowWidth;
            y = Constants.FirstOpenedWindowYOffset + Constants.TradeWindowOKButtonYOffset;
            MouseSimulator.click(x, y);
        }

        #endregion

    }
}
