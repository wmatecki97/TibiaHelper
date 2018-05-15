using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TibiaHeleper.MemoryOperations;
using TibiaHeleper.Modules;
using TibiaHeleper.Windows;

namespace TibiaHeleper.BackgroundWorkers
{
    class WhoIsLoggedInWorker : BackgroundWorker
    {

        private int _loggedID;
        public static Stopwatch sw { get; set; }

        public override void Run()
        {

            sw = new Stopwatch();
            sw.Start();
            bool modulesDisabled = false;

            while (working)
            {
                GetData.WhoAmI();
                
                modulesDisabled = ManageModules(modulesDisabled);

                WindowsManager.menu.Update();



                if (sw.ElapsedMilliseconds > 60000) // every 1 min reset all spotted creatures list
                {
                    GetData.ResetAllSpottedCreatureList();
                    GetData.ActualizeAllSpottedCreaturesList();
                    sw.Restart();
                }



                Thread.Sleep(1000 * refreshFrequency);
                WindowsManager.walkerWindow.Update();
            }
            finished = true;
        }

        private static bool ManageModules(bool modulesDisabled)
        {
            bool a = GetData.isAnybodyLoggedIn;
            if (!GetData.isAnybodyLoggedIn)
            {
                if (!modulesDisabled)
                {
                    ModulesManager.DisableAllWorkingModules();
                    modulesDisabled = true;
                    GetData.ResetAllSpottedCreatureList();
                }
            }
            else if (modulesDisabled)
            {
                ModulesManager.EnableAllDisabledModules();
                modulesDisabled = false;
            }

            return modulesDisabled;
        }


        public WhoIsLoggedInWorker()
        {
            refreshFrequency = 1;
            _loggedID = -1;
        }
    }
}
