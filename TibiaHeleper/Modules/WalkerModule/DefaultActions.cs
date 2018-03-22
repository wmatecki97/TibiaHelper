using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TibiaHeleper.MemoryOperations;
using TibiaHeleper.Simulators;

namespace TibiaHeleper.Modules.WalkerModule
{
    public static class DefaultActions
    {
        public static void DoAction(int actionType, params object[] arguments)
        {
            if (actionType == (int)StatementType.getType["Say"]) Say((string)arguments[0]);
            if (actionType == (int)StatementType.getType["Mouse Click"]) Click((int)arguments[0], (int)arguments[1], (int)arguments[2], (bool)arguments[3]);
            if (actionType == (int)StatementType.getType["Hotkey"]) hotkey((string)arguments[0]);
            if (actionType == (int)StatementType.getType["Go To Label"]) GoToLabel((string)arguments[0]);
            if (actionType == (int)StatementType.getType["Use On Position"]) UseOnPosition((string)arguments[0], (int)arguments[1], (int)arguments[2], (int)arguments[3]);
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
                DefaultActions.Click(xPosition, yPosition,floor,false);
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

    }
}
