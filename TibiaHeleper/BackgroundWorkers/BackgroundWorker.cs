using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibiaHeleper.BackgroundWorkers
{
    public class BackgroundWorker
    {
        public int TimeToWait;
        public bool working { get; set; }
        public bool finished { get; set; }

        public virtual void Run() { }
        
        public BackgroundWorker()
        {
            TimeToWait = 2;
            working = false;
            finished = false;
        }
    }
}
