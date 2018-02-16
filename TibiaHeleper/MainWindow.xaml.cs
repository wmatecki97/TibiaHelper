using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TibiaHeleper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
               public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!GetData.inject())
            {
               // Environment.Exit(0);
            }
            if (Modules.Healer.isWorking()) HealerEnable.IsChecked = true;
           
        }

        private void HealerDisable(object sender, RoutedEventArgs e)
        {
            Modules.ModuesManager.HealerDisable();
        }

        private void HealerRun(object sender, RoutedEventArgs e)
        {
            Modules.ModuesManager.HealerRun();
        }

        private void HealerButtonClicked(object sender, RoutedEventArgs e)
        {
            Windows.HealerWindow healerWindow = new Windows.HealerWindow();
            healerWindow.Show();
            this.Hide();
        }

        private void Close(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
