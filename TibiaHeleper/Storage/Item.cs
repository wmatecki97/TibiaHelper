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
            if (!Equals(lhs, null))
            {
                if (!Equals(rhs, null))
                    return lhs.ID == rhs.ID;
                else
                    return false;
            }
            else
                return Equals(rhs, null);

        }
        public static bool operator !=(Item lhs, Item rhs)
        {
            if (!Equals(lhs, null))
            {
                if (!Equals(rhs, null))
                    return lhs.ID != rhs.ID;
                else
                    return true;
            }
            else
                return !Equals(rhs, null);
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
