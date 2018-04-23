using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TibiaHeleper.Storage;

namespace TibiaHeleper.Modules.WalkerModule
{
    [Serializable]
    class TradeItem
    {
        public Item item { get; set; }
        public Action action { get; set; }
        public int amount { get; set; }
        public string displayName
        {
            get
            {
                if (action == Action.Sell)
                    return "Sell " + item.name;
                else
                    return "Buy " + amount + " " + item.name;
            }
        }

        public TradeItem(Item item, Action action, int amount = -1)
        {
            this.item = item;
            this.action = action;
            this.amount = amount;
        }

        public enum Action { Sell = 0, Buy = 1 };

    }
}
