using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TibiaHeleper.MemoryOperations;

namespace TibiaHeleper.Modules
{
    [Serializable]
    public class Alarms : Module
    {
        public bool stopped { get; set; }
        public bool working { get; set; }

        public bool lowHP { get; set; }
        public int hp { get; set; }
        public bool notMoving { get; set; }
        public long notMovingTime { get; set; }
        public bool loggedOut { get; set; }
        public bool playerOnScreen { get; set; }
        public bool playerAttack { get; set; }


        public Alarms()
        {
            stopped = true;
        }

        public void Run()
        {

            bool alarm = false;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int x=0, y=0;

            while (working)
            {
                if (lowHP)
                    if (GetData.MyHP < hp)
                        alarm = true;

                if (notMoving)
                    if (GetData.MyXPosition == x && GetData.MyYPosition == y)
                    {
                        if (sw.ElapsedMilliseconds > notMovingTime)
                            alarm = true;
                    }
                    else sw.Restart();

                if (loggedOut)
                    if (!GetData.isAnybodyLoggedIn)
                        alarm = true;

                if (playerOnScreen)
                    throw new NotImplementedException();

                if(playerAttack)
                    throw new NotImplementedException();


                if (alarm)
                {
                    System.Media.SystemSounds.Asterisk.Play();
                    Thread.Sleep(500);
                }
                Thread.Sleep(500);

                alarm = false;

            }

            stopped = true;
        }
    }
}
