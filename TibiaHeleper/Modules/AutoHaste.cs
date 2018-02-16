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
        public int HasteMana { get; set; }

        private UInt32 ActualSpeedAdress = Adresses.ActualSpeed;
        private UInt32 NormalSpeedAdress = Adresses.NormalSpeed;
        private UInt32 Mana

        public void Run()
        {
            working = true;
            int mana;

            while (working)
            {
                mana=
                if (GetData.getDataFromAdress(ActualSpeedAdress) <= GetData.getDataFromAdress(NormalSpeedAdress) + 100)
                {
                    KeyboardSimulator.Message(HasteSpell);
                }
                Thread.Sleep(300);
            }
        }
    }
}
