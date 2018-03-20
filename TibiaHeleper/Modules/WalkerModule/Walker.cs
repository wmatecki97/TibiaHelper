using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TibiaHeleper.MemoryOperations;
using TibiaHeleper.Simulators;

namespace TibiaHeleper.Modules.WalkerModule
{
    class Walker : Module
    {
        public List<WalkerStatement> list;

        public Stack<Waypoint> wayBack;

        public bool stopped { get; set; }
        public bool working { get; set; }
        private int actualStatementId;

        public int tolerance;

        public Walker()
        {
            stopped = true;
            list = new List<WalkerStatement>();
            wayBack = new Stack<Waypoint>();
            tolerance = 0;
        }

        public void Run()
        {

            WalkerStatement statement;
            int listCount;
            lock (list) { listCount = list.Count(); }

            while (working)
            {
                for (actualStatementId = 0; actualStatementId <listCount && working; actualStatementId++)
                {
                    lock (list)
                    {
                        listCount = list.Count();

                        statement = list[actualStatementId];
                        //getting and setting on window list

                        if (statement.type == (int)WalkerStatement.getType["waypoint"])//walk
                        {
                            if (!goToCoordinates((Waypoint)statement))// goes to the coordinates
                            {
                                tryTogetBack(); //if player is in not good position then goes back to the last reached waypoint
                                actualStatementId--;
                            }
                        }
                        else//do action
                        {

                        }
                    }
                }
            }
            stopped = true;
        }

        private bool tryTogetBack()
        {
            Waypoint waypoint;
            bool isStackEmpty;
            lock (wayBack)
            {
               isStackEmpty  = wayBack.Count() == 0;
            }

            if (isStackEmpty) return getNewDirection();

            while (!isStackEmpty && working)
            {
                lock (wayBack)
                {
                    waypoint = wayBack.Pop();
                }
                if (!goToCoordinates(waypoint)) //if it is impossible to get back
                {
                    if(!getNewDirection())
                        return false;
                    break;
                }
                isStackEmpty = wayBack.Count() == 0;
            }
            return true;
        }

        /// <summary>
        /// trying to go to the coordinates, returns true if waypoint has been reached.
        /// </summary>
        /// <param name="waypoint"></param>
        /// <returns></returns>
        private bool goToCoordinates(Waypoint waypoint)
        {
            while(!hasWaypointBeenReached(waypoint) && GetData.isOnScreen(waypoint.xPos, waypoint.yPos) && working)
            {
                while (ModulesManager.targeting.attacking) Thread.Sleep(500); // waits for targetting
                    go(waypoint);
            }
            return hasWaypointBeenReached(waypoint);
        }

        /// <summary>
        /// return true if character is standing far enough to waypoint
        /// </summary>
        /// <param name="waypoint"></param>
        /// <returns></returns>
        private bool hasWaypointBeenReached(Waypoint waypoint)
        {
            return Math.Abs(GetData.MyXPosition - waypoint.xPos) <= tolerance && Math.Abs(GetData.MyYPosition - waypoint.yPos) <= tolerance;
        }

        /// <summary>
        /// simple attempt to go to the waypoint. It is not certain to get there!
        /// </summary>
        /// <param name="waypoint"></param>
        private void go(Waypoint waypoint)
        {
            MouseSimulator.clickOnField(waypoint.xPos, waypoint.yPos);
        }

        private bool getNewDirection()
        {
            int distance=99;
            Waypoint waypoint;
            int nextStatementID = actualStatementId;

            for (int i=actualStatementId+1; i!=actualStatementId; i++) //checking last 20 and next 20 waypoints and gets the closest one
            {
                if (i >= list.Count())
                {
                    i = 0;
                    if (i == actualStatementId)
                        break;
                }
                    

                if (list[i].type==(int)WalkerStatement.getType["waypoint"]) // if statement is waypoint
                {
                    waypoint = (Waypoint)list[i];
                    if (GetData.isOnScreen(waypoint.xPos, waypoint.yPos))
                    {
                        if(distance>GetData.GetDistance(waypoint.xPos, waypoint.yPos))//setting the new result
                        {
                            distance = GetData.GetDistance(waypoint.xPos, waypoint.yPos);
                            nextStatementID = i-1;
                        }
                    }
                }
                else if(list[i].type == (int)WalkerStatement.getType["check"])// if statement is action
                {
                    throw new NotImplementedException();
                    //when all conditions are good then go to label
                }
            }
            actualStatementId = nextStatementID;
            return distance <99;
        }
     
        
        public List<WalkerStatement> CopyList()
        {
            List<WalkerStatement> result = new List<WalkerStatement>();
            lock (list)
            {
                foreach (WalkerStatement statement in list)
                {
                    result.Add((WalkerStatement)(statement.Clone()));
                }
            }
            
            return result;
        }

        public void SetList(List<WalkerStatement> newList)
        {
            lock (list)
            {
                list = newList;
            }
        }
        
    }
}
