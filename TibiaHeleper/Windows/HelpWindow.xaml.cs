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
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        public HelpWindow()
        {
            InitializeComponent();
        }

        private void Initialize(object sender, EventArgs e)
        {
            string help = "Tibia Helper Help";
            string Actions_And_Hotkeys = "In field named Hotkey required content is hotkey set int Game. For example if you have \"exura\" spell on Shift F1 hotkey you should set this field to \"Shift + F1\" It doesn't matter if you use upper or lowercase letters. + sign is special sign if you want to use shift  or control key with Function key \n In field named \"Action\" required content is: ";
            help += Actions_And_Hotkeys;


            HelpText.Text = help;
        }
    }
}
