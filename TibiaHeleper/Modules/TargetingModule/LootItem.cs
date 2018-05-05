using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TibiaHeleper.Storage;

namespace TibiaHeleper.Modules.TargetingModule
{
    [Serializable]
    public class LootItem : Item
    {
        public Item container;
        public string displayedName { get { return name + " to " + container.name; } }
        public LootItem(string name, int id) : base(name, id)
        {
            container = new Item("Default container", -1);
        }
        public LootItem(string name, int id, Item container) : base(name, id)
        {
            if (container != null)
            {
                this.container = container;
            }
            else
                container = new Item("Default container", -1);
        }
       
    }
}
