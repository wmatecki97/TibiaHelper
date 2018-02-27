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
using TibiaHeleper.MemoryOperations;

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
            if (Modules.ModulesManager.healer.working) HealerEnable.IsChecked = true;

        }

        private void HealerDisable(object sender, RoutedEventArgs e)
        {
            Modules.ModulesManager.HealerDisable();
        }

        private void HealerRun(object sender, RoutedEventArgs e)
        {
            Modules.ModulesManager.HealerEnable();
        }

        private void HealerButtonClicked(object sender, RoutedEventArgs e)
        {
            HealerWindow healerWindow = new Windows.HealerWindow();
            healerWindow.Show();
            this.Hide();
        }

        private void Close(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void OpenAdditionalModulesWindow(object sender, RoutedEventArgs e)
        {
            AdditionalModules othersWindow = new AdditionalModules();
            othersWindow.Show();
            this.Hide();
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            Storage.Storage.Save();
        }

        private void Load(object sender, RoutedEventArgs e)
        {
            Storage.Storage.Load();
        }

        
    }
}
