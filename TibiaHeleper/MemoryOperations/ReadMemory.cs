using System;
using System.Runtime.InteropServices;
using System.Text;

namespace TibiaHeleper.MemoryOperations
{
    class ReadMemory
    {
        [DllImport("kernel32.dll")]
        public static extern Int32 ReadProcessMemory(IntPtr hprocess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, uint size, out IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

        public static byte[] ReadBytes(IntPtr Handle, Int64 Address, uint BytesToRead)
        {
            IntPtr ptrBytesRead;
            byte[] buffer = new byte[BytesToRead];
            ReadProcessMemory(Handle, new IntPtr(Address), buffer, BytesToRead, out ptrBytesRead);
            return buffer;
        }

        public static int ReadInt32(Int64 Address, IntPtr Handle)
        {
            return BitConverter.ToInt32(ReadBytes(Handle, Address, 4), 0);
        }

        public static string ReadString(long Address, IntPtr Handle, uint length = 32)
        {
            if (length > 32)
            {
                var a = ASCIIEncoding.Default.GetString(ReadBytes(Handle, Address, length)).Split('\0');
                string result = "";
                foreach (var i in a)
                {
                    if (i == ",")
                        break;
                    result += i;
                }
                return result;
            }

            return ASCIIEncoding.Default.GetString(ReadBytes(Handle, Address, length)).Split('\0')[0];
        }
        //not sure
        public static void WriteInt32(uint Address, IntPtr Handle, byte[] lpBuffer, int bufferLength, ref int lpNumberOfBytesWritten)
        {
            WriteProcessMemory((int)Handle, (int)Address, lpBuffer, bufferLength, ref lpNumberOfBytesWritten);
        }
        public static void WriteString(uint Address, IntPtr Handle, byte[] lpBuffer, int bufferLength, ref int lpNumberOfBytesWritten)
        {
            WriteProcessMemory((int)Handle, (int)Address, lpBuffer, bufferLength, ref lpNumberOfBytesWritten);
        }
    }
}
