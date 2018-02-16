using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TibiaHeleper.Keyboard;

namespace TibiaHeleper.Modules
{
    class AutoHaste
    {
        public static bool working { get; }
        private static UInt32 ActualSpeedAdress = Adresses.ActualSpeed;
        private static UInt32 NormalSpeedAdress = Adresses.NormalSpeed;
        public void start()
        {
            
            while (working)
            {
                if (GetData.getDataFromAdress(ActualSpeedAdress) <= GetData.getDataFromAdress(NormalSpeedAdress) + 100)
                {
                    
                }
            }
        }
    }
}
