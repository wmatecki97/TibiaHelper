using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibiaHeleper.MemoryOperations
{
    class GetData
    {
        static Process Tibia;
        static IntPtr Handle;
        static UInt32 Base;
        static UInt32 BlockOfInformationOfPlayer;

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

        public static UInt32 getPersonLastOcurranceAdress(string characterName, ref bool found, UInt32 startAdress=0x0)
        {
            UInt32 adress;
            if (startAdress == 0x0)
                adress = Adresses.InformationsOfSpottedCreaturesAndPlayersSartAdress;
            else
                adress = startAdress;

            UInt32 sizeOfBlock = Adresses.PlayerInformationBlockSize;
            string name = getStringFromAdress(adress);
            UInt32 resultAdress = 0x0;
            found = false;

            while (name != "") //Person can occure few time if relog
            {
                name = getStringFromAdress(adress);
                if (name == characterName)
                {
                    resultAdress = adress;
                    found = true;
                }
                adress += sizeOfBlock;
            }
            return found ? resultAdress : adress ;
        }

        public static bool isPlayerOnScreen(UInt32 playerAdress)
        {
            return (getIntegerDataFromAdress(playerAdress + Adresses.PlayerOnScreenShift)==1);                
        }

        public static int getPlayerHPPercent(UInt32 playerAdress)
        {
            return getIntegerDataFromAdress(playerAdress + Adresses.PlayerHpShift);
        }
    
    }
}
