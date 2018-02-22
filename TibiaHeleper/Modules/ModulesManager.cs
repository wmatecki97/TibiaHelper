using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
            autoHaste = new AutoHaste();
            sio = new Sio();
          
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
                module.Stop();
            sem.Release();
            
        }

        public static void HealerEnable() { enableThread((Module)healer); }
        public static void HealerDisable() { disableThread((Module)healer); }

        public static void AutoHasteEnable() { enableThread((Module)autoHaste); serialize(); }
        public static void AutoHasteDisable() { disableThread((Module)autoHaste); }

        public static void SioEnable() { enableThread((Module)sio); }
        public static void SioDisable() { disableThread((Module)sio); }

        public static void serialize()
        {
            // Serialize the object data to a file
            Stream stream = File.Open("AnimalData.dat", FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();

            // Send the object data to the file
            bf.Serialize(stream, autoHaste);
            stream.Close();


        }


    }
}
