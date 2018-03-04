using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibiaHeleper.Modules
{
    interface Module
    {
        void Run();
        bool working { get; set; }
        bool stopped { get; set; }
        void Stop();
    }
}
