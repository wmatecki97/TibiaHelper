using System;
using System.Windows;
using TibiaHeleper.BackgroundWorkers;
using TibiaHeleper.MemoryOperations;
using TibiaHeleper.Modules;
using TibiaHeleper.Simulators;

namespace TibiaHeleper.Windows
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();
        }
       
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowsManager.menu = this;
            if (!GetData.inject())
            {
                Environment.Exit(0);
            }
            checkWorkingModules();
            WorkersManager.EnvironmentGettingSterted();
            Show(); // check's if is character logged in
        }

        private void checkWorkingModules()
        {
            if (ModulesManager.healer.working) HealerEnable.IsChecked = true;
            if (ModulesManager.targeting.working) TargetingEnable.IsChecked = true;
        }

        private void HealerDisable(object sender, RoutedEventArgs e)
        {
            ModulesManager.HealerDisable();
        }

        private void HealerRun(object sender, RoutedEventArgs e)
        {
            ModulesManager.HealerEnable();
        }

        private void HealerButtonClicked(object sender, RoutedEventArgs e)
        {
            WindowsManager.healerWindow.Show();
            WindowsManager.healerWindow.assignData();
            this.Hide();
        }

        private void Close(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        public new void Show()
        {
            base.Show();
            GetData.WhoAmI();
            if (GetData.Me != null)
                Title = "Tibia Helper -" + GetData.Me.name;
        }

        private void OpenAdditionalModulesWindow(object sender, RoutedEventArgs e)
        {
            WindowsManager.additionalModulesWindow.Show();
            WindowsManager.additionalModulesWindow.checkWorkingModules();
            this.Hide();
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            Storage.Storage.Save();
        }

        private void Load(object sender, RoutedEventArgs e)
        {
            Storage.Storage.Load();
            checkWorkingModules();
            WindowsManager.additionalModulesWindow.checkWorkingModules();
            WindowsManager.healerWindow.assignData();
        }

        private void TargetingButtonClicked(object sender, RoutedEventArgs e)
        {
            WindowsManager.targeting.Show();
            this.Hide();
        }        

        private void TargetingDisable(object sender, RoutedEventArgs e)
        {
            ModulesManager.TargetingDisable();
        }

        private void TargetingRun(object sender, RoutedEventArgs e)
        {
            ModulesManager.TargetingEnable();
        }

        private void Test(object sender, RoutedEventArgs e)
        {
            int a = GetData.getGameWindowDistanceFromLeft();
        }

      
    }
}
