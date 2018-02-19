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


        public void Run()
        {
           
            /*
            //TEMP TEMP TEMP TEMP TEMP TEMP TEMP TEMP TEMP TEMP TEMP
            lowHP = 500;
            lowHPButton = "shift + f3";
            lowHPMana = 120;
            medHP = 700;
            medHPButton = "ShiFT + f2";
            medHPMana = 70;
            highHP = 900;
            highHPButton = "ShiFT + f1";
            highHPMana = 20;
            //TEMP TEMP TEMP TEMP TEMP TEMP TEMP TEMP TEMP TEMP TEMP
            */

            working = true;

            while (working)
            {

                //int maxHP = HPXOR ^ GetData.getIntegerDataFromAdress(MaxHPAdr);
                int HP = GetData.getHP();
                int mana = GetData.getMana();
                healHP(HP, mana);
                healMana(mana);
                Thread.Sleep(100);
            }

        }
        private void healMana(int mana)
        {
            if (mana < lowMana)
                KeyboardSimulator.Press(lowManaButton);
            else if (mana < highMana)
                KeyboardSimulator.Press(highManaButton);
        }

        private void healHP(int HP, int mana)
        {
            if (HP < lowHP && mana > lowHPMana)
            {
                KeyboardSimulator.Press(lowHPButton);
            }
            else if (HP < medHP && mana > medHPMana)
            {
                KeyboardSimulator.Press(medHPButton);
            }
            else if (HP < highHP && mana > medHPMana)
            {
                KeyboardSimulator.Press(highHPButton);
            }
        }

    }
}


