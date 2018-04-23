using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TibiaHeleper.MemoryOperations;
using TibiaHeleper.Simulators;

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
                DefaultActions.Click(xPosition, yPosition,floor,false);
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
            foreach(WalkerStatement statement in list) //finds in list label and sets Walker.actualstatementIndex to previous one
            {
                if(statement.name == label)
                {
                    ModulesManager.walker.actualStatementIndex = list.IndexOf(statement) - 1;
                }
            }
        }

        private static void FulfillCondition(string label, List<Condition> list)
        {
            bool fulyfilled = false;
            short and = StatementType.conditionElement["And"];

            foreach(Condition cond in list)
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
            int item1 = getItem(1,cond);
            int item2 = getItem(2, cond);
            if(cond.comparator == StatementType.conditionElement[">"])
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
            int itemType=-1;
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
                int result=0;
                string action = cond.args.FirstOrDefault() as string;
                int x, y;
                GetData.getItemFromEQWindowPosition(out x, out y, Constants.ShieldXOffset, Constants.ShieldYOffset);
                bool wasHealerEnabled = ModulesManager.healer.working;

                int timeToSleep = 50;
                for(int i=0; i<5; i++) //nr of tries
                {

                    ModulesManager.HealerDisable();

                    KeyboardSimulator.Press(action);
                    Thread.Sleep(timeToSleep);
                    MouseSimulator.click(x, y);
                    Thread.Sleep(timeToSleep);
                    result = GetData.getCountFromServerInfo();

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
            while(sw.ElapsedMilliseconds<timeToWait && ModulesManager.walker.working)
            {
                //wait
            }
            sw.Stop();
        }

        private static void Trade(List<TradeItem> list)
        {
            List<TradeItem> toBuy = list.Where(item => item.action == TradeItem.Action.Buy).ToList();
            List<TradeItem> toSell = list.Where(item => item.action == TradeItem.Action.Sell).ToList();

            GetData.SellItems(toSell);
            GetData.BuyItems(toBuy);
        }
    }
}
