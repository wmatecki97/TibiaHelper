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

            int normalSpeed = GetData.MyNormalSpeed;

            while (working)
            {
                mana = GetData.MyMana;
                actualSpeed = GetData.MyActualSpeed;

                if (mana>=ManaCost && !GetData.AmIHasted)
                {
                    KeyboardSimulator.Simulate(HasteSpell);
                }
                Thread.Sleep(500);
            }
            stopped = true;
        }
    }
}
