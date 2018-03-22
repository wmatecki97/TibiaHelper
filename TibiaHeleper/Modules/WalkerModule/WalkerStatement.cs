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
        /// <summary>
        /// 0 - waypoint
        /// 1 - action
        /// 2 - check
        /// 3 - goto
        /// </summary>
        public int type { get; set; }

        public string name { get; set; }

        public virtual object Clone()
        {
            return null;
        }
    }


}
