namespace TibiaHeleper.MemoryOperations
{
    public static class Flags
    {
        public static int AmIInPZ {get;}
        public static int AmIHasted { get; }

        public static int FollowtTargetTrue { get; }
        public static int FollowTargetFalse { get; }

        public static int PlayerOnScreen { get; }
        public static int PlayerOnScreenEdge { get; }
        public static int PlayerOutOfScreen { get; }

        public static int EQWindowID { get; }
        public static int EQWindowHidden { get; }


        static Flags()
        {
            AmIInPZ = 16384;
            AmIHasted = 64;

            FollowTargetFalse = 0;
            FollowtTargetTrue = 1;

            PlayerOnScreen = 0;
            PlayerOnScreenEdge = 256;
            PlayerOutOfScreen = 127;

            EQWindowID = 0xB5;
            EQWindowHidden = 0x4E;
        }
    }
}
