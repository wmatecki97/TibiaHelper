using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TibiaHeleper.Storage;

namespace TibiaHeleper.Modules.WalkerModule
{
    [Serializable]
    public class DepositItem : Item
    {
        public int chestNumber { get; set; }
        public int amount { get; set; }
        public string displayedName
        {
            get
            {
                string special = "";
                if (whereToPut != null)
                    special += " to " + whereToPut.name;
                string result = amount==-1 ? "all " + name : amount + " " + name;
                return result+special;
            }
        }
        public Item whereToPut { get; set; }

        public DepositItem(string name, int id, int chestNumber, int amount, Item containerToPut=null) : base(name,id)
        {
            if (containerToPut == null)
                amount = -1;

            this.chestNumber = chestNumber;
            this.amount = amount;
            whereToPut = containerToPut;
        }
    
    }
}
