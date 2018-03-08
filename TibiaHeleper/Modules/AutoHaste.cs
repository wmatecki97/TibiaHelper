using System;
using System.Threading;
using TibiaHeleper.MemoryOperations;
using TibiaHeleper.Simulators;

namespace TibiaHeleper.Modules
{
    [Serializable]
    public class AutoHaste : Module
    {
        public bool working { get; set; }
        public bool stopped { get; set; }

        public string HasteSpell { get; set; }
        public int ManaCost { get; set; }

        public AutoHaste()
        {
            stopped = true;
            working = false;
        }

        public void Run()
        {
            int mana;
            int actualSpeed;

            int normalSpeed = GetData.getNormalSpeed();

            while (working)
            {
                mana = GetData.getMana();
                actualSpeed = GetData.getActualSpeed();

                if (mana>=ManaCost && GetData.getActualSpeed()<=normalSpeed+90)
                {
                    KeyboardSimulator.Simulate(HasteSpell);
                }
                Thread.Sleep(500);
            }
            stopped = true;
        }
    }
}
