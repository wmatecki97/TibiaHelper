using System;

namespace TibiaHeleper.Modules.Targeting
{
    [Serializable]
    public class Target
    {
        public string name { get; set; }
        public int priority { get; set; }
        public string action { get; set; }
        public int minHP { get; set; }
        public int maxHP { get; set; }
        public int maxDistance { get; set; }

        public bool diagonal { get; set; }
        public bool HPMoreImportantThanDistance { get; set; }
    }
}
