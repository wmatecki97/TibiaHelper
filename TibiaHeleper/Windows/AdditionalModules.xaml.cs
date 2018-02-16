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
            AHEnable = ModuesManager.AutoHaste.;
        }

        private void EnableAutoHaste(object sender, RoutedEventArgs e)
        {
            ModuesManager.AutoHasteEnable();
        }

        private void DisableAutoHate(object sender, RoutedEventArgs e)
        {
            ModuesManager.AutoHasteDisable();
        }

        
    }
}
