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
        public string lowHPAction {get; set;}
        public string medHPAction {get; set;}
        public string highHPAction {get; set;}
        public string lowManaAction {get; set;}
        public string medManaAction {get; set;}
        public string highManaAction {get; set;}

        public bool stopped { get; set; }

        public Healer()
        {
            stopped = true;
        }

        public void Run()
        {

            while (working)
            {
                //int maxHP = HPXOR ^ GetData.getIntegerDataFromAdress(MaxHPAdr);
                int HP = GetData.getHP();
                int mana = GetData.getMana();
                if (HP > 0)
                {
                    healHP(HP, mana);
                    healMana(mana);
                   // KeyboardSimulator.Simulate("f6");
                }                
                Thread.Sleep(100);
            }
            stopped = true;

        }


        private void healMana(int mana)
        {
            if (mana < lowMana)
                KeyboardSimulator.Simulate(lowManaAction);
            else if (mana < highMana)
                KeyboardSimulator.Simulate(highManaAction);
        }

        private void healHP(int HP, int mana)
        {
            if (HP < lowHP && mana > lowHPMana)
            {
                KeyboardSimulator.Simulate(lowHPAction);
            }
            else if (HP < medHP && mana > medHPMana)
            {
                KeyboardSimulator.Simulate(medHPAction);
            }
            else if (HP < highHP && mana > medHPMana)
            {
                KeyboardSimulator.Simulate(highHPAction);
            }
        }
        

    }
}


