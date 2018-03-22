using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TibiaHeleper.MemoryOperations;
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
                if(loggedID!=GetData.MyID)
                {
                    loggedID = GetData.MyID;
                    WindowsManager.menu.Update();
                    GetData.ResetAllSpottedCreatureList();
                    GetData.ActualizeAllSpottedCreaturesList();
                }
                Thread.Sleep(1000 * refreshFrequency);
            }
            finished = true;
        }

        public WhoIsLoggedInWorker()
        {
            refreshFrequency = 10;
            loggedID = -1;
        }
    }
}
