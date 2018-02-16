using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TibiaHeleper.Keyboard;

namespace TibiaHeleper.Modules
{

    public static class Healer
    {
        static UInt32 Base;
        static bool working;
        static int lowHP, medHP, highHP, lowHPMana, medHPMana, highHPMana;
        static int lowMana, medMana, highMana;
        static string lowHPButton, medHPButton, highHPButton, lowManaButton, medManaButton, highManaButton;
        static UInt32 XORAdr;
        static UInt32 MaxHPAdr;
        static UInt32 HPAdr;
        static UInt32 MaxManaAdr;
        static UInt32 ManaAdr;
         
        static Healer()
        {
            //getting adresses of variables in Tibia.exe
            MaxHPAdr = Adresses.MaxHPAdr;
            MaxManaAdr = Adresses.MaxManaAdr;
            HPAdr = Adresses.HPAdr;
            ManaAdr = Adresses.ManaAdr;
            XORAdr = Adresses.HPXORAdr;
            
        }

        public static void Run()
        {
            int XOR = GetData.getDataFromAdress(XORAdr);



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


            working = true;

            while (working)
            {
                //int maxHP = HPXOR ^ GetData.getDataFromAdress(MaxHPAdr);
                int HP = XOR ^ GetData.getDataFromAdress(HPAdr);
                int mana = XOR ^ GetData.getDataFromAdress(ManaAdr);
                healHP(HP, mana);
                healMana(mana);
                Thread.Sleep(100);
            }

        }
        private static void healMana(int mana)
        {
            if (mana < lowMana)
                KeyboardSimulator.Press(lowManaButton);
            else if (mana < highMana)
                KeyboardSimulator.Press(highManaButton);
        }

        private static void healHP(int HP, int mana)
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

        public static void AsignValues(int lHP,int mHP,int hHP,string lHPB,string mHPB,string hHPB, int lHPMana, int mHPMana, int hHPMana, int lMana, int hMana, string lManaB, string hManaB)
        {
            lowHP = lHP;
            medHP = mHP;
            highHP = hHP;
            lowHPButton = lHPB;
            medHPButton = mHPB;
            highHPButton = hHPB;
            lowHPMana = lHPMana;
            medHPMana = lHPMana;
            highHPMana = hHPMana;
            lowMana = lMana;
            lowManaButton = lManaB;
            highMana = hMana;
            highManaButton = hManaB;
        }


        public static string getLowHP() { return lowHP.ToString(); }
        public static string getMedHP() { return medHP.ToString(); }

        internal static string getlowHPMana()
        {
            return lowHPMana.ToString();
        }

        internal static string gethighHPMana()
        {
            return medHPMana.ToString();
        }

        internal static string getmedHPMana()
        {
            return highHPMana.ToString();
        }

        public static string getLowMana()
        {
            return lowMana.ToString();
        }

        public static string getLowManaButton()
        {
            return lowManaButton;
        }

        public static string getHighManaButton()
        {
            return highManaButton;
        }

        public static string getHighMana()
        {
            return highMana.ToString();
        }

        public static string getHighHP() { return highHP.ToString(); }
        public static string getLowHPB() { return lowHPButton; }
        public static string getMedHPB() { return medHPButton; }
        public static string getHighHPB() { return highHPButton; }
        public static bool isWorking() { return working; }

        public static void Stop()
        {
            working = false;
        }
    }
}


