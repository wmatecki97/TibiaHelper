using System;
using System.Collections.Generic;
using System.Windows;
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

        private string startLabellName;
        private int startIndex;

        private void Load(object sender, RoutedEventArgs e)
        {
            startIndex = ModulesManager.walker.actualStatementIndex;
            list = ModulesManager.walker.CopyList();
            tolerance = ModulesManager.walker.tolerance;
            fillList();
            listBox.SelectedIndex = startIndex;
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
            if (!wasWorking) ModulesManager.walker.startStatementIndex = startIndex;

            if (wasWorking) ModulesManager.WalkerEnable();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            WindowsManager.menu.Show();
            this.Hide();
        }

        private void AddLabel(object sender, RoutedEventArgs e)
        {
            String name = LabelTextBox.Text;
            bool isGood = true;
            foreach (WalkerStatement statement in list)
            {
                if (statement.name == name)
                    isGood = false;
            }
            if (isGood)
            {
                int index = listBox.SelectedIndex;
                if (index == -1) index = list.Count;
                WalkerLabel label = new WalkerLabel(name);
                list.Insert(index, label);
                fillList();
            }
            else
            {
                ErrorLabel.Content = "Unacceptable value";
                Error.Visibility = Visibility.Visible;
            }
        }

        private void SetStartLabel(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
