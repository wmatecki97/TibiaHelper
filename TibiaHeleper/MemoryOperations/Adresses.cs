using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibiaHeleper.MemoryOperations
{
    class Adresses
    {
        public static UInt32 MaxHPAdr { get; }
        public static UInt32 HPAdr { get; }
        public static UInt32 XORAdr { get; }
        public static UInt32 ManaAdr { get; }
        public static UInt32 MaxManaAdr { get; }
        public static UInt32 ActualSpeed { get; }
        public static UInt32 NormalSpeed { get; }

        static Adresses()
        {
            MaxHPAdr = 0x70E048;
            HPAdr = 0x70E000;
            XORAdr = 0x570458;
            MaxManaAdr = 0x57045C;
            ManaAdr = 0x57048C;
            ActualSpeed = 570418;
            NormalSpeed = 570480;
        }
    }
}
