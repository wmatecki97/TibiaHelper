using System;
using TibiaHeleper.MemoryOperations;

namespace TibiaHeleper.Storage
{
    [Serializable]
    class Creature
    {
        public string name { get; }
        public int id { get; }
        public int HPPercent { get { return GetData.getCreatureHPPercent(adress); } }
        public bool onScreen { get { return GetData.isCreatureOnScreen(adress); } }
        public UInt32 adress { get; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }

        public Creature(int id, UInt32 adress)
        {
            this.id = id;
            this.adress = adress;
            this.name = GetData.GetCreatureName(adress);
        }

    }
}
