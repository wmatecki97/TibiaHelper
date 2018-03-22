using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using TibiaHeleper.Modules.Targeting;
using TibiaHeleper.Modules.WalkerModule;

namespace TibiaHeleper.Modules
{
    static class ModulesManager
    {

        public static Healer healer { get; set; }
        public static AutoHaste autoHaste { get; set; }
        public static Sio sio { get; set; }
        public static AntyPalalyse antyParalyse { get; set; }
        public static Targeter targeting { get; set; }
        public static Walker walker { get; set; }
        public static Tracker tracker { get; set; }

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
            antyParalyse = new AntyPalalyse();
            targeting = new Targeter();
            walker = new Walker();
            tracker = new Tracker();

        }
        /// <summary>
        /// Enables thread without checking if is working. Used when old same module with thread working got working=false and module has been replaced by new one
        /// </summary>
        /// <param name="module"></param>
        public static void HardEnableThread(Module module)
        {
            sem.WaitOne();

                Thread t = new Thread(module.Run);
                module.stopped = false;
                module.working = true;
                t.Start();

            sem.Release();

        }
        private static void enableThread(Module module)
        {
            sem.WaitOne();
            if (!module.working)
            {
                Thread t = new Thread(module.Run);
                while (!module.stopped) ;//waiting for thread finish

                module.stopped = false;
                module.working = true;
                t.Start();
            }
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

        public static void AntyParalyseEnable() { enableThread((Module)antyParalyse); }
        public static void AntyParalyseDisable() { disableThread((Module)antyParalyse); }

        public static void TargetingEnable() { enableThread((Module)targeting); }
        public static void TargetingDisable() { disableThread((Module)targeting); }

        public static void WalkerEnable() {  enableThread((Module)walker); }
        public static void WalkerDisable() { disableThread((Module)walker); }

        public static void TrackerEnable() { enableThread((Module)tracker); }
        public static void TrackerDisable() { disableThread((Module)tracker); }

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
