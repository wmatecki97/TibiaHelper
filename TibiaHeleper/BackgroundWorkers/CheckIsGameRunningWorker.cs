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
    public class CheckIsGameRunningWorker : BackgroundWorker
    {
        public override void Run()
        {
            while (working)
            {
                if (!GetData.isGameOpened())
                    Environment.Exit(0);
                Thread.Sleep(1000 * refreshFrequency);
            }
            finished = true;
        }

    }
}
