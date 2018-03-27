using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TibiaHeleper.MemoryOperations;
using TibiaHeleper.Simulators;
using TibiaHeleper.Windows;

namespace TibiaHeleper.Modules.WalkerModule
{
    [Serializable]
    class Walker : Module
    {
        public List<WalkerStatement> list;

        private Stack<Waypoint> wayBack;

        public bool stopped { get; set; }
        public bool working { get; set; }
        public int actualStatementIndex;
        public int startStatementIndex;
        public int attemptsToRandomDirection;
        private Random rand;

        public int tolerance;

        private int direction;
        private int lastDirection;
        //8 - north
        //2 - south
        //4 - west
        //6 - east
        //1 - south-west
        //3 - south-east
        //7 - north-west
        //9 - north-east



        public Walker()
        {
            stopped = true;
            list = new List<WalkerStatement>();
            wayBack = new Stack<Waypoint>();
            tolerance = 0;
            attemptsToRandomDirection = 5;
            rand = new Random();
            actualStatementIndex = 0;
        }

        public void Run()
        {
            WalkerStatement statement;
            int listCount;
            lock (list) { listCount = list.Count(); }
            int temp;

            while (working && actualStatementIndex < listCount)
            {
                lock (list)
                {
                    listCount = list.Count();
                    statement = list[actualStatementIndex];
                }
                //getting and setting on window list

                if (statement.type == StatementType.getType["Waypoint"] || statement.type == StatementType.getType["Stand"])//walk
                {
                    temp = tolerance;
                    if (statement.type == StatementType.getType["Stand"])
                        tolerance = 0;
                    if (!goToCoordinates((Waypoint)statement))// goes to the coordinates
                    {
                        tryTogetBack(); //if player is in not good position then goes back to the last reached waypoint
                        actualStatementIndex--;
                    }
                    tolerance = temp;
                }
                else if (statement.type == StatementType.getType["action"])//do action
                {
                    try
                    {
                        Action action = (Action)statement;
                        action.DoAction();
                    }
                    catch (Exception) { }
                }
                Thread.Sleep(50);

                actualStatementIndex++;
            }
            actualStatementIndex = 0;
            working = false;
            stopped = true;
            WindowsManager.menu.Update();
        }

        private bool tryTogetBack()
        {
            Waypoint waypoint;
            bool isStackEmpty;
            lock (wayBack)
            {
                isStackEmpty = wayBack.Count() == 0;
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
                    if (!getNewDirection())
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
            int attempt = 0;
            while (!hasWaypointBeenReached(waypoint) && GetData.isOnScreen(waypoint.xPos, waypoint.yPos, waypoint.floor) && working)
            {
                attempt++;
                while (ModulesManager.targeting.attacking)
                    Thread.Sleep(500); // waits for targetting

                direction = 0;
                if (waypoint.xPos > GetData.MyXPosition)
                    direction += 1;
                else if (waypoint.xPos < GetData.MyXPosition)
                    direction -= 1;
                if (waypoint.yPos > GetData.MyYPosition)
                    direction += 2;
                else if (waypoint.yPos < GetData.MyYPosition)
                    direction += 8;
                else direction += 5;
                if (attempt == attemptsToRandomDirection)
                {
                    direction = rand.Next(1, 9);
                    attempt = 0;
                }

                go(waypoint);
                Thread.Sleep(200);
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
            return Math.Abs(GetData.MyXPosition - waypoint.xPos) + Math.Abs(GetData.MyYPosition - waypoint.yPos) <= tolerance;
        }

        /// <summary>
        /// simple attempt to go to the waypoint. It is not certain to get there!
        /// </summary>
        /// <param name="waypoint"></param>
        private void go(Waypoint waypoint)
        {
            if (direction == 8)
                KeyboardSimulator.Press("up");
            else if (direction == 6)
                KeyboardSimulator.Press("right");
            else if (direction == 4)
                KeyboardSimulator.Press("left");
            else if (direction == 2)
                KeyboardSimulator.Press("down");
            else
                MouseSimulator.clickOnField(waypoint.xPos, waypoint.yPos);

            // KeyboardSimulator.Press("NUM" + direction);

            //MouseSimulator.clickOnField(waypoint.xPos, waypoint.yPos);
        }

        private bool getNewDirection()
        {
            int distance = 99;
            Waypoint waypoint;
            int nextStatementID = actualStatementIndex;

            for (int i = actualStatementIndex + 1; i != actualStatementIndex; i++) //checking waypoints and gets the closest one
            {
                lock (list)
                {
                    if (i >= list.Count())
                    {
                        i = 0;
                        if (i == actualStatementIndex)
                            break;
                    }

                    if (list[i].type == StatementType.getType["Waypoint"]) // if statement is waypoint
                    {
                        waypoint = (Waypoint)list[i];
                        if (GetData.isOnScreen(waypoint.xPos, waypoint.yPos, waypoint.floor))
                        {
                            if (distance > GetData.GetDistance(waypoint.xPos, waypoint.yPos))//setting the new result
                            {
                                distance = GetData.GetDistance(waypoint.xPos, waypoint.yPos);
                                nextStatementID = i - 1;
                            }
                        }
                    }
                    else if (list[i].type == StatementType.getType["check"])// if statement is action
                    {
                        throw new NotImplementedException();
                        //when all conditions are good then go to label
                    }
                }
            }
            actualStatementIndex = nextStatementID;
            return distance < 99;
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
            result = Way.changeWaypointListToWay(result);
            return result;
        }

        public void SetList(List<WalkerStatement> newList)
        {
            lock (list)
            {
                list = newList;
                actualStatementIndex = 0;
            }
        }

    }
}
