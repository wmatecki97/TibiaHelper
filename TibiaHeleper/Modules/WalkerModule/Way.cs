using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibiaHeleper.Modules.WalkerModule
{
    [Serializable]
    class Way : WalkerStatement, ICloneable
    {
        public List<WalkerStatement> list { get; set; }

        public Way(List<WalkerStatement> listOfWaypoints)
        {
            type = StatementType.getType["Way"];
            list = listOfWaypoints;
            if(list!=null && list.Count>0)
            {
                Waypoint from = list[0] as Waypoint;
                Waypoint to = list[list.Count - 1] as Waypoint;
                name = "Way from: " + from.floor + ":" + from.xPos + ":" + from.yPos + " to: " + to.floor + ":" + to.xPos + ":" + to.yPos;
            }
        }

        /// <summary>
        /// converts all Way in list to list of waypoints
        /// </summary>
        /// <param name="StatementsList"></param>
        /// <returns></returns>
        public static List<WalkerStatement> changeWayToWaypointList(List<WalkerStatement> StatementsList)
        {
            List<WalkerStatement> list = new List<WalkerStatement>();

            for (int i = 0; i < StatementsList.Count; i++)
            {
                if (StatementsList[i].type == StatementType.getType["Way"])
                {
                    Way way = (Way)StatementsList[i];
                    foreach (Waypoint waypoint in way.list)
                    {
                        list.Add(waypoint);
                    }
                }
                else
                {
                    list.Add(StatementsList[i]);
                }
            }

            return list;
        }

        public static List<WalkerStatement> changeWaypointListToWay(List<WalkerStatement> StatementsList)
        {

            List<WalkerStatement> list = new List<WalkerStatement>();
            bool skip = false;
            List<WalkerStatement> way = new List<WalkerStatement>();
            for (int i = 0; i < StatementsList.Count; i++)
            {
                if (StatementsList[i].type != StatementType.getType["Waypoint"])
                {
                    if (way.Count == 1)
                    {
                        list.Add(way[0]);
                    }
                    else if (way.Count > 1)
                    {
                        list.Add(new Way(way));
                        way = new List<WalkerStatement>();
                    }

                    list.Add(StatementsList[i]);
                }
                else
                {
                    way.Add(StatementsList[i]);
                }
            }
            if (way.Count == 1)
            {
                list.Add(way[0]);
            }
            else if (way.Count > 1)
            {
                list.Add(new Way(way));
                way = new List<WalkerStatement>();
            }

            /*
            for (int i = 0; i < StatementsList.Count; i++)
            {
                if (!skip)
                {
                    list.Add(StatementsList[i]);
                }

                if (StatementsList[i].type == StatementType.getType["Way"])
                {
                    skip = !skip;
                }

            }
            */
            return list;
        }

        


    }
}
