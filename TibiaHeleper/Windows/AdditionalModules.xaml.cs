using System;
using System.Windows;
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
            checkWorkingModules();
        }

        public void checkWorkingModules()
        {
            if (ModulesManager.autoHaste.working)
            {
                AHEnable.IsChecked = true;
                AHSpell.Text = ModulesManager.autoHaste.HasteSpell;
                AHMana.Text = ModulesManager.autoHaste.ManaCost.ToString();
            }
            if (ModulesManager.sio.working)
            {
                SioEnable.IsChecked = true;
                PlayerHpPercent.Text = ModulesManager.sio.healthPercentToHeal.ToString();
                PlayerName.Text = ModulesManager.sio.playerName;
            }
            if (ModulesManager.antyParalyse.working)
            {
                APSpell.Text = ModulesManager.antyParalyse.AntyParalyseSpell;
                APMana.Text = ModulesManager.autoHaste.ManaCost.ToString();
                APEnable.IsChecked = true;
            }

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
                ModulesManager.SioEnable();
            }
            catch (Exception)
            {
                Error.Visibility = Visibility.Visible;
            }
        }

        private void DisableAntyParalyse(object sender, RoutedEventArgs e)
        {
            ModulesManager.AntyParalyseDisable();
        }

        private void EnableAntyParalyse(object sender, RoutedEventArgs e)
        {
            try
            {
                ModulesManager.antyParalyse.AntyParalyseSpell = APSpell.Text;
                ModulesManager.antyParalyse.ManaCost = int.Parse(APMana.Text);
                ModulesManager.AntyParalyseEnable();
            }
            catch (Exception)
            {
                Error.Visibility = Visibility.Visible;
            }
        }
    }
}
