using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TibiaHeleper.MemoryOperations;

namespace TibiaHeleper.Storage
{

    class Window
    {
        private List<uint> address;
        public int height { get { return GetData.getIntegerDataFromDynamicAddress((uint)GetData.getDynamicAddress(address)); } }


        public Window(List<uint> address)
        {
            this.address = address;
        }
    }
}
