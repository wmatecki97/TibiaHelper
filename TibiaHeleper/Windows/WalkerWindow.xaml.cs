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
        private void insertToList(WalkerStatement item)
        {
            int index = list.IndexOf((WalkerStatement)listBox.SelectedItem) + 1;
            if (index == -1) index = 0;
            list.Insert(index, item);
            fillList();
            listBox.SelectedItem = item;
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
                insertToList(new WalkerLabel(name));
            }
            else
            {
                ErrorLabel.Content = "Not unique value";
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
                string text="";
                Creature me = GetData.Me;
                if (me != null)
                     text = me.name + ": X: " + me.XPosition + "  Y: " + me.YPosition + " Floor: " + me.Floor;
                if (ModulesManager.walker.working)
                    text += "\t Actual Statement: " + ModulesManager.walker.list[ModulesManager.walker.actualStatementIndex].name;
                InformationLabel.Content = text;
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
            else if (action == "Waypoint")
            {
                PositionGrid.Visibility = Visibility.Visible;
            }
        }
        /// <summary>
        /// returns coordinates entered by user. Parsing string to int and Throws an exception.
        /// </summary>
        /// <returns></returns>
        private int[] Coordinates()
        {
            int[] arr;
            int x = int.Parse(XPositionTextBox.Text);
            int y = int.Parse(YPositionTextBox.Text);
            int f = int.Parse(FloorTextBox.Text);
            arr = new int[] { x, y, f };
            return arr;
        }
        private void AddAction(object sender, RoutedEventArgs e)
        {
            int actionType = (int)((DictionaryEntry)ActionsListBox.SelectedItem).Value;
            WalkerStatement action = null;
            Hashtable type = StatementType.getType;
            try
            {
                if (actionType == (int)type["Waypoint"] || actionType == (int)type["Stand"])
                {
                    int[] position = Coordinates();
                    action = new Waypoint(position[0], position[1], position[2], actionType == (int)type["Stand"]);
                }
                else if (actionType == (int)type["Go To Label"])
                {
                    action = new Modules.WalkerModule.Action(actionType, ActionTextBox.Text);
                }
                else if (actionType == (int)type["Mouse Click"])
                {
                    int[] pos = Coordinates();
                    action = new Modules.WalkerModule.Action(actionType, pos[0], pos[1], pos[2], RightClickCheckBox.IsChecked);

                }
                else if (actionType == (int)type["Say"] || actionType == (int)type["Hotkey"])
                {
                    action = new Modules.WalkerModule.Action(actionType, ActionTextBox.Text);
                }
                else if (actionType == (int)type["Use On Field"])
                {
                    int[] pos = Coordinates();
                    action = new Modules.WalkerModule.Action(actionType, ActionTextBox.Text, pos[0], pos[1], pos[2]);
                }
                else return; //protect from adding null to the list
                insertToList(action);
            }
            catch (Exception)
            {
                ErrorLabel.Content = "Unacceptable value";
                Error.Visibility = Visibility.Visible;
            }

            //    Modules.WalkerModule.Action action = new Modules.WalkerModule.Action(actionType,;

        }
    }
}
