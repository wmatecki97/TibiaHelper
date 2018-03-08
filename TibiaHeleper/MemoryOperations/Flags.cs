using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibiaHeleper.MemoryOperations
{
    public static class Flags
    {
        public static int PlayerOnScreen { get; }
        public static int PlayerOnScreenEdge { get; }
        public static int PlayerOutOfScreen { get; }


        static Flags()
        {
            PlayerOnScreen = 0;
            PlayerOnScreenEdge = 256;
            PlayerOutOfScreen = 127;
        }
    }
}
