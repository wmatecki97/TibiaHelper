using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TibiaHeleper.MemoryOperations;

namespace TibiaHeleper.Storage
{
    class Creature
    {
        public string name { get; }
        public int id { get; }
        public int HPPercent { get { return GetData.getCreatureHPPercent(adress); } }
        public bool onScreen { get { return GetData.isCreatureOnScreen(adress); } }
        private UInt32 adress;

        public Creature(int id, UInt32 adress)
        {
            this.id = id;
            this.adress = adress;
            this.name = GetData.GetCreatureName(adress);
        }

    }
}
