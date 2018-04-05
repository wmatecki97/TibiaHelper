using System;
using System.Collections.Generic;

namespace TibiaHeleper.MemoryOperations
{
    class Addresses
    {
        
        //Player
        public static UInt32 MaxHPAdr { get; }
        public static UInt32 HPAdr { get; }
        public static UInt32 XORAdr { get; }
        public static UInt32 ManaAdr { get; }
        public static UInt32 MaxManaAdr { get; }
        public static UInt32 Hasted { get; }
        public static UInt32 ActualSpeed { get; }
        public static UInt32 NormalSpeed { get; }
        public static UInt32 MyXPosition { get; }
        public static UInt32 MyYPosition { get; }
        public static UInt32 MyFloorByteAddress { get; }
        public static UInt32 CapXor { get; }


        public static UInt32 FollowTargetAddress { get; }

        //list of spotted monsters persons and NPC
        public static UInt32 InformationsOfSpottedCreaturesAndPlayersSartAddress { get; } 
        public static UInt32 CreatureInformationBlockSize { get; }
        public static UInt32 CreatureHpShift { get; }// means PlayerInformationsAddress + shift is player HP
        public static UInt32 CreatureOnScreenShift { get; }
        public static UInt32 CreatureNameShift { get; }
        public static UInt32 CreatureXPositionShift { get; }
        public static UInt32 CreatureYPositionShift { get; }
        public static UInt32 CreatureFloorShift { get; }

        //Battle List
        public static UInt32 BattleListAddress { get; }
        public static UInt32 Target { get; }

        //GUI
        public static UInt32 GameWindowHeight { get; }
        public static UInt32 GameWindowHeightShift1 { get; }
        public static UInt32 GameWindowHeightShift2 { get; }
        public static UInt32 GameWindowFromLeftDistance { get; }
        public static UInt32 GameWindowFromLeftDistanceShift1 { get; }
        public static UInt32 GameWindowFromLeftDistanceShift2 { get; }
        public static UInt32 GameWindowFromLeftDistanceShift3 { get; }

        public static uint GameWindowWidth { get; }

        public static uint WindowIDOffset;
        public static List<UInt32> ThirdWindowFromTop;
        public static List<UInt32> SecondWindowFromTop;

        public static UInt32 ActualInput { get; }
        public static UInt32 ActualInputShift1 { get; }
        public static UInt32 ActualInputShift2 { get; }
        public static UInt32 ActualInputShift3 { get; }
        public static UInt32 ActualInputShift4 { get; }

        public static UInt32 LastServerInfoMessage { get; }

        static Addresses()
        {

            MaxHPAdr = 0x70E048;
            HPAdr = 0x70E000;
            XORAdr = 0x570458;
            MaxManaAdr = 0x57045C;
            ManaAdr = 0x57048C;
            ActualSpeed = 0x570418;
            NormalSpeed = 0x570480;
            Hasted = 0x570410;
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
            GameWindowHeight = 0x570744;
            GameWindowHeightShift1 = 0x30;
            GameWindowHeightShift2 = 0x20;
            GameWindowFromLeftDistance = 0x570744;
            GameWindowFromLeftDistanceShift1 = 0x30;
            GameWindowFromLeftDistanceShift2 = 0x4;
            GameWindowFromLeftDistanceShift3 = 0x4;

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

            ActualInput = 0x570744;
            ActualInputShift1 = 0x40;
            ActualInputShift2 = 0x40;
            ActualInputShift3 = 0x2C;
            ActualInputShift4 = 0x0;

            LastServerInfoMessage = 0x5C3DC0;


        }
    }
}
