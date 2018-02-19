using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TibiaHeleper.Modules
{
    static class ModulesManager
    {
        
        public static Healer healer { get; set; }
        public static AutoHaste autoHaste { get; set; }
        public static Sio sio { get; set; }

       // private static Thread THealer;
       // private static Thread TAutoHaste;
       // private static Thread TSio;

        private static Semaphore sem;

        static ModulesManager()
        {
            
            sem = new Semaphore(1, 1);

            
            healer = new Healer();
      //      THealer = new Thread(healer.Run);
            autoHaste = new AutoHaste();
      //      TAutoHaste = new Thread(autoHaste.Run);
            sio = new Sio();
    //        TSio = new Thread(sio.Run);
          
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

        public static void SioEnable() { enableThread((Module)sio); }
        public static void SioDisable() { disableThread((Module)sio); }

        


    }
}
