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
using TibiaHeleper.Modules.WalkerModule;

namespace TibiaHeleper.Windows
{
    /// <summary>
    /// Interaction logic for WalkerWindow.xaml
    /// </summary>
    public partial class WalkerWindow : Window
    {
        public WalkerWindow()
        {
            InitializeComponent();
        }

        private bool workingOnCopy;
        private List<WalkerStatement> list;
        private int tolerance;

        private void Load(object sender, RoutedEventArgs e)
        {
            list = ModulesManager.walker.CopyList();
            tolerance = ModulesManager.walker.tolerance;
            fillList();
        }
        private void fillList()
        {
            listBox.Items.Clear();
            foreach (WalkerStatement item in list)
            {
                listBox.DisplayMemberPath = "name";
                listBox.Items.Add(item);
            }
        }

        public void ReloadData()
        {
            list = ModulesManager.walker.CopyList();
            fillList();
        }

        private void StartTracking(object sender, RoutedEventArgs e)
        {
            ModulesManager.TrackerEnable();
            startButton.Visibility = Visibility.Hidden;
        }

        private void StopTracking(object sender, RoutedEventArgs e)
        {
            ModulesManager.TrackerDisable();
            List<Waypoint> trackedList = ModulesManager.tracker.list;
            while (!ModulesManager.tracker.stopped) ;//waiting for tracker finished
            ModulesManager.tracker.list = new List<Waypoint>();
            foreach(Waypoint waypoint in trackedList)
            {
                list.Add(waypoint);
            }
            fillList();
            startButton.Visibility = Visibility.Visible;
        }

        private void SetAccuracy(object sender, RoutedEventArgs e)
        {
            try
            {
                ModulesManager.walker.tolerance = int.Parse(Accuracy.Text);
            }
            catch (Exception)
            {
                ErrorLabel.Content = "Unacceptable value";
                Error.Visibility = Visibility.Visible;
            }
        }

        private void HideErrorGrid(object sender, RoutedEventArgs e)
        {
            Error.Visibility = Visibility.Hidden;
        }

        private void Close(object sender, EventArgs e)
        {
            WindowsManager.menu.Show();
            WindowsManager.walkerWindow = new WalkerWindow();
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            bool wasWorking = ModulesManager.walker.working;
            if(wasWorking==true) ModulesManager.WalkerDisable();
            while (!ModulesManager.walker.stopped) ;
            ModulesManager.walker.SetList(list);
            if (wasWorking) ModulesManager.WalkerEnable();
        }
    }
}
