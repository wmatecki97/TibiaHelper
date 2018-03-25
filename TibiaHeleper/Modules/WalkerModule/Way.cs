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
            type = (int)StatementType.getType["Way"];
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
                list.Add(StatementsList[i]);

                if (StatementsList[i].type == (int)StatementType.getType["Way"])
                {
                    Way way = (Way)StatementsList[i];
                    foreach (Waypoint waypoint in way.list)
                    {
                        list.Add(waypoint);
                    }
                    list.Add(StatementsList[i]);
                }
            }

            return list;
        }

        public static List<WalkerStatement> changeWaypointListToWay(List<WalkerStatement> StatementsList)
        {
            List<WalkerStatement> list = new List<WalkerStatement>();
            bool skip = false;
            
            for (int i = 0; i < StatementsList.Count; i++)
            {
                if (!skip)
                {
                    list.Add(StatementsList[i]);
                }

                if (StatementsList[i].type == (int)StatementType.getType["Way"])
                {
                    skip = !skip;
                }

            }

            return list;
        }

        public override object Clone()
        {
            return MemberwiseClone();
        }


    }
}
