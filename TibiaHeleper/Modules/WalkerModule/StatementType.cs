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
            getType.Add("waypoint", 0);
            getType.Add("action", 1);
            getType.Add("check", 2);
            getType.Add("label", 3);
            getType.Add("goto", 4);

            getType.Add("mouseClick", 5);
            getType.Add("say", 6);
            getType.Add("stand", 7);
            getType.Add("stand", 8);
            getType.Add("hotkey", 9);

        }
    }
}
