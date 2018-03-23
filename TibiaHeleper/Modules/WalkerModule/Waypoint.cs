using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibiaHeleper.Modules.WalkerModule
{
    [Serializable]
    public class Waypoint : WalkerStatement, ICloneable
    {
        public int xPos;
        public int yPos;
        public int floor;

        public Waypoint(int xPosition, int yPosition, int Floor, bool Stand =false)
        {
            xPos = xPosition;
            yPos = yPosition;
            floor = Floor;
            name = "W: " + floor.ToString() + ":" + xPosition.ToString() + ":" + yPosition.ToString();
            if (!Stand)
                type = (int)StatementType.getType["Waypoint"];
            else
            {
                type = (int)StatementType.getType["Stand"];
                name = "S: " + floor.ToString() + ":" + xPosition.ToString() + ":" + yPosition.ToString();

            }

        }

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
