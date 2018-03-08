using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TibiaHeleper.Storage;

namespace TibiaHeleper.MemoryOperations
{
    class GetData
    {
        static Process Tibia;
        public static IntPtr Handle { get; set; }
        static UInt32 Base;
        private static List<Creature> allSpottedCreaturesList;
        private static UInt32 lastSpottedCreatureAdress;

        static GetData()
        {
            allSpottedCreaturesList = new List<Creature>();
            lastSpottedCreatureAdress = Adresses.InformationsOfSpottedCreaturesAndPlayersSartAdress;
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
        /// returns 32bit integer from adress given
        /// </summary>
        /// <param name="adress"></param>
        /// <returns></returns>
        public static int getIntegerDataFromAdress(UInt32 adress)
        {
            return ReadMemory.ReadInt32(Base + adress, Handle); ;
        }
        public static string getStringFromAdress(UInt32 adress)
        {
            return ReadMemory.ReadString(Base + adress, Handle);
        }
        public static void writeInt32(UInt32 adress, int toWrite)
        {
            byte[] lpBuffer = BitConverter.GetBytes(toWrite);
            int bytesWritten=0;
            ReadMemory.WriteInt32(adress + Base, Handle, lpBuffer, 4, ref bytesWritten);
        }
        public static Process getProcess()
        {
            return Tibia;
        }

       

        public static int getXOR() { return getIntegerDataFromAdress(Adresses.XORAdr); }
        public static int getHP() {
            int xor = getXOR();
            int hp = getIntegerDataFromAdress(Adresses.HPAdr);
            return xor ^ hp;
        }
        public static int getMana() {
            int xor = getXOR();
            int mana = getIntegerDataFromAdress(Adresses.ManaAdr);
            return xor ^ mana;
        }
        public static int getActualSpeed() { return getIntegerDataFromAdress( Adresses.ActualSpeed); }
        public static int getNormalSpeed() { return getIntegerDataFromAdress( Adresses.NormalSpeed); }

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

        public static string GetCreatureName(UInt32 adress)
        {
            return getStringFromAdress(adress + Adresses.CreatureNameShift);
        }
        public static bool isCreatureOnScreen(UInt32 playerAdress)
        {
            return (getIntegerDataFromAdress(playerAdress + Adresses.CreatureOnScreenShift)==1);                
        }
        public static int getCreatureHPPercent(UInt32 CreatureAdress)
        {
            return getIntegerDataFromAdress(CreatureAdress + Adresses.CreatureHpShift);
        }

        public static int GetDistance(Creature creature) //TO IMPLEMENT
        {
            int Xpos = creature.XPosition;
            int Ypos = creature.YPosition;
            return 0;
        }


        public static string getActualInput()
        {
            return getStringFromAdress(Adresses.InputAdrWithoutBase-Base);
        }
        
        public static void ActualizeAllSpottedCreaturesList()
        {
            bool wasCreatureSpotted = true;
            int id;
            UInt32 CreatureInformationBlockSize = Adresses.CreatureInformationBlockSize;
            while (wasCreatureSpotted)
            {
                if ((id = getIntegerDataFromAdress(lastSpottedCreatureAdress)) != 0)
                {
                    Creature creature = new Creature(id, lastSpottedCreatureAdress);
                    lock (allSpottedCreaturesList)
                    {
                        allSpottedCreaturesList.Insert(0, creature);
                    }
                    lastSpottedCreatureAdress += CreatureInformationBlockSize;
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
        public static void Attack(Creature creature)
        {
            writeInt32(Adresses.Target, creature.id);
        }
        public static int getTargetID()
        {
            return getIntegerDataFromAdress(Adresses.Target);
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
