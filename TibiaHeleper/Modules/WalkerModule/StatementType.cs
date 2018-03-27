using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibiaHeleper.Modules.WalkerModule
{
    public static class StatementType
    {
        public static Dictionary<string, int> getType;
        public static string getTypeName(int value)
        {
            return getType.FirstOrDefault(x => x.Value == value).Key;
        }
        public static Dictionary<string, short> conditionElement;
        public static string getConditionElementName(params short[] args)
        {
            string result = "";
            foreach(short value in args)
            {
                result += " " + conditionElement.FirstOrDefault(x => x.Value == value).Key;
            }
            return result;
        }

        static StatementType()
        {
            conditionElement = new Dictionary<string, short>();

            conditionElement.Add("Not set", 0);
            conditionElement.Add("And", 1);
            conditionElement.Add("Or", 2);

            conditionElement.Add(">", 3);
            conditionElement.Add("<", 4);
            conditionElement.Add("=", 5);
            conditionElement.Add("!=", 6);
            conditionElement.Add("HP", 7);
            conditionElement.Add("Mana", 8);
            conditionElement.Add("Cap", 9);
            conditionElement.Add("Item count", 10);
            conditionElement.Add("Value", 11);

            getType = new Dictionary<string, int>();
            getType.Add("label", 0);
            getType.Add("Way", 1);
            getType.Add("action", 2);
            getType.Add("Waypoint", 3);

            getType.Add("Condition", 4);
            getType.Add("Mouse Click", 5);
            getType.Add("Say", 6);
            getType.Add("Stand", 7);
            getType.Add("Hotkey", 9);
            getType.Add("Use On Field", 10);
            getType.Add("Go To Label", 11);


        }


        public static string getTypeByValue(int value)
        {
            foreach(KeyValuePair<string,int> item in getType)
            {
                if (item.Value == value) return item.Key as string;
            }
            return "";
        }
    }
}
