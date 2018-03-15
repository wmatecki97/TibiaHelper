namespace TibiaHeleper.MemoryOperations
{
    public static class Flags
    {
        public static int AmIInPZ {get;}
        public static int AmIHasted { get; }

        public static int PlayerOnScreen { get; }
        public static int PlayerOnScreenEdge { get; }
        public static int PlayerOutOfScreen { get; }



        static Flags()
        {
            AmIInPZ = 16384;
            AmIHasted = 64;

            PlayerOnScreen = 0;
            PlayerOnScreenEdge = 256;
            PlayerOutOfScreen = 127;
        }
    }
}
