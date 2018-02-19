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
    /// Interaction logic for AdditionalModules.xaml
    /// </summary>
    public partial class AdditionalModules : Window
    {

        public AdditionalModules()
        {
            InitializeComponent();
        }

        private void AssignData(object sender, RoutedEventArgs e) //on loading
        {
            if (ModulesManager.autoHaste.working)
                AHEnable.IsChecked = true;
            AHSpell.Text = ModulesManager.autoHaste.HasteSpell;
            AHMana.Text = ModulesManager.autoHaste.ManaCost.ToString();
            PlayerHpPercent.Text = ModulesManager.sio.healthPercentToHeal.ToString();
            PlayerName.Text = ModulesManager.sio.playerName;
        }

        private void EnableAutoHaste(object sender, RoutedEventArgs e)
        {
            if (!ModulesManager.autoHaste.working)
                try
                {
                    ModulesManager.autoHaste.HasteSpell = AHSpell.Text;
                    ModulesManager.autoHaste.ManaCost = int.Parse(AHMana.Text);
                    ModulesManager.AutoHasteEnable();
                }
                catch (Exception)
                {
                    Error.Visibility = Visibility.Visible;
                }

        }

        private void DisableAutoHate(object sender, RoutedEventArgs e)
        {
            ModulesManager.AutoHasteDisable();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            WindowsManager.menu.Show();
            this.Close();
        }

        private void Close(object sender, EventArgs e)
        {
            WindowsManager.menu.Show();
        }

        private void HideErrorGrid(object sender, RoutedEventArgs e)
        {
            Error.Visibility = Visibility.Hidden;
        }

        private void DisableSio(object sender, RoutedEventArgs e)
        {
            ModulesManager.SioDisable();
        }

        private void EnableSio(object sender, RoutedEventArgs e)
        {
            try
            {
                ModulesManager.sio.playerName = PlayerName.Text;
                ModulesManager.sio.healthPercentToHeal = int.Parse(PlayerHpPercent.Text);
            }
            catch (Exception)
            {
                Error.Visibility = Visibility.Visible;
            }
            ModulesManager.SioEnable();
        }
    }
}
