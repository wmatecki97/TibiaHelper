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
        
        public static Healer healer { get; set; }
        public static AutoHaste autoHaste;

        private static Thread THealer;
        private static Thread TAutoHaste;
        private static Semaphore sem;

        static ModuesManager()
        {
            
            sem = new Semaphore(1, 1);

            
            healer = new Healer();
            THealer = new Thread(healer.Run);
            autoHaste = new AutoHaste();
            TAutoHaste = new Thread(autoHaste.Run);
          
        }

        private static void enableThread(Module module)
        {
            sem.WaitOne();

                if (!module.working);
                    Thread t = new Thread(module.Run);
                t.Start();

            sem.Release();
                
        }

        private static void disableThread(Module module)
        {
            
            sem.WaitOne();
                module.working = false;
            sem.Release();
            
        }

        public static void HealerEnable() { enableThread((Module)healer); }
        
        public static void HealerDisable() { disableThread((Module)healer); }

        public static void AutoHasteEnable() { enableThread((Module)autoHaste); }

        public static void AutoHasteDisable() { disableThread((Module)autoHaste); }

        


    }
}
