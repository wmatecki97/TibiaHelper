using System;
using System.Collections.Generic;
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

        public static int getDataFromAdress(UInt32 adress)
        {
            return ReadMemory.ReadInt32(Base + adress, Handle); ;
        }

        public static Process getProcess()
        {
            return Tibia;
        }
    }
}
