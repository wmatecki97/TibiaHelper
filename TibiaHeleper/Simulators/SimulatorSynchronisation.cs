using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TibiaHeleper.Simulators
{
    public static class SimulatorSynchronisation
    {
        public static Semaphore semaphore;

        static SimulatorSynchronisation()
        {
            semaphore = new Semaphore(1,1);            
        }
    }
}
