using System;
using System.Collections.Generic;

namespace TibiaHeleper.MemoryOperations
{
    class Addresses
    {
        
        //Player
        public static uint MaxHPAdr { get; }
        public static uint HPAdr { get; }
        public static uint XORAdr { get; }
        public static uint ManaAdr { get; }
        public static uint MaxManaAdr { get; }
        public static uint MyFlags { get; }
        public static uint ActualSpeed { get; }
        public static uint NormalSpeed { get; }
        public static uint MyXPosition { get; }
        public static uint MyYPosition { get; }
        public static uint MyFloorByteAddress { get; }
        public static uint CapXor { get; }


        public static uint FollowTargetAddress { get; }

        //list of spotted monsters persons and NPC
        public static uint InformationsOfSpottedCreaturesAndPlayersSartAddress { get; } 
        public static uint CreatureInformationBlockSize { get; }
        public static uint CreatureHpShift { get; }// means PlayerInformationsAddress + shift is player HP
        public static uint CreatureOnScreenShift { get; }
        public static uint CreatureNameShift { get; }
        public static uint CreatureXPositionShift { get; }
        public static uint CreatureYPositionShift { get; }
        public static uint CreatureFloorShift { get; }

        //Battle List
        public static uint BattleListAddress { get; }
        public static uint Target { get; }

        //GUI
        public static List<uint> GameWindowHeight { get; }
        public static List<uint> GameWindowFromLeftDistance { get; }

        public static uint GameWindowWidth { get; }

        public static uint WindowIDOffset;
        public static List<uint> ThirdWindowFromTop;
        public static List<uint> SecondWindowFromTop;

        public static List<uint> ActualInput { get; }

        public static uint LastServerInfoMessage { get; }

        static Addresses()
        {

            MaxHPAdr = 0x70E048;
            HPAdr = 0x70E000;
            XORAdr = 0x570458;
            MaxManaAdr = 0x57045C;
            ManaAdr = 0x57048C;
            ActualSpeed = 0x570418;
            NormalSpeed = 0x570480;
            MyFlags = 0x570410;
            MyXPosition = 0x70E054;
            MyYPosition = 0x70E058;
            MyFloorByteAddress = 0x70E05C;
            CapXor = 0x70E040;

            FollowTargetAddress = 0x5815B8;

            InformationsOfSpottedCreaturesAndPlayersSartAddress = 0x76A0B0;
            CreatureInformationBlockSize = 0xDC;
            CreatureHpShift = 0x8C;
            CreatureOnScreenShift = 0xA4;
            CreatureNameShift = 0x4;
            CreatureXPositionShift = 0x2c;
            CreatureYPositionShift = 0x28;
            CreatureFloorShift = 0x24;


            BattleListAddress = 0x0030B9F0;
            Target = 0x570488;

            //GUI
            GameWindowHeight = new List<uint>();
            GameWindowHeight.Add(0x570744);
            GameWindowHeight.Add(0x30);
            GameWindowHeight.Add(0x20);
            GameWindowFromLeftDistance = new List<uint>();
            GameWindowFromLeftDistance.Add(0x570744);
            GameWindowFromLeftDistance.Add(0x30);
            GameWindowFromLeftDistance.Add(0x4);
            GameWindowFromLeftDistance.Add(0x4);

            GameWindowWidth = 0x581F90;

            WindowIDOffset = 0x8;

            ThirdWindowFromTop = new List<uint>();
            ThirdWindowFromTop.Add(0x7B00D4);
            ThirdWindowFromTop.Add(0xD0);
            ThirdWindowFromTop.Add(0x10);
            ThirdWindowFromTop.Add(0x10);
            ThirdWindowFromTop.Add(0x18);

            SecondWindowFromTop = new List<uint>();
            SecondWindowFromTop.Add(0x7B00D4);
            SecondWindowFromTop.Add(0x88);
            SecondWindowFromTop.Add(0x58);
            SecondWindowFromTop.Add(0xC);
            SecondWindowFromTop.Add(0x10);
            SecondWindowFromTop.Add(0x18);

            ActualInput = new List<uint>();
            ActualInput.Add(0x570744);
            ActualInput.Add(0x40);
            ActualInput.Add(0x40);
            ActualInput.Add(0x2C);
            ActualInput.Add(0x0);

            LastServerInfoMessage = 0x5C3DC0;


        }
    }
}
