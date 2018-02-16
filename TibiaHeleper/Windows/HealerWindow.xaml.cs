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
                bool isWorking = ModuesManager.healer.working;
                if (isWorking)
                    ModuesManager.HealerDisable();
               
                ModuesManager.healer.lowHPButton = lowHPB.Text;
                ModuesManager.healer.medHPButton = medHPB.Text;
                ModuesManager.healer.highHPButton = highHPB.Text;

                if (lowHPMana.Text == "") lowHPMana.Text = "0";
                if (medHPMana.Text == "") medHPMana.Text = "0";
                if (highHPMana.Text == "") highHPMana.Text = "0";
                ModuesManager.healer.lowHPMana = int.Parse(lowHPMana.Text);
                ModuesManager.healer.medHPMana = int.Parse(medHPMana.Text);
                ModuesManager.healer.highHPMana = int.Parse(highHPMana.Text);

                if (lowHP.Text == "") lowHP.Text = "0";
                if (medHP.Text == "") medHP.Text = "0";
                if (highHP.Text == "") highHP.Text = "0";
                ModuesManager.healer.lowHP = int.Parse(lowHP.Text);
                ModuesManager.healer.medHP = int.Parse(medHP.Text);
                ModuesManager.healer.highHP = int.Parse(highHP.Text);

                if (lowMana.Text == "") lowMana.Text = "0";
                if (highMana.Text == "") highMana.Text = "0";
                ModuesManager.healer.lowMana = int.Parse(lowMana.Text);
                ModuesManager.healer.highMana = int.Parse(highMana.Text);
                ModuesManager.healer.lowManaButton = lMB.Text;
                ModuesManager.healer.highManaButton = hMB.Text;

               
                if(isWorking)
                    ModuesManager.HealerEnable();
            }
            catch(Exception err)
            {
                ErrorLabel.Content = "Unacceptable value";
                Error.Visibility = Visibility.Visible;
            }

           
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
            lowHP.Text = ModuesManager.healer.lowHP.ToString();
            medHP.Text = ModuesManager.healer.medHP.ToString();
            highHP.Text = ModuesManager.healer.highHP.ToString();
            lowHPB.Text = ModuesManager.healer.lowHPButton;
            medHPB.Text = ModuesManager.healer.medHPButton;
            highHPB.Text = ModuesManager.healer.highHPButton;

            lowHPMana.Text = ModuesManager.healer.lowHPMana.ToString();
            medHPMana.Text = ModuesManager.healer.medHPMana.ToString();
            highHPMana.Text = ModuesManager.healer.highHPMana.ToString();

            lowMana.Text = ModuesManager.healer.lowMana.ToString();
            highMana.Text = ModuesManager.healer.highMana.ToString();
            lMB.Text = ModuesManager.healer.lowManaButton;
            hMB.Text = ModuesManager.healer.highManaButton;
        }
    

        private void Close(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
