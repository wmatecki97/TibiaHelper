using System;
using System.Threading;
using TibiaHeleper.MemoryOperations;
using TibiaHeleper.Simulators;

namespace TibiaHeleper.Modules
{
    [Serializable]
    public class AntyPalalyse : Module
    {
        public bool working { get; set; }
        public bool stopped { get; set; }

        public string AntyParalyseSpell { get; set; }
        public int ManaCost { get; set; }

        public AntyPalalyse()
        {
            stopped = true;
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

                if (mana >= ManaCost && GetData.MyActualSpeed <= normalSpeed-50)
                {                    
                    KeyboardSimulator.Simulate(AntyParalyseSpell);
                    Thread.Sleep(800);
                }
                Thread.Sleep(50);
                KeyboardSimulator.Simulate("f5");
            }
            stopped = true;
        }
    }
}
