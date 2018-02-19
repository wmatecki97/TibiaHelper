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
    public class AutoHaste : Module
    {
        public bool working { get; set; }
        public string HasteSpell { get; set; }
        public int ManaCost { get; set; }

        

        public void Run()
        {
            working = true;
            int mana;
            int actualSpeed;

            /* 
            int hastedSpeed;

          //  int speedBeforeHaste = normalSpeed = GetData.getActualSpeed();

            hastedSpeed = GetData.getActualSpeed();
            KeyboardSimulator.Message(HasteSpell);

            // while (hastedSpeed = GetData.getActualSpeed()) ;
            //Thread.Sleep(3000);
            hastedSpeed = GetData.getActualSpeed();

            */
            int normalSpeed = GetData.getNormalSpeed();

            while (working)
            {
                mana = GetData.getMana();
                actualSpeed = GetData.getActualSpeed();

                if (mana>=ManaCost && GetData.getActualSpeed()<=normalSpeed+50)
                {
                    KeyboardSimulator.Message(HasteSpell);
                }
                Thread.Sleep(500);
            }
        }
    }
}
