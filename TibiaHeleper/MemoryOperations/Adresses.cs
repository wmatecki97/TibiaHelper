using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibiaHeleper.MemoryOperations
{
    class Adresses
    {
        //GUI
        public static UInt32 InputAdrWithoutBase { get; }

        //Player
        public static UInt32 MaxHPAdr { get; }
        public static UInt32 HPAdr { get; }
        public static UInt32 XORAdr { get; }
        public static UInt32 ManaAdr { get; }
        public static UInt32 MaxManaAdr { get; }
        public static UInt32 ActualSpeed { get; }
        public static UInt32 NormalSpeed { get; }

        //list of spotted monsters persons and NPC
        public static UInt32 InformationsOfSpottedCreaturesAndPlayersSartAdress { get; } 
        public static UInt32 PlayerInformationBlockSize { get; }
        public static UInt32 PlayerHpShift { get; }// means PlayerInformationsAdress + shift is player HP
        public static UInt32 PlayerOnScreenShift { get; }

        static Adresses()
        {
            InputAdrWithoutBase= 0x09924D10;

            MaxHPAdr = 0x70E048;
            HPAdr = 0x70E000;
            XORAdr = 0x570458;
            MaxManaAdr = 0x57045C;
            ManaAdr = 0x57048C;
            ActualSpeed = 0x570418;
            NormalSpeed = 0x570480;


            InformationsOfSpottedCreaturesAndPlayersSartAdress = 0x76A0B4;
            PlayerInformationBlockSize = 0xDC;
            PlayerHpShift = 0x88;
            PlayerOnScreenShift = 0xA0;
        }
    }
}
