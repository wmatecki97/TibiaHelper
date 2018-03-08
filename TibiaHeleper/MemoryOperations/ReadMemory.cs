using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TibiaHeleper.MemoryOperations
{
    class ReadMemory
    {
        [DllImport("kernel32.dll")]
        public static extern Int32 ReadProcessMemory(IntPtr hprocess, IntPtr lpBaseAdress, [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

        public static byte[] ReadBytes(IntPtr Handle, Int64 Adress, uint BytesToRead)
        {
            IntPtr ptrBytesRead;
            byte[] buffer = new byte[BytesToRead];
            ReadProcessMemory(Handle, new IntPtr(Adress), buffer, BytesToRead, out ptrBytesRead);
            return buffer;
        }

        public static int ReadInt32(Int64 Adress, IntPtr Handle)
        {
            return BitConverter.ToInt32(ReadBytes(Handle, Adress, 4), 0);
        }

        public static string ReadString(long Adress, IntPtr Handle, uint length = 32)
        {
            return ASCIIEncoding.Default.GetString(ReadBytes(Handle, Adress, length)).Split('\0')[0];
        }
        //not sure
        public static void WriteInt32(UInt32 Adress, IntPtr Handle, byte[] lpBuffer, int bufferLength, ref int lpNumberOfBytesWritten)
        {
            WriteProcessMemory((int)Handle, (int)Adress, lpBuffer, bufferLength, ref lpNumberOfBytesWritten);
        }
        public static void WriteString(UInt32 Adress, IntPtr Handle, byte[] lpBuffer, int bufferLength, ref int lpNumberOfBytesWritten)
        {
            WriteProcessMemory((int)Handle, (int)Adress, lpBuffer, bufferLength, ref lpNumberOfBytesWritten);
        }
    }
}
