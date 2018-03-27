using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibiaHeleper.Modules.WalkerModule
{
    [Serializable]
    class Condition :WalkerStatement, ICloneable
    {
        public short item1;
        public short comparator;
        public short item2;
        public short connector;
        public List<object> args;

        public Condition()
        {
            args = new List<object>();
            connector = StatementType.conditionElement["Not set"];
        }
    }
}
