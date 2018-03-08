using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibiaHeleper.Windows
{
    public static class WindowsManager
    {
        public static HealerWindow healerWindow { get; set; }
        public static AdditionalModules additionalModulesWindow { get; set; }
        public static Menu menu { get; set; }
        public static Targeting targeting { get; set; }

        static WindowsManager()
        {
            healerWindow = new HealerWindow();
            additionalModulesWindow = new AdditionalModules();
            menu = new Menu();
            targeting = new Targeting();
        }
        
    }
}
