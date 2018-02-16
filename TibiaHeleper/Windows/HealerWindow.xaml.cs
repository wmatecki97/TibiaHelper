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
                string lHPB = lowHPB.Text;
                string mHPB = medHPB.Text;
                string hHPB = highHPB.Text;
                if (lowHP.Text == "") lowHP.Text = "0";
                if (medHP.Text == "") medHP.Text = "0";
                if (highHP.Text == "") highHP.Text = "0";
                int lHP = int.Parse(lowHP.Text);
                int mHP = int.Parse(medHP.Text);
                int hHP = int.Parse(highHP.Text);
                if (lowHPMana.Text == "") lowHPMana.Text = "0";
                if (medHPMana.Text == "") medHPMana.Text = "0";
                if (highHPMana.Text == "") highHPMana.Text = "0";
                int lHPMana = int.Parse(lowHPMana.Text);
                int mHPMana = int.Parse(medHPMana.Text);
                int hHPMana = int.Parse(highHPMana.Text);

                if (lowMana.Text == "") lowMana.Text = "0";
                if (highMana.Text == "") highMana.Text = "0";
                int lMana = int.Parse(lowMana.Text);
                int hMana = int.Parse(highMana.Text);
                string lManaB = lMB.Text;
                string hManaB = hMB.Text;


                bool isWorking = Modules.Healer.isWorking();
                if(isWorking)
                    Modules.ModuesManager.HealerDisable();
                Modules.Healer.AsignValues(lHP, mHP, hHP, lHPB, mHPB, hHPB, lHPMana, mHPMana, hHPMana, lMana, hMana, lManaB, hManaB);
                if(isWorking)
                    Modules.ModuesManager.HealerRun();
            }
            catch(Exception err)
            {
                ErrorLabel.Content = "Unacceptable value";
                Error.Visibility = Visibility.Visible;
            }

            

          //  Modules.Healer.AsignValues();
        }

        private void HideErrorGrid(object sender, RoutedEventArgs e)
        {
            Error.Visibility = Visibility.Hidden;
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Hide();
        }

        private void SetValues(object sender, RoutedEventArgs e)
        {
            lowHP.Text = Modules.Healer.getLowHP();
            medHP.Text = Modules.Healer.getMedHP();
            highHP.Text = Modules.Healer.getHighHP();
            lowHPB.Text = Modules.Healer.getLowHPB();
            medHPB.Text = Modules.Healer.getMedHPB();
            highHPB.Text = Modules.Healer.getHighHPB();

            lowHPMana.Text = Modules.Healer.getlowHPMana();
            medHPMana.Text = Modules.Healer.getmedHPMana();
            highHPMana.Text = Modules.Healer.gethighHPMana();

            lowMana.Text = Modules.Healer.getLowMana();
            highMana.Text = Modules.Healer.getHighMana();
            lMB.Text = Modules.Healer.getLowManaButton();
            hMB.Text = Modules.Healer.getHighManaButton();
        }
    

        private void Close(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
