using System;
using System.Collections.Generic;
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

        private int loggedID;

        public override void Run()
        {
            while (working)
            {
                GetData.WhoAmI();
                if(GetData.Me!=null && loggedID!=GetData.MyID)
                {
                    loggedID = GetData.MyID;
                    
                    WindowsManager.menu.Update();

                    GetData.ResetAllSpottedCreatureList();
                    GetData.ActualizeAllSpottedCreaturesList();
                }
                Thread.Sleep(1000 * refreshFrequency);
                WindowsManager.walkerWindow.Update();
            }
            finished = true;
        }

        public WhoIsLoggedInWorker()
        {
            refreshFrequency = 3;
            loggedID = -1;
        }
    }
}
