using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibiaHeleper.Modules.Targeting
{
    class Targeting : Module
    {
        public bool working { get; set; }
        public bool stopped { get; set; }


        public void Run()
        {
            while (working)
            {

            }
        }

        public void Stop()
        {
            working = false;
        }
    }
}
