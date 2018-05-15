using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TibiaHeleper.MemoryOperations;
using TibiaHeleper.Modules.TargetingModule;
using TibiaHeleper.Simulators;
using TibiaHeleper.Storage;

namespace TibiaHeleper.Modules.WalkerModule
{
    public static class DefaultActions
    {
        public static void DoAction(int actionType, params object[] arguments)
        {
            if (actionType == StatementType.getType["Say"]) Say((string)arguments[0]);
            else if (actionType == StatementType.getType["Mouse Click"]) Click((int)arguments[0], (int)arguments[1], (int)arguments[2], (bool)arguments[3]);
            else if (actionType == StatementType.getType["Hotkey"]) hotkey((string)arguments[0]);
            else if (actionType == StatementType.getType["Go To Label"]) GoToLabel((string)arguments[0]);
            else if (actionType == StatementType.getType["Use On Field"]) UseOnPosition((string)arguments[0], (int)arguments[1], (int)arguments[2], (int)arguments[3]);
            else if (actionType == StatementType.getType["Condition"]) FulfillCondition((string)arguments[0], (List<Condition>)arguments[1]);
            else if (actionType == StatementType.getType["Wait"]) Wait((int)arguments[0]);
            else if (actionType == StatementType.getType["Trade"]) Trade((List<TradeItem>)arguments[0]);
            else if (actionType == StatementType.getType["Deposit"]) Deposit((bool)arguments[0], (List<DepositItem>)arguments[1], (List<DepositItem>)arguments[2]);


        }

        public static void Say(string text)
        {
            KeyboardSimulator.Message(text);
        }

        public static void hotkey(string key)
        {
            KeyboardSimulator.Press(key);
        }
        public static void UseOnPosition(string hotkey, int xPosition, int yPosition, int floor)
        {
            if (GetData.isOnScreen(xPosition, yPosition, floor))
            {
                DefaultActions.hotkey(hotkey);
                Thread.Sleep(100);
                DefaultActions.Click(xPosition, yPosition, floor, false);
                Thread.Sleep(500);
            }
        }
        public static void Click(int xPosition, int yPosition, int floor, bool isRightClick)
        {
            if (GetData.isOnScreen(xPosition, yPosition, floor))
            {
                MouseSimulator.clickOnField(xPosition, yPosition, isRightClick);
            }
        }
        private static void GoToLabel(string label)
        {
            List<WalkerStatement> list = ModulesManager.walker.list;
            foreach (WalkerStatement statement in list) //finds in list label and sets Walker.actualstatementIndex to previous one
            {
                if (statement.name == label)
                {
                    ModulesManager.walker.actualStatementIndex = list.IndexOf(statement) - 1;
                }
            }
        }

        private static void FulfillCondition(string label, List<Condition> list)
        {
            bool fulyfilled = false;
            short and = StatementType.conditionElement["And"];

            foreach (Condition cond in list)
            {
                if (cond.connector == and)
                {
                    if (fulyfilled) //if not result is false
                    {
                        fulyfilled = checkCondition(cond);
                    }
                }
                else if (!fulyfilled) //if yes then "or" is true
                {
                    fulyfilled = checkCondition(cond);
                }
            }

            if (fulyfilled)
            {
                GoToLabel(label);
            }


        }

        private static bool checkCondition(Condition cond)
        {
            int item1 = getItem(1, cond);
            int item2 = getItem(2, cond);
            if (cond.comparator == StatementType.conditionElement[">"])
            {
                if (item1 > item2) return true;
                return false;
            }
            else if (cond.comparator == StatementType.conditionElement["<"])
            {
                if (item1 < item2) return true;
                return false;
            }
            else if (cond.comparator == StatementType.conditionElement["="])
            {
                if (item1 == item2) return true;
                return false;
            }
            else if (cond.comparator == StatementType.conditionElement["!="])
            {
                if (item1 != item2) return true;
                return false;
            }
            return false;
        }
        private static int getItem(int itemNumber, Condition cond)
        {
            object arg = null;
            int itemType = -1;
            if (itemNumber == 1)
            {
                itemType = cond.item1;
                if (cond.args.Count > 0)
                    arg = cond.args[0];
            }
            else
            {
                itemType = cond.item2;
                int item1 = cond.item1;
                if (cond.args.Count > 1)
                    arg = cond.args[1];
                else if (cond.args.Count > 0)
                    arg = cond.args[0];
            }

            if (itemType == StatementType.conditionElement["Cap"])
            {
                return GetData.Cap;
            }
            else if (itemType == StatementType.conditionElement["Item count"])
            {
                int result = 0;
                string action = cond.args.FirstOrDefault() as string;
                int x, y;
                GetData.getItemFromEQWindowPosition(out x, out y, Constants.ShieldXOffset, Constants.ShieldYOffset);
                bool wasHealerEnabled = ModulesManager.healer.working;

                int timeToSleep = 350;
                for (int i = 0; i < 5; i++) //nr of tries
                {

                    ModulesManager.HealerDisable();

                    KeyboardSimulator.Press(action);
                    Thread.Sleep(timeToSleep);
                    MouseSimulator.click(x, y);
                    Thread.Sleep(timeToSleep);
                    result = GetData.GetCountFromServerInfo();

                    if (wasHealerEnabled)
                        ModulesManager.HealerEnable();

                    GetData.clearLastServerInfo();

                    if (result != -1) break;
                    timeToSleep += 40;
                }
                if (result == -1) result = 0;

                return result;
            }
            else if (itemType == StatementType.conditionElement["Value"])
            {
                return (int)arg;
            }
            else if (itemType == StatementType.conditionElement["HP"])
            {
                return GetData.MyHP;
            }
            else if (itemType == StatementType.conditionElement["Mana"])
            {
                return GetData.MyMana;
            }
            return -1;
        }

        private static void Wait(int timeToWait)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.ElapsedMilliseconds < timeToWait && ModulesManager.walker.working)
            {
                //wait
            }
            sw.Stop();
        }

        private static void Trade(List<TradeItem> list)
        {
            List<TradeItem> toBuy = list.Where(item => item.action == TradeItem.Action.Buy).ToList();
            List<TradeItem> toSell = list.Where(item => item.action == TradeItem.Action.Sell).ToList();

            bool targetingEnabled = ModulesManager.targeting.working;

            if (targetingEnabled)
                ModulesManager.TargetingDisable();

            GetData.SellItems(toSell);
            GetData.BuyItems(toBuy);

            if (targetingEnabled)
                ModulesManager.TargetingEnable();
        }

        private static void Deposit(bool deposit, List<DepositItem> toPutInto, List<DepositItem> toTakeFrom)
        {

            //field is chest number%4
            int[] field = new int[4];
            field[3] = GetData.gameWindowWidth + Constants.ItemInOpenedWindowFromRightOffset;
            field[2] = field[3] - Constants.ItemInOpenWindowWidth;
            field[1] = field[2] - Constants.ItemInOpenWindowWidth;
            field[0] = field[1] - Constants.ItemInOpenWindowWidth;

            //FIND AND OPEN DEPO CHEST

            //put into deposit
            if (deposit && toPutInto.Count > 0)
            {
                OpenWindows(deposit, field);

                toPutInto.Sort((i1, i2) => String.Compare(i1.name, i2.name));
                int y = PutIntoDeposit(toPutInto, field);

                GetData.closeAllOpenedWindows(true);
                GetData.GoToPreviousWindow(GetData.firstOpenedWindow);

            }

            //take from
            if (toTakeFrom.Count > 0)
            {
                OpenWindows(deposit, field);


                toTakeFrom.Sort((i1, i2) => i1.chestNumber.CompareTo(i2.chestNumber));

                int currentLine = 0;
                int lastOpenedChest = toTakeFrom[0].chestNumber;

                if (deposit)
                    currentLine = OpenSelectedChest(lastOpenedChest, field, currentLine);

                foreach (DepositItem item in toTakeFrom)
                {
                    if (!ModulesManager.walker.working)
                        break;
                    //open windows            

                    if (deposit && lastOpenedChest != item.chestNumber)
                    {
                        GetData.GoToPreviousWindow(GetData.firstOpenedWindow);
                        lastOpenedChest = item.chestNumber;

                        if (item.chestNumber != -1)
                            OpenSelectedChest(item.chestNumber, field, currentLine);
                    }


                    //take items
                    int amount = 0, fromX, fromY, toX, toY, lastCap = GetData.Cap, notMoving = 0;
                    while ((item.amount == -1 || amount < item.amount) && GetData.getItemCoordinatesFromOpenedWindow(out fromX, out fromY, item.ID, GetData.firstOpenedWindow) && ModulesManager.walker.working)
                    {

                        if (GetData.getItemCoordinatesFromOpenedWindow(out toX, out toY, item.whereToPut.ID, GetData.secondOpenedWindow))
                        {
                            MouseSimulator.drag(fromX, fromY, toX, toY, true);
                            Thread.Sleep(200);

                            int itemAmount = item.amount;
                            amount = TakeItems(amount, itemAmount);
                        }
                        else
                        {
                            //try to open next backpack
                            if (!GetData.UseItemFromOpenedWindow(ModulesManager.targeting.nextContainer.ID, GetData.secondOpenedWindow))
                                item.whereToPut = ItemList.defaultContainer;
                        }

                        //protection when has no more cap
                        if (GetData.Cap == lastCap)
                            notMoving++;
                        else
                        {
                            lastCap = GetData.Cap;
                            notMoving = 0;
                        }
                        if (notMoving == 5)
                            break;
                    }
                }

                GetData.closeAllOpenedWindows();
            }


        }

        private static bool FindAndOpenDeposit()
        {
            
            int depoID=-1;
            int floorID=-1;

            List<Waypoint> deposit = GetData.FindItemOnTheGround(depoID);
            List<Waypoint> floor = GetData.FindItemOnTheGround(floorID);

            deposit.Sort((x, y) => x.xPos.CompareTo(y.xPos));
            floor.Sort((x, y) => x.xPos.CompareTo(y.xPos));

            List<Waypoint> toStand = new List<Waypoint>();

            foreach(Waypoint dp in deposit)
            {
                foreach(Waypoint fl in floor)
                {
                    
                }
            }



            
            
            throw new NotImplementedException();
            return false;
        }
        private static int TakeItems(int currentAmount, int targetAmount)
        {
            int lastCount = -1, sameCount = 0;

            while (currentAmount != targetAmount)
            {
                if (GetData.moveItemsCount != 0) // That means that move item window is opened
                {

                    if (GetData.moveItemsCount == targetAmount || targetAmount==-1)
                    {
                        currentAmount += GetData.moveItemsCount;
                        KeyboardSimulator.Press("Enter");
                        break;
                    }
                    else if (GetData.moveItemsCount < targetAmount)
                        KeyboardSimulator.Press("Right");
                    else if (GetData.moveItemsCount > targetAmount)
                        KeyboardSimulator.Press("Left");
                    
                    Thread.Sleep(50);
                    //check if moved item count change ane breaks when needed
                    if (GetData.moveItemsCount == lastCount)
                        sameCount++;
                    else
                        sameCount = 0;

                    if (sameCount == 3)
                        break;
                }
                else
                {
                    currentAmount += 1;
                    break;
                }
            }

            return currentAmount;
        }

        /// <summary>
        /// opens deposit or mailbox and player backpack
        /// </summary>
        /// <param name="deposit"></param>
        /// <param name="field"></param>
        private static void OpenWindows(bool deposit, int[] field)
        {
            int y = GetData.yPositionOfOpenedWindow(GetData.firstOpenedWindow) + Constants.ItemInOpenedWindowYOffset;

            //open deposit or mail box
            if (deposit)
                MouseSimulator.click(field[0], y, true);
            else
                MouseSimulator.click(field[1], y, true);

            GetData.openBackpack();
            GetData.waitForWindowOpen(GetData.secondOpenedWindow);
            Thread.Sleep(2000);
            GetData.resizeOpenedWindows();
        }
        private static int OpenSelectedChest(int chestNumber, int[] field,int currentLine)
        {
            chestNumber--;
            int result = currentLine + ScrollDown(chestNumber, currentLine);
            int y = GetData.yPositionOfOpenedWindow(GetData.firstOpenedWindow) + Constants.ItemInOpenedWindowYOffset;
            int x = field[chestNumber % 4];
            MouseSimulator.click(x, y, true);
            Thread.Sleep(500);
            return result;
        }
        private static int PutIntoDeposit(List<DepositItem> toPutInto, int[] field)
        {
            int currentLine = 0;
            int y = GetData.yPositionOfOpenedWindow(GetData.firstOpenedWindow) + Constants.ItemInOpenedWindowYOffset;

            //put items into depo
            if (toPutInto.Count > 0)
            {
                bool isMoreBackpack = true;

                while (isMoreBackpack)
                {
                    for (int i = 1; i < 18; i++)
                    {
                        List<DepositItem> list = toPutInto.Where(item => item.chestNumber == i).ToList();
                        if (list.Count > 0)
                        {
                            GetData.ScrollUpWindow(GetData.secondOpenedWindow);

                            int chestNumber = list[0].chestNumber - 1;

                            currentLine += ScrollDown(chestNumber, currentLine);
                            int x = field[chestNumber % 4];

                            int fromX, fromY;
                            List<int> idList = LootItem.ToIdList(list);

                            while (GetData.getItemCoordinatesFromOpenedWindow(out fromX, out fromY, idList, GetData.secondOpenedWindow) && ModulesManager.walker.working)
                            {
                                MouseSimulator.drag(fromX, fromY, x, y, true);
                                Thread.Sleep(500);
                                KeyboardSimulator.Press("Enter");
                            }
                        }
                    }
                    isMoreBackpack = GetData.UseItemFromOpenedWindow(ModulesManager.targeting.nextContainer.ID, GetData.secondOpenedWindow);
                }
                
            }

            return y;
        }
        /// <summary>
        /// scrolls dwn to have properly chest visible
        /// </summary>
        /// <param name="chestNumber"></param>
        /// <param name="currentLine"></param>
        /// <returns></returns>
        private static int ScrollDown(int chestNumber, int currentLine)
        {
            int linesToScroll = (int)chestNumber / 4  - currentLine;
            int column = chestNumber % 4;


            int x = GetData.gameWindowWidth + Constants.OpenedWindowScrollFromRightXOffset;
            int y = GetData.yPositionOfOpenedWindow(GetData.firstOpenedWindow) + Constants.OpenedWindowScrollDownButtonFromBottomYOffset + GetData.firstOpenedWindow.height;

            for (int i= 0; i < linesToScroll; i++)
            {
                for(int j=0; j<Constants.FullLineScrollClickCount; j++)
                {
                    MouseSimulator.click(x, y);
                }
            }

            return linesToScroll;
            
        }

    }
}
