using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibiaHeleper.Storage
{
    [Serializable]
    class VersionedObject
    {
        public string version { get; set; }
        public object obj;

        public VersionedObject(object obj)
        {
            this.obj = obj;
            version = Storage.THVersion;
        }
    }
}
