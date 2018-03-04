using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TibiaHeleper.Keyboard;
using TibiaHeleper.MemoryOperations;
using TibiaHeleper.Storage;

namespace TibiaHeleper.Modules
{
    [Serializable]
    class Sio : Module
    {
        public bool working { get; set; }
        public string playerName { get;  set; }
        public int healthPercentToHeal { get; set; }

        private Creature player { get; set; }

        private bool stopped;
        public void Run()
        {
            working = true;
            findPlayer();
            string spell = "exura sio \"" + player.name + "\"";
            
            while (working)
            {
                if(player.onScreen)
                {
                    int mana = GetData.getMana();
                    if (player.HPPercent < healthPercentToHeal && GetData.getMana() >= 140)
                    {
                        //KeyboardSimulator.Message(spell);
                        KeyboardSimulator.Simulate("f10");
                        Thread.Sleep(1000);
                    }
                }
                Thread.Sleep(50);
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
