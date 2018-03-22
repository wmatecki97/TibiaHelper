using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibiaHeleper.Modules.WalkerModule
{
    [Serializable]
    public class WalkerStatement: ICloneable
    {

        public int type { get; set; }

        public string name { get; set; }

        public virtual object Clone()
        {
            return null;
        }
    }


}
