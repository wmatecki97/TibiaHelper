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
using TibiaHeleper.Modules;

namespace TibiaHeleper.Windows
{
    /// <summary>
    /// Interaction logic for AlarmsWindow.xaml
    /// </summary>
    public partial class AlarmsWindow : Window
    {
        public AlarmsWindow()
        {
            InitializeComponent();
        }

        private void Load(object sender, RoutedEventArgs e)
        {
            lowHPCheckBox.IsChecked = ModulesManager.alarms.lowHP;
            notMovingCheckBox.IsChecked = ModulesManager.alarms.notMoving;
            loggedOutCheckBox.IsChecked = ModulesManager.alarms.loggedOut;            
        }

        private void Close(object sender, System.ComponentModel.CancelEventArgs e)
        {
            WindowsManager.menu.Show();
            WindowsManager.alarms = new AlarmsWindow();
        }

        private void showPopUpWindow(string errorMessage = "Unacceptable value")
        {
            ErrorLabel.Content = errorMessage;
            Error.Visibility = Visibility.Visible;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            try
            {
                ModulesManager.alarms.lowHP = lowHPCheckBox.IsChecked.Value;
                if (lowHPCheckBox.IsChecked.Value)
                    ModulesManager.alarms.hp = int.Parse(hpAmountTextBox.Text);
                ModulesManager.alarms.loggedOut = loggedOutCheckBox.IsChecked.Value;
                ModulesManager.alarms.notMoving = notMovingCheckBox.IsChecked.Value;
                showPopUpWindow("Saved Successfully");
            }
            catch (Exception)
            {
                showPopUpWindow();
            }

        }

        private void Back(object sender, RoutedEventArgs e)
        {
            WindowsManager.menu.Show();
            this.Hide();
        }

        private void HideErrorGrid(object sender, RoutedEventArgs e)
        {
            Error.Visibility = Visibility.Hidden;
        }
    }
}
