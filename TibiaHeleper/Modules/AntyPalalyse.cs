using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TibiaHeleper.Keyboard;
using TibiaHeleper.MemoryOperations;

namespace TibiaHeleper.Modules
{
    [Serializable]
    public class AntyPalalyse : Module
    {
        public bool working { get; set; }
        private bool stopped;

        public string AntyParalyseSpell { get; set; }
        public int ManaCost { get; set; }

        public void Run()
        {
            stopped = false;
            working = true;

            int mana;
            int actualSpeed;
            int normalSpeed = GetData.getNormalSpeed();

            while (working)
            {
                mana = GetData.getMana();
                actualSpeed = GetData.getActualSpeed();

                if (mana >= ManaCost && GetData.getActualSpeed() <= normalSpeed-50)
                {                    
                    KeyboardSimulator.Simulate(AntyParalyseSpell);
                    Thread.Sleep(800);
                }
                Thread.Sleep(50);
            }
            stopped = true;
        }

        public void Stop()
        {
            working = false;
            while (!stopped) ;
        }
    }
}
