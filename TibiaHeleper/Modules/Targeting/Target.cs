using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibiaHeleper.Modules.Targeting
{
    class Target
    {
        public string name { get; set; }
        public int priority { get; set; }
        public string action { get; set; }

        public bool diagonal { get; set; }
        public bool HPMoreImportantThanDistance { get; set; }
    }
}
