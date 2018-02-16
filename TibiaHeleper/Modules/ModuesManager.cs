using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TibiaHeleper.Modules
{
    static class ModuesManager
    {
        static Thread THealer;

        static ModuesManager()
        {
            THealer = new Thread(Modules.Healer.Run);
        }

        public static void HealerDisable()
        {
            Healer.Stop();
            THealer.Abort();//for safety
            THealer = new Thread(Modules.Healer.Run);
        }

        public static void HealerRun()
        {
            if(!Healer.isWorking())
                THealer.Start();
        }
    }
}
