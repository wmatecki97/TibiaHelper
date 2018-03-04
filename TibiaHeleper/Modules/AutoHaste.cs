using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TibiaHeleper.Keyboard;
using TibiaHeleper.MemoryOperations;

namespace TibiaHeleper.Modules
{
    [Serializable]
    public class AutoHaste : Module
    {
        public bool working { get; set; }
        private bool stopped;

        public string HasteSpell { get; set; }
        public int ManaCost { get; set; }


        public void Run()
        {
            working = true;
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

        public void Stop()
        {
            if (working)
            {
                stopped = false;
                working = false;
                while (!stopped) ;//waiting for thread finish
            }
        }
    }
}
