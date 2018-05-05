using System;
using System.Windows;
using TibiaHeleper.MemoryOperations;

namespace TibiaHeleper.Windows
{
    public static class WindowsManager
    {
        public static HealerWindow healerWindow { get; set; }
        public static AdditionalModules additionalModulesWindow { get; set; }
        public static Menu menu { get; set; }
        public static Targeting targeting { get; set; }
        public static WalkerWindow walkerWindow { get; set; }
        public static AlarmsWindow alarms { get; set; }

        static WindowsManager()
        {
            healerWindow = new HealerWindow();
            additionalModulesWindow = new AdditionalModules();
            menu = new Menu();
            targeting = new Targeting();
            walkerWindow = new WalkerWindow();
            alarms = new AlarmsWindow();
        }
        
        
       
    }
}
