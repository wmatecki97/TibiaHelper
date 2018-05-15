using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TibiaHeleper.BackgroundWorkers
{
    public static class WorkersManager
    {
        private static List<Thread> _workersThreadsList;
        private static List<BackgroundWorker> _workersList;

        private static void addWorker(BackgroundWorker worker)
        {
            Thread t = new Thread(worker.Run);
            _workersList.Add(worker);
            worker.working = true;
            t.Start();
        }

        static WorkersManager()
        {
            _workersList = new List<BackgroundWorker>();
            _workersThreadsList = new List<Thread>();

        }

        public static void EnvironmentGettingSterted()
        {
            addWorker(new CheckIsGameRunningWorker());
            addWorker(new WhoIsLoggedInWorker());
        }


    }
}
