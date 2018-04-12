using System;
using System.Windows;
using TibiaHeleper.Modules;

namespace TibiaHeleper.Windows
{
    /// <summary>
    /// Interaction logic for HealerWindow.xaml
    /// </summary>
    public partial class HealerWindow : Window
    {
        public HealerWindow()
        {
            InitializeComponent();
        }

      
        private void Save(object sender, RoutedEventArgs e)
        {
            try
            {
                bool isWorking = ModulesManager.healer.working;
                if (isWorking)
                    ModulesManager.HealerDisable();
               
                ModulesManager.healer.lowHPAction = lowHPB.Text;
                ModulesManager.healer.medHPAction = medHPB.Text;
                ModulesManager.healer.highHPAction = highHPB.Text;

                if (lowHPMana.Text == "") lowHPMana.Text = "0";
                if (medHPMana.Text == "") medHPMana.Text = "0";
                if (highHPMana.Text == "") highHPMana.Text = "0";
                ModulesManager.healer.lowHPMana = int.Parse(lowHPMana.Text);
                ModulesManager.healer.medHPMana = int.Parse(medHPMana.Text);
                ModulesManager.healer.highHPMana = int.Parse(highHPMana.Text);

                if (lowHP.Text == "") lowHP.Text = "0";
                if (medHP.Text == "") medHP.Text = "0";
                if (highHP.Text == "") highHP.Text = "0";
                ModulesManager.healer.lowHP = int.Parse(lowHP.Text);
                ModulesManager.healer.medHP = int.Parse(medHP.Text);
                ModulesManager.healer.highHP = int.Parse(highHP.Text);

                if (lowMana.Text == "") lowMana.Text = "0";
                if (highMana.Text == "") highMana.Text = "0";
                ModulesManager.healer.lowMana = int.Parse(lowMana.Text);
                ModulesManager.healer.highMana = int.Parse(highMana.Text);
                ModulesManager.healer.lowManaAction = lMB.Text;
                ModulesManager.healer.highManaAction = hMB.Text;

               
                if(isWorking)
                    ModulesManager.HealerEnable();

                showPopUpWindow("Saved succesfully");

            }
            catch(Exception)
            {
                showPopUpWindow();
            }

           
        }

        private void HideErrorGrid(object sender, RoutedEventArgs e)
        {
            Error.Visibility = Visibility.Hidden;
        }

        public void showPopUpWindow(string errorMessage = "Unacceptable value")
        { 
            ErrorLabel.Content = errorMessage;
            Error.Visibility = Visibility.Visible;
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            WindowsManager.menu.Show();
            this.Hide();
        }

        private void SetValues(object sender, RoutedEventArgs e)
        {
            assignData();
        }
    
        public void assignData()
        {
            lowHP.Text = ModulesManager.healer.lowHP.ToString();
            medHP.Text = ModulesManager.healer.medHP.ToString();
            highHP.Text = ModulesManager.healer.highHP.ToString();
            lowHPB.Text = ModulesManager.healer.lowHPAction;
            medHPB.Text = ModulesManager.healer.medHPAction;
            highHPB.Text = ModulesManager.healer.highHPAction;

            lowHPMana.Text = ModulesManager.healer.lowHPMana.ToString();
            medHPMana.Text = ModulesManager.healer.medHPMana.ToString();
            highHPMana.Text = ModulesManager.healer.highHPMana.ToString();

            lowMana.Text = ModulesManager.healer.lowMana.ToString();
            highMana.Text = ModulesManager.healer.highMana.ToString();
            lMB.Text = ModulesManager.healer.lowManaAction;
            hMB.Text = ModulesManager.healer.highManaAction;
        }

        private void Close(object sender, System.ComponentModel.CancelEventArgs e)
        {
            WindowsManager.menu.Show();
            WindowsManager.healerWindow = new HealerWindow();
        }
    }
}
