using System;
using System.Threading;
using TibiaHeleper.MemoryOperations;
using TibiaHeleper.Simulators;
using TibiaHeleper.Storage;

namespace TibiaHeleper.Modules
{
    [Serializable]
    public class Sio : Module
    {
        public bool working { get; set; }
        public string playerName { get;  set; }
        public int healthPercentToHeal { get; set; }

        private Creature player { get; set; }

        public Sio()
        {
            stopped = true;
        }

        public bool stopped { get; set; }
        public void Run()
        {
            findPlayer();
            if (player == null) return;
            string spell = "exura sio \"" + player.name + "\"";
            
            while (working)
            {
                if(player.onScreen)
                {
                    int mana = GetData.MyMana;
                    if (player.HPPercent < healthPercentToHeal && GetData.MyMana >= 140)
                    {
                        KeyboardSimulator.Message(spell);
                      //  KeyboardSimulator.Simulate("f10");
                        Thread.Sleep(1000);
                    }
                }
                Thread.Sleep(50);
            }
            stopped = true;
        }

        /// <summary>
        /// checks every 500ms if has player been spotted
        /// </summary>
        /// <returns></returns>
        private void findPlayer()
        {
            while (player == null && working)
            {
                player = GetData.getPlayer(playerName);
                Thread.Sleep(500);
            }
        }
    }
}
