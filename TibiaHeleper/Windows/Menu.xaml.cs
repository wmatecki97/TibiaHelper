using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TibiaHeleper.Keyboard;
using TibiaHeleper.MemoryOperations;
using TibiaHeleper.Modules;

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
            if (!GetData.inject())
            {
                Environment.Exit(0);
            }
            checkWorkingModules();
            GetData.ActualizeAllSpottedCreaturesList();
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
    }
}
