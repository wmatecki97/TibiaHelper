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
    public class Healer : Module
    {
        public bool working { get; set; }
        public int lowHP { get; set; }
        public int medHP { get; set; }
        public int highHP {get; set;}
        public int lowHPMana {get; set;}
        public int medHPMana {get; set;}
        public int highHPMana {get; set;}
        public int lowMana { get; set; }
        public int medMana {get; set;}
        public int highMana {get; set;}
        public string lowHPButton {get; set;}
        public string medHPButton {get; set;}
        public string highHPButton {get; set;}
        public string lowManaButton {get; set;}
        public string medManaButton {get; set;}
        public string highManaButton {get; set;}

        private bool stopped;

        public void Run()
        {
            working = true;

            while (working)
            {
                //int maxHP = HPXOR ^ GetData.getIntegerDataFromAdress(MaxHPAdr);
                int HP = GetData.getHP();
                int mana = GetData.getMana();
                if (HP > 0)
                {
                    healHP(HP, mana);
                    healMana(mana);
                }                
                Thread.Sleep(100);
            }
            stopped = true;

        }


        private void healMana(int mana)
        {
            if (mana < lowMana)
                KeyboardSimulator.Simulate(lowManaButton);
            else if (mana < highMana)
                KeyboardSimulator.Simulate(highManaButton);
        }

        private void healHP(int HP, int mana)
        {
            if (HP < lowHP && mana > lowHPMana)
            {
                KeyboardSimulator.Simulate(lowHPButton);
            }
            else if (HP < medHP && mana > medHPMana)
            {
                KeyboardSimulator.Simulate(medHPButton);
            }
            else if (HP < highHP && mana > medHPMana)
            {
                KeyboardSimulator.Simulate(highHPButton);
            }
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


