using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TibiaHeleper.Storage;
using System.Linq;

namespace TibiaHeleper.MemoryOperations
{
    class GetData
    {
        static Process Tibia;
        public static IntPtr Handle { get; set; }
        static UInt32 Base;
        private static List<Creature> allSpottedCreaturesList;
        private static UInt32 lastSpottedCreatureAddress;
        public static Creature LoggedCharacter;

        static GetData()
        {
            allSpottedCreaturesList = new List<Creature>();
            lastSpottedCreatureAddress = Addresses.InformationsOfSpottedCreaturesAndPlayersSartAddress;
        }




        /// <summary>
        /// looking for tibia.exe returns false when Tibia.exe not found
        /// </summary>
        /// <returns></returns>
        public static bool inject()
        {

            Process[] processes = Process.GetProcessesByName("Tibia");
            if (processes.Length > 0)
            {
                Tibia = processes[0];
            }
            else return false;
            Handle = Tibia.Handle;
            Base = (UInt32)Tibia.MainModule.BaseAddress;
            return true;
        }
        /// <summary>
        /// returns 32bit integer from Address given
        /// </summary>
        /// <param name="Address"></param>
        /// <returns></returns>
        public static int getIntegerDataFromAddress(UInt32 Address)
        {
            return ReadMemory.ReadInt32(Base + Address, Handle);
        }
        public static int getByteAsIntegerFromAddress(UInt32 Address)
        {
            return (int)ReadMemory.ReadBytes(Handle, (long)(Base + Address), 1).FirstOrDefault();
        }
        public static int getIntegerDataFromDynamicAddress(UInt32 Address)
        {
            return ReadMemory.ReadInt32(Address, Handle);
        }
        public static string getStringFromAddress(UInt32 Address, UInt32 length = 32)
        {
            return ReadMemory.ReadString(Base + Address, Handle, length);
        }
        public static string getStringFromDynamicAddress(UInt32 Address, UInt32 length = 32)
        {
            return ReadMemory.ReadString(Address, Handle, length);
        }
        public static void writeInt32(UInt32 Address, int toWrite)
        {
            byte[] lpBuffer = BitConverter.GetBytes(toWrite);
            int bytesWritten = 0;
            ReadMemory.WriteInt32(Address + Base, Handle, lpBuffer, 4, ref bytesWritten);
        }
        public static Process getProcess()
        {
            return Tibia;
        }



        public static int XOR { get { return getIntegerDataFromAddress(Addresses.XORAdr); } }
        public static int MyHP {
            get
            {
                int xor = XOR;
                int hp = getIntegerDataFromAddress(Addresses.HPAdr);
                return xor ^ hp;
            }
        }
        public static int MyMana {
            get
            {
                int xor = XOR;
                int mana = getIntegerDataFromAddress(Addresses.ManaAdr);
                return xor ^ mana;
            }
        }
        public static bool AmIHasted { get { return getIntegerDataFromAddress(Addresses.Hasted) % Flags.AmIInPZ == Flags.AmIHasted; } }
        public static int MyActualSpeed { get { return getIntegerDataFromAddress( Addresses.ActualSpeed); } }
        public static int MyNormalSpeed { get { return getIntegerDataFromAddress( Addresses.NormalSpeed); } }
        public static int MyXPosition { get { return getIntegerDataFromAddress(Addresses.MyXPosition); } }
        public static int MyYPosition { get { return getIntegerDataFromAddress(Addresses.MyYPosition); } }
        public static int MyFloor { get { return getByteAsIntegerFromAddress(Addresses.MyFloorByteAddress); } }

        public static Creature getPlayer(string playerName)
        {
            ActualizeAllSpottedCreaturesList();
            lock (allSpottedCreaturesList)
            {
                foreach (Creature creature in allSpottedCreaturesList)
                {
                    if (creature.name == playerName)
                        return creature;
                }
            }
            return null;
        }

        public static string GetCreatureName(UInt32 Address)
        {
            return getStringFromAddress(Address + Addresses.CreatureNameShift);
        }
        public static bool isCreatureOnScreen(UInt32 playerAddress)
        {
            return (getIntegerDataFromAddress(playerAddress + Addresses.CreatureOnScreenShift)==1);                
        }
        public static int getCreatureHPPercent(UInt32 CreatureAddress)
        {
            return getIntegerDataFromAddress(CreatureAddress + Addresses.CreatureHpShift);
        }
        public static int getCreatureXPosition(UInt32 CreatureAddress)
        {
            return getIntegerDataFromAddress(CreatureAddress + Addresses.CreatureXPositionShift);
        }
        public static int getCreatureYPosition (UInt32 CreatureAddress)
        {
            return getIntegerDataFromAddress(CreatureAddress + Addresses.CreatureYPositionShift);
        }
        public static int getCreatureFloor(UInt32 CreatureAddress)
        {
            return getByteAsIntegerFromAddress(CreatureAddress + Addresses.CreatureFloorShift);

        }


        public static int GetDistance(Creature creature) //TO IMPLEMENT
        {
            return Math.Abs(creature.XPosition - MyXPosition) + Math.Abs(creature.YPosition - MyYPosition);
        }


        
        public static void ActualizeAllSpottedCreaturesList()
        {
            bool wasCreatureSpotted = true;
            int id;
            UInt32 CreatureInformationBlockSize = Addresses.CreatureInformationBlockSize;
            while (wasCreatureSpotted)
            {
                if ((id = getIntegerDataFromAddress(lastSpottedCreatureAddress)) != 0)
                {
                    Creature creature = new Creature(id, lastSpottedCreatureAddress);
                    lock (allSpottedCreaturesList)
                    {
                        allSpottedCreaturesList.Insert(0, creature);
                    }
                    lastSpottedCreatureAddress += CreatureInformationBlockSize;
                }
                else wasCreatureSpotted = false;
            }            
        }
        public static List<Creature> GetBattleList()
        {
            List<Creature> battleList = new List<Creature>();
            ActualizeAllSpottedCreaturesList();
            lock (allSpottedCreaturesList)
            {
                foreach (Creature creature in allSpottedCreaturesList)
                {
                    if (creature.onScreen)
                        battleList.Add(creature);                        
                }
            }
            return battleList;
        }
        public static int getTargetID()
        {
            return getIntegerDataFromAddress(Addresses.Target);
        }
  /*      public static bool FollowTarget
        {
            get {return getIntegerDataFromAddress(Addresses.FollowTargetAddress) == 1; }
            set { writeInt32(Addresses.FollowTargetAddress, value == false ? 0 : 1); }
        }

    */
        
        public static int getGameWindowHeight()
        {
            UInt32 first = (UInt32)getIntegerDataFromAddress(Addresses.GameWindowHeight);
            UInt32 second = (UInt32)getIntegerDataFromDynamicAddress(first + Addresses.GameWindowHeightShift1);
            return getIntegerDataFromDynamicAddress(second + Addresses.GameWindowHeightShift2);
        }
        public static int getGameWindowDistanceFromLeft()
        {
            UInt32 first = (UInt32)getIntegerDataFromAddress(Addresses.GameWindowFromLeftDistance);
            UInt32 second = (UInt32)getIntegerDataFromDynamicAddress(first + Addresses.GameWindowFromLeftDistanceShift1);
            UInt32 third = (UInt32)getIntegerDataFromDynamicAddress(second + Addresses.GameWindowFromLeftDistanceShift2);
            return getIntegerDataFromDynamicAddress(third + Addresses.GameWindowFromLeftDistanceShift3);
        }


        public static string getActualInput()
        {
            UInt32 first = (UInt32)getIntegerDataFromAddress(Addresses.ActualInput);
            UInt32 second = (UInt32)getIntegerDataFromDynamicAddress(first + Addresses.ActualInputShift1);
            UInt32 third = (UInt32)getIntegerDataFromDynamicAddress(second + Addresses.ActualInputShift2);
            UInt32 fourth = (UInt32)getIntegerDataFromDynamicAddress(third + Addresses.ActualInputShift3);
            return getStringFromDynamicAddress(fourth + Addresses.ActualInputShift4);
        }

        public static void sendInput(string inputString)
        {
            string temp = getActualInput();
            byte[] bytes = Encoding.ASCII.GetBytes(inputString);
            int bufferLength = bytes.Length;
            int numberOfBytesWritten = 0;

            ReadMemory.WriteString(Base, Handle, bytes, bufferLength, ref numberOfBytesWritten);
        }
    
    }
}
