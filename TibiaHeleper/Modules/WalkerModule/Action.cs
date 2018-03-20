using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibiaHeleper.Modules.WalkerModule
{
    public class Action : WalkerStatement, ICloneable
    {
        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
