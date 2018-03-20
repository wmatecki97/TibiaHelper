using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TibiaHeleper.MemoryOperations;

namespace TibiaHeleper.Modules.WalkerModule
{
    public class Tracker: Module
    {
        public  List<Waypoint> list;
        public bool working { get; set; }
        public bool stopped { get; set; }

        public Tracker()
        {
            stopped = true;
            list = new List<Waypoint>();
        }

        public  void Run()
        {
            int xPos = GetData.MyXPosition;
            int yPos = GetData.MyYPosition;
            int floor = GetData.MyFloor;

            list.Add(new Waypoint(xPos, yPos, floor));

            while (working)
            {
                if(xPos!=GetData.MyXPosition || yPos!=GetData.MyYPosition || floor != GetData.MyFloor)
                {
                    xPos = GetData.MyXPosition;
                    yPos = GetData.MyYPosition;
                    floor = GetData.MyFloor;
                    list.Add(new Waypoint(xPos, yPos, floor));
                }

                Thread.Sleep(200);
            }
            stopped = true;
        }

       
    }
}
