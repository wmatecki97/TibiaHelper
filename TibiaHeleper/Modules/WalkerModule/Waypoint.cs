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

        public Waypoint(int xPosition, int yPosition, int Floor)
        {
            xPos = xPosition;
            yPos = yPosition;
            floor = Floor;
            type = (int)StatementType.getType["waypoint"];
            name = "W: " + floor.ToString() + ":" + xPosition.ToString() + ":" + yPosition.ToString();
        }

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
