using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TibiaHeleper.Modules.TargetingModule;

namespace TibiaHeleper.Storage
{
    [Serializable]
    public class Item
    {
        public int ID { get; set; }
        public string name { get; set; }

        public Item(string name)
        {
            this.name = name;
        }
        public Item(string name, int id)
        {
            ID = id;
            this.name = name;
        }

        public static bool operator == (Item lhs, Item rhs)
        {
            return lhs.ID == rhs.ID;
        }
        public static bool operator !=(Item lhs, Item Rhs)
        {
            return lhs.ID != Rhs.ID;
        }

        public static List<int> ToIdList(List<LootItem> items)
        {
            List<int> IDs = new List<int>();

            foreach (LootItem item in items)
            {
                IDs.Add(item.ID);
            }

            return IDs;
        }

        public static List<int> ToIdList(List<Item> items)
        {
            List<int> IDs = new List<int>();
            
            foreach (Item item  in items)
            {
                IDs.Add(item.ID);
            }
            return IDs;
        }
    }
}
