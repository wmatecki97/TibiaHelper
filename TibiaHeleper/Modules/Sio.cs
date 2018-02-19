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
    class Sio : Module
    {
        public bool working { get; set; }
        public string playerName { get; set; }
        public int healthPercentToHeal { get; set; }


        public void Run()
        {
            working = true;
            UInt32 playerAdress = findPlayerAdress();
            string spell = "exura sio \"" + playerName + "\"";
            
            while (working)
            {
                if(GetData.isPlayerOnScreen(playerAdress))
                {
                    int playerHPPercent = GetData.getPlayerHPPercent(playerAdress);
                    int mana = GetData.getMana();
                    if (GetData.getPlayerHPPercent(playerAdress) < healthPercentToHeal && GetData.getMana() >= 140)
                    {
                        KeyboardSimulator.Message(spell);
                        Thread.Sleep(1000);
                    }
                }
                Thread.Sleep(50);
            }
            
        }

        /// <summary>
        /// checks every 500ms if has player been spotted
        /// </summary>
        /// <returns></returns>
        private UInt32 findPlayerAdress()
        {
            bool isPlayerfound = false;
            UInt32 playerAdress = GetData.getPersonLastOcurranceAdress(playerName, ref isPlayerfound);

            while (!isPlayerfound && working)
            {
                playerAdress = GetData.getPersonLastOcurranceAdress(playerName, ref isPlayerfound, playerAdress);//last parameter is last spotted players adress if Player was not spotted yet he would  be shown at the end
                Thread.Sleep(500);
            }

            return playerAdress;
        }
    }
}
