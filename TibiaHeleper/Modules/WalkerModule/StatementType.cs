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
        public static Hashtable getType;

        static StatementType()
        {
            getType = new Hashtable();
            getType.Add("label", 0);
            getType.Add("check", 1);
            getType.Add("Way", 2);
            getType.Add("action", 3);
            getType.Add("Waypoint", 4);

            getType.Add("Mouse Click", 5);
            getType.Add("Say", 6);
            getType.Add("Stand", 7);
            getType.Add("Hotkey", 9);
            getType.Add("Use On Field", 10);
            getType.Add("Go To Label", 11);

        }

        public static string getTypeByValue(int value)
        {
            foreach(DictionaryEntry item in getType)
            {
                if ((int)item.Value == value) return item.Key as string;
            }
            return "";
        }
    }
}
