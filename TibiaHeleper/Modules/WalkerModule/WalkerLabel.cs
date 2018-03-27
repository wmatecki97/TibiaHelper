using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibiaHeleper.Modules.WalkerModule
{
    [Serializable]
    public class WalkerLabel : WalkerStatement, ICloneable
    {
        public WalkerLabel(string name)
        {
            this.name = name;
            this.type = StatementType.getType["label"];
        }

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
