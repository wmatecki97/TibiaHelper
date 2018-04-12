using System;
using TibiaHeleper.MemoryOperations;

namespace TibiaHeleper.Storage
{
    [Serializable]
    class Creature
    {
        public string name { get; }
        public int id { get; }
        public int HPPercent { get { return GetData.getCreatureHPPercent(Address); } }
        public bool onScreen { get { return GetData.isCreatureOnScreen(Address); } }
        public uint Address { get; }
        public int XPosition { get { return GetData.getCreatureXPosition(Address); } }
        public int YPosition { get { return GetData.getCreatureYPosition(Address); } }
        public int Floor { get { return GetData.getCreatureFloor(Address); } }

        public Creature(int id, uint Address)
        {
            this.id = id;
            this.Address = Address;
            this.name = GetData.GetCreatureName(Address);
        }

    }
}
