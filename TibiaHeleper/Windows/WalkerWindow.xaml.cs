using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using TibiaHeleper.MemoryOperations;
using TibiaHeleper.Modules;
using TibiaHeleper.Modules.WalkerModule;
using TibiaHeleper.Storage;

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

        private string startLabelName;
        private int startIndex;

        private void Load(object sender, RoutedEventArgs e)
        {
            startIndex = ModulesManager.walker.actualStatementIndex;
            list = ModulesManager.walker.CopyList();
            tolerance = ModulesManager.walker.tolerance;
            fillList();
            listBox.SelectedIndex = startIndex;

            foreach (DictionaryEntry item in StatementType.getType)
            {
                if ((int)item.Value > (int)StatementType.getType["action"])
                {
                    ActionsListBox.DisplayMemberPath = "Key";
                    ActionsListBox.Items.Add(item);
                }

            }
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
            foreach (Waypoint waypoint in trackedList)
            {
                list.Add(waypoint);
            }
            fillList();
            startButton.Visibility = Visibility.Visible;
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
            if (wasWorking == true) ModulesManager.WalkerDisable();
            while (!ModulesManager.walker.stopped) ;

            ModulesManager.walker.SetList(list);
            if (startLabelName != "") ModulesManager.walker.startStatementIndex = list.FindIndex(x => x.name == startLabelName);

            if (!wasWorking) ModulesManager.walker.startStatementIndex = startIndex;
            if (wasWorking) ModulesManager.WalkerEnable();

            list = ModulesManager.walker.CopyList();
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

            startLabelName = startLabel.Text;
            if (labelExist(startLabelName))
                startLabelName = "";
        }
        private bool labelExist(string name)
        {
            foreach (WalkerStatement item in list)
            {
                if (item.name == name)
                    return true;
            }
            return false;
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


        private void Down(object sender, RoutedEventArgs e)
        {
            WalkerStatement selectedItem = (WalkerStatement)listBox.SelectedItem;
            int selectedIndex = list.IndexOf(selectedItem);
            if (selectedIndex < list.Count - 1)
            {
                list.RemoveAt(selectedIndex);
                list.Insert(selectedIndex - 1, selectedItem);
                fillList();
                listBox.SelectedIndex = selectedIndex + 1;
            }
        }
        private void Up(object sender, RoutedEventArgs e)
        {
            WalkerStatement selectedItem = (WalkerStatement)listBox.SelectedItem;
            int selectedIndex = list.IndexOf(selectedItem);
            if (selectedIndex > 0)
            {
                list.RemoveAt(selectedIndex);
                list.Insert(selectedIndex - 1, selectedItem);
                fillList();
                listBox.SelectedIndex = selectedIndex - 1;
            }
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            list.Remove((WalkerStatement)listBox.SelectedItem);
            fillList();
        }

        public void Update()
        {
            this.Dispatcher.Invoke(() =>
            {
                Creature me = GetData.Me;
                if (me != null)
                    InformationLabel.Content = me.name + ": X: " + me.XPosition + "  Y: " + me.YPosition + " Floor: " + me.Floor;
            });
        }

        private void GetMyCoordinates(object sender, RoutedEventArgs e)
        {
            Creature me = GetData.Me;
            XPositionTextBox.Text = me.XPosition.ToString();
            YPositionTextBox.Text = me.YPosition.ToString();
            FloorTextBox.Text = me.Floor.ToString();
        }
        
        private void hideActionFields()
        {
            TextActionGrid.Visibility = Visibility.Hidden;
            PositionGrid.Visibility = Visibility.Hidden;
            MouseClickGrid.Visibility = Visibility.Hidden;
     //       RightClickCheckBox.IsChecked = false;

        }

        private void ActionSelected(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            hideActionFields();
            string action = (string)((DictionaryEntry)ActionsListBox.SelectedItem).Key;
            if (action == "Hotkey")
            {
                TextActionGrid.Visibility = Visibility.Visible;
                InputDescriptionLabel.Content = "Hotkey";
            }
            else if (action == "Stand")
            {
                PositionGrid.Visibility = Visibility.Visible;
            }
            else if (action == "Use On Field")
            {
                PositionGrid.Visibility = Visibility.Visible;
                TextActionGrid.Visibility = Visibility.Visible;
                InputDescriptionLabel.Content = "Hotkey";
            }
            else if (action == "Say")
            {
                InputDescriptionLabel.Content = "Text To Say";
                TextActionGrid.Visibility = Visibility.Visible;
            }
            else if (action == "Go To Label")
            {
                InputDescriptionLabel.Content = "Label Name";
                TextActionGrid.Visibility = Visibility.Visible;
            }
            else if (action == "Mouse Click")
            {
                PositionGrid.Visibility = Visibility.Visible;
                MouseClickGrid.Visibility = Visibility.Visible;
            }
        }
    }
}
