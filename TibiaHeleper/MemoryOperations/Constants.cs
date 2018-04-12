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
        public static int ShieldYOffset { get; }
        public static int ShieldXOffset { get;}
        public static int MaximizeEQButtonXOffset { get; }
        public static int MaximizeEQButtonYOffset { get; }
        public static int AttackModeXOffset { get; }
        public static int[] AttackMode_YOffset { get; }
        public static int FollowTargetModeXOffset { get; }
        public static int[] FollowTargetModeYOffset { get; }


        static Constants()
        {

            GameWindowHeightSquares = 11;
            GameWindowWidthSquares = 15;
            ShieldYOffset = 75;
            ShieldXOffset = -73;
            MaximizeEQButtonXOffset = -160;
            MaximizeEQButtonYOffset = 10;

            AttackModeXOffset = -40;
            AttackMode_YOffset = new int[3];
            AttackMode_YOffset[0] = -5;
            AttackMode_YOffset[1] = 15;
            AttackMode_YOffset[2] = 35;

            FollowTargetModeXOffset = -20;
            FollowTargetModeYOffset = new int[2];
            FollowTargetModeYOffset[0] = 10;
            FollowTargetModeYOffset[1] = 30;
        }


    }
}
