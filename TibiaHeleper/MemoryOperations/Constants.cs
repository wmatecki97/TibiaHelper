using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibiaHeleper.MemoryOperations
{
    public static class Constants
    {

        public static int GameWindowWidthSquares { get; set; }
        public static int GameWindowHeightSquares { get; set; }

        static Constants()
        {
            GameWindowHeightSquares = 11;
            GameWindowWidthSquares = 15;
        }


    }
}
