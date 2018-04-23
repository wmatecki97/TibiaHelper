using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
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

        private List<WalkerStatement> StatementsList;
        private int tolerance;

        List<Modules.WalkerModule.Condition> conditionsList;
        Modules.WalkerModule.Condition condition;
        short NotSet;
        List<TradeItem> tradeList;

        private string startLabelName;
        private int startIndex;

        private Item _item;

        private void Load(object sender, RoutedEventArgs e)
        {
            startIndex = ModulesManager.walker.actualStatementIndex;
            StatementsList = ModulesManager.walker.CopyList();
            tolerance = ModulesManager.walker.tolerance;
            fillList();
            NotSet = StatementType.conditionElement["Not set"];
            listBox.SelectedIndex = startIndex;

            foreach (KeyValuePair<string, int> item in StatementType.getType)
            {
                if ((int)item.Value > StatementType.getType["Action"])
                {
                    ActionsListBox.DisplayMemberPath = "Key";
                    ActionsListBox.Items.Add(item);
                }

            }

           
            SellOrBuyComboBox.Items.Add("Sell");
            SellOrBuyComboBox.Items.Add("Buy");
            SellOrBuyComboBox.SelectedIndex = 0;

            tradeList = new List<TradeItem>();

        }
        private void fillList()
        {
            listBox.Items.Clear();
            foreach (WalkerStatement item in StatementsList)
            {
                listBox.DisplayMemberPath = "name";
                listBox.Items.Add(item);
            }
        }
        private void insertToList(WalkerStatement item)
        {
            int index = StatementsList.IndexOf((WalkerStatement)listBox.SelectedItem) + 1;
            if (index == -1) index = listBox.Items.Count - 1;
            StatementsList.Insert(index, item);
            fillList();
            listBox.SelectedIndex = index;
        }

        public void ReloadData()
        {
            StatementsList = ModulesManager.walker.CopyList();
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
            List<WalkerStatement> trackedList = ModulesManager.tracker.list;
            while (!ModulesManager.tracker.stopped) ;//waiting for tracker finished
            ModulesManager.tracker.list = new List<WalkerStatement>();
            Way way = new Way(trackedList);
            insertToList(way);
            fillList();
            listBox.SelectedItem = way;
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

            ModulesManager.walker.SetList(Way.changeWayToWaypointList(StatementsList));
            if (startLabelName != "") ModulesManager.walker.startStatementIndex = StatementsList.FindIndex(x => x.name == startLabelName);

            if (!wasWorking) ModulesManager.walker.startStatementIndex = startIndex;
            if (wasWorking) ModulesManager.WalkerEnable();

            StatementsList = ModulesManager.walker.CopyList();
            fillList();

            showPopUpWindow("Saved succesfully");
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
            foreach (WalkerStatement statement in StatementsList)
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
                showPopUpWindow("Not unique value");
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
            foreach (WalkerStatement item in StatementsList)
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
                showPopUpWindow();
            }
        }


        private void Down(object sender, RoutedEventArgs e)
        {
            WalkerStatement selectedItem = (WalkerStatement)listBox.SelectedItem;
            int selectedIndex = StatementsList.IndexOf(selectedItem);
            if (selectedIndex < StatementsList.Count - 1)
            {
                StatementsList.RemoveAt(selectedIndex);
                StatementsList.Insert(selectedIndex + 1, selectedItem);
                fillList();
                listBox.SelectedIndex = selectedIndex + 1;
            }
        }
        private void Up(object sender, RoutedEventArgs e)
        {
            WalkerStatement selectedItem = (WalkerStatement)listBox.SelectedItem;
            int selectedIndex = StatementsList.IndexOf(selectedItem);
            if (selectedIndex > 0)
            {
                StatementsList.RemoveAt(selectedIndex);
                StatementsList.Insert(selectedIndex - 1, selectedItem);
                fillList();
                listBox.SelectedIndex = selectedIndex - 1;
            }
        }
        private void Delete(object sender, RoutedEventArgs e)
        {
            StatementsList.Remove((WalkerStatement)listBox.SelectedItem);
            fillList();
        }
        private void Edit(object sender, RoutedEventArgs e)
        {
            WalkerStatement selected = listBox.SelectedItem as WalkerStatement;
            string action="";
            int selectedType = selected.type;
            if (selected != null)
            {
                hideActionFields();

                if(selected.type == StatementType.getType["Action"])
                {
                    Modules.WalkerModule.Action act = selected as Modules.WalkerModule.Action;
                    selectedType = act.defaultAction;
                }
                foreach (var type in StatementType.getType)
                {
                    if (type.Value == selectedType)
                    {
                        ActionsListBox.SelectedItem = type;
                        action = StatementType.getTypeName(selectedType);
                        break;
                    }
                }


                if (action == "Hotkey")
                {
                    Modules.WalkerModule.Action statement = selected as Modules.WalkerModule.Action;
                    ActionTextBox.Text = statement.args[0].ToString();
                }
                else if (action == "Stand")
                {
                    Waypoint statement = selected as Waypoint;
                    setPositionTextBoxes(statement);
                }
                else if (action == "Use On Field")
                {
                    Modules.WalkerModule.Action statement = selected as Modules.WalkerModule.Action;
                    ActionTextBox.Text = statement.args[0].ToString();

                    XPositionTextBox.Text = statement.args[1].ToString();
                    YPositionTextBox.Text = statement.args[2].ToString();
                    FloorTextBox.Text = statement.args[3].ToString();
                }
                else if (action == "Say")
                {
                    Modules.WalkerModule.Action statement = selected as Modules.WalkerModule.Action;
                    ActionTextBox.Text = statement.args[0].ToString();
                }
                else if (action == "Go To Label")
                {
                    Modules.WalkerModule.Action statement = selected as Modules.WalkerModule.Action;
                    ActionTextBox.Text = statement.args[0].ToString();
                }
                else if (action == "Mouse Click")
                {
                    Modules.WalkerModule.Action statement = selected as Modules.WalkerModule.Action;
                    XPositionTextBox.Text = statement.args[0].ToString();
                    YPositionTextBox.Text = statement.args[1].ToString();
                    FloorTextBox.Text = statement.args[2].ToString();
                    RightClickCheckBox.IsChecked = statement.args[3] as bool?;
                }
                else if (action == "Waypoint")
                {
                    Waypoint statement = selected as Waypoint;
                    setPositionTextBoxes(statement);
                }
                else if (action == "Condition")
                {
                    CreateCondition(null, null);
                    Modules.WalkerModule.Action statement = selected as Modules.WalkerModule.Action;
                    ConditionFulfilledTextBox.Text = statement.args[0].ToString();
                    conditionsList = statement.args[1] as List<Modules.WalkerModule.Condition>;
                    refreshCondition();
                }
                else if(action == "Trade")
                {
                    Modules.WalkerModule.Action statement = selected as Modules.WalkerModule.Action;
                    tradeList = (List<TradeItem>)statement.args[0];
                    ShowTradeGrid(new object(), new RoutedEventArgs());
                    refreshTradeList();
                }
            }
        }
        private void setPositionTextBoxes(Waypoint waypoint)
        {
            XPositionTextBox.Text = waypoint.xPos.ToString();
            YPositionTextBox.Text = waypoint.yPos.ToString();
            FloorTextBox.Text = waypoint.floor.ToString();
        }

        public void Update()
        {

            this.Dispatcher.Invoke(() =>
            {
                string text = "";
                int index;
                Creature me = GetData.Me;
                if (me != null)
                    text = me.name + ": X: " + me.XPosition + "  Y: " + me.YPosition + " Floor: " + me.Floor;
                if (ModulesManager.walker.working && (index = ModulesManager.walker.actualStatementIndex) >= 0)
                    text += "\t Actual Statement: " + ModulesManager.walker.list[index].name;
                InformationLabel.Content = text;
            });
        }
        public void showPopUpWindow(string errorMessage = "Unacceptable value")
        {
            ErrorLabel.Text = errorMessage;
            Error.Visibility = Visibility.Visible;
        }

        private void GetMyCoordinates(object sender, RoutedEventArgs e)
        {
            Creature me = GetData.Me;
            if (me != null)
            {
                XPositionTextBox.Text = me.XPosition.ToString();
                YPositionTextBox.Text = me.YPosition.ToString();
                FloorTextBox.Text = me.Floor.ToString();
            }

        }
        private void hideActionFields()
        {
            TextActionGrid.Visibility = Visibility.Hidden;
            PositionGrid.Visibility = Visibility.Hidden;
            MouseClickGrid.Visibility = Visibility.Hidden;
            ConditionButtonGrid.Visibility = Visibility.Hidden;
            ConditionGrid.Visibility = Visibility.Hidden;
            //       RightClickCheckBox.IsChecked = false;
            TradeButtonGrid.Visibility = Visibility.Hidden;

        }
        private void ActionSelected(object sender, SelectionChangedEventArgs e)
        {
            hideActionFields();
            string action = ((KeyValuePair<string, int>)ActionsListBox.SelectedItem).Key;
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
            else if (action == "Condition")
            {
                ConditionButtonGrid.Visibility = Visibility.Visible;
            }
            else if (action == "Wait")
            {
                InputDescriptionLabel.Content = "Time to wait";
                TextActionGrid.Visibility = Visibility.Visible;
            }
            else if (action == "Trade")
            {
                TradeButtonGrid.Visibility = Visibility.Visible;
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
            int actionType = ((KeyValuePair<string, int>)ActionsListBox.SelectedItem).Value;
            WalkerStatement action = null;
            Dictionary<string, int> type = StatementType.getType;
            try
            {
                if (actionType == type["Waypoint"] || actionType == type["Stand"])
                {
                    int[] position = Coordinates();
                    action = new Waypoint(position[0], position[1], position[2], actionType == type["Stand"]);
                }
                else if (actionType == type["Go To Label"])
                {
                    action = new Modules.WalkerModule.Action(actionType, ActionTextBox.Text);
                }
                else if (actionType == type["Mouse Click"])
                {
                    int[] pos = Coordinates();
                    action = new Modules.WalkerModule.Action(actionType, pos[0], pos[1], pos[2], RightClickCheckBox.IsChecked);

                }
                else if (actionType == type["Say"] || actionType == type["Hotkey"] || actionType == type["Hotkey"])
                {
                    action = new Modules.WalkerModule.Action(actionType, ActionTextBox.Text);
                }
                else if (actionType == type["Use On Field"])
                {
                    int[] pos = Coordinates();
                    action = new Modules.WalkerModule.Action(actionType, ActionTextBox.Text, pos[0], pos[1], pos[2]);
                }
                else if (actionType == type["Wait"])
                {
                    action = new Modules.WalkerModule.Action(actionType, int.Parse(ActionTextBox.Text));
                }
                else if (actionType == type["Condition"])
                {
                    if (conditionsList.Count > 0)
                    {
                        if (condition.connector == StatementType.conditionElement["Not set"])
                        {
                            if (ConditionFulfilledTextBox.Text != "")
                            {
                                action = new Modules.WalkerModule.Action(actionType, ConditionFulfilledTextBox.Text, conditionsList);
                            }
                            else
                            {
                                showPopUpWindow("Condition must has specified label name to go to when condition is fulfilled");
                                return;
                            }
                        }
                        else
                        {
                            showPopUpWindow("Condition not completed. Every particular condition contains first value comparator and second value.");
                            return;
                        }
                    }
                    else
                    {
                        showPopUpWindow("You can not add empty condition");
                        return;
                    }
                }
                else if (actionType == type["Trade"])
                {
                    if(tradeList.Count > 0)
                    {
                        action = new Modules.WalkerModule.Action(actionType, tradeList);
                        tradeList = new List<TradeItem>();
                        AmountTextBox.Text = "";
                        refreshTradeList();
                        TradeGrid.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        showPopUpWindow("Can not add empty action. Add an action first.");
                    }
                }
               


                else return; //protect from adding null to the list
                insertToList(action);
                ConditionGrid.Visibility = Visibility.Hidden;
            }
            catch (Exception)
            {
                showPopUpWindow();
            }

            //    Modules.WalkerModule.Action action = new Modules.WalkerModule.Action(actionType,;

        }

        private void LoadProcedure(object sender, RoutedEventArgs e)
        {
            int a = listBox.SelectedIndex;
            List<WalkerStatement> procedure = Storage.Storage.LoadProcedure();
            if (procedure != null) //if cancelled on file choose
            {
                foreach (WalkerStatement statement in procedure)
                    insertToList(statement);
            }

        }
        private void SaveProcedure(object sender, RoutedEventArgs e)
        {
            IList lst = listBox.SelectedItems;
            if (lst.Count <= 0)
            {
                showPopUpWindow("Select at least one statement");
            }
            else
            {
                List<WalkerStatement> StoreList = new List<WalkerStatement>();
                foreach (object item in lst)
                {
                    StoreList.Add((WalkerStatement)item);
                }

                Storage.Storage.SaveProcedure(StoreList);
            }
        }
        private void HideProcedureGrid(object sender, RoutedEventArgs e)
        {
            ProcedureGrid.Visibility = Visibility.Hidden;

            BasicsWalkerOperationsGrid.Visibility = Visibility.Visible;
            ActionGrid.Visibility = Visibility.Visible;

            listBox.SelectionMode = SelectionMode.Single;
        }
        private void Procedure(object sender, RoutedEventArgs e)
        {
            BasicsWalkerOperationsGrid.Visibility = Visibility.Hidden;
            ActionGrid.Visibility = Visibility.Hidden;

            ProcedureGrid.Visibility = Visibility.Visible;
            listBox.SelectionMode = SelectionMode.Multiple;
        }
        private void SelectAll(object sender, RoutedEventArgs e)
        {

            listBox.SelectAll();

        }
        private void DeselectAll(object sender, RoutedEventArgs e)
        {
            listBox.UnselectAll();
        }

        private void changePossitionTextBox(TextBox t, int modifier)
        {
            try
            {
                if (t.Text != "")
                    t.Text = (int.Parse(t.Text) + modifier).ToString();
            }
            catch
            {
                showPopUpWindow();
            }
        }
        private void North(object sender, RoutedEventArgs e)
        {
            changePossitionTextBox(YPositionTextBox, -1);
        }
        private void South(object sender, RoutedEventArgs e)
        {
            changePossitionTextBox(YPositionTextBox, 1);
        }
        private void East(object sender, RoutedEventArgs e)
        {
            changePossitionTextBox(XPositionTextBox, 1);
        }
        private void West(object sender, RoutedEventArgs e)
        {
            changePossitionTextBox(XPositionTextBox, -1);
        }
        private void FloorUp(object sender, RoutedEventArgs e)
        {
            changePossitionTextBox(FloorTextBox, -1);
        }
        private void FloorDown(object sender, RoutedEventArgs e)
        {
            changePossitionTextBox(FloorTextBox, -1);
        }


        private void CreateCondition(object sender, RoutedEventArgs e)
        {
            ConditionGrid.Visibility = Visibility.Visible;
            conditionsList = new List<Modules.WalkerModule.Condition>();
            condition = new Modules.WalkerModule.Condition();
            ConditionText.Text = "";
            ConditionFulfilledTextBox.Text = "";
        }

        private string setText(Modules.WalkerModule.Condition cond, string text = "")
        {
            int argsNumber = 0;

            if (cond.connector != NotSet)
                text += StatementType.getConditionElementName(cond.connector) + "\n";
            if (cond.item1 == StatementType.conditionElement["Value"])
                text += cond.args[argsNumber++];
            else if (cond.item1 == StatementType.conditionElement["Item count"])
                text += cond.args[argsNumber++] + " count";
            else
                text += StatementType.getConditionElementName(cond.item1);

            text += " " + StatementType.getConditionElementName(cond.comparator) + " ";

            if (cond.item2 == StatementType.conditionElement["Value"])
                text += cond.args[argsNumber++];
            else if (cond.item2 == StatementType.conditionElement["Item count"])
                text += cond.args[argsNumber++] + " count";
            else
                text += StatementType.getConditionElementName(cond.item2);

            return text;
        }
        private void refreshCondition()
        {
            string text = "";
            foreach (Modules.WalkerModule.Condition cond in conditionsList)
            {
                text = setText(cond, text);
            }
            text = setText(condition, text);

            text = text.Replace("Not set", "");
            ConditionText.Text = text;
        }
        private void setComparator(string cond)
        {
            if (condition.item1 != NotSet)
            {
                condition.comparator = StatementType.conditionElement[cond];
            }
            refreshCondition();
        }
        private void setConnector(string connector)
        {
            if (conditionsList.Count > 0)
            {
                condition.connector = StatementType.conditionElement[connector];
            }
            refreshCondition();
        }
        private void setItem(string item, object value = null)
        {

            if (condition.connector != NotSet || conditionsList.Count == 0)
            {
                if (value != null)
                    condition.args.Add(value);
                if (condition.item1 == NotSet)
                {
                    condition.item1 = StatementType.conditionElement[item];
                }
                else if (condition.comparator != NotSet)
                {
                    condition.item2 = StatementType.conditionElement[item];
                    conditionsList.Add(condition);
                    condition = new Modules.WalkerModule.Condition();
                }
                refreshCondition();
            }



        }

        private void GreaterButtonClicked(object sender, RoutedEventArgs e)
        {
            setComparator(">");
        }
        private void SmallerButtonClicked(object sender, RoutedEventArgs e)
        {
            setComparator("<");

        }
        private void EqualButtonClicked(object sender, RoutedEventArgs e)
        {
            setComparator("=");

        }
        private void NotEqualButtonClicked(object sender, RoutedEventArgs e)
        {
            setComparator("!=");

        }
        private void AndButtonClicked(object sender, RoutedEventArgs e)
        {
            setConnector("And");
        }
        private void OrButtonClicked(object sender, RoutedEventArgs e)
        {
            setConnector("Or");
        }
        private void DeleteButtonClicked(object sender, RoutedEventArgs e)
        {
            if (conditionsList.Count > 0)
            {
                conditionsList.RemoveAt(conditionsList.Count - 1);
            }
            refreshCondition();
        }
        private void AddValueButtonClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                setItem("Value", int.Parse(ValueTextBox.Text));
            }
            catch (Exception)
            {
                showPopUpWindow();
            }
        }
        private void AddHpButtonClicked(object sender, RoutedEventArgs e)
        {
            setItem("HP");
        }
        private void AddManaButtonClicked(object sender, RoutedEventArgs e)
        {
            setItem("Mana");
        }
        private void AddCapButtonClicked(object sender, RoutedEventArgs e)
        {
            setItem("Cap");
        }
        private void ItemCountButtonClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                setItem("Item count", ItemCountTextBox.Text);
            }
            catch (Exception)
            {
                showPopUpWindow();
            }
        }
        private void CancelCondition(object sender, RoutedEventArgs e)
        {
            condition = new Modules.WalkerModule.Condition();
            conditionsList = new List<Modules.WalkerModule.Condition>();

            refreshCondition();
            ConditionGrid.Visibility = Visibility.Hidden;

        }

        private void ShowTradeGrid(object sender, RoutedEventArgs e)
        {
            TradeGrid.Visibility = Visibility.Visible;
        }

        private void CancelTrade(object sender, RoutedEventArgs e)
        {
            TradeGrid.Visibility = Visibility.Hidden;
        }

        private void CheckForHint(object sender, TextChangedEventArgs e)
        {
            string text = ItemTextBox.Text.ToUpper();
            if (text != "")
            {
                _item = ItemList.GetItemByPartOfItsName(text);
                if (_item != null)
                {
                    HintTextBox.Text = _item.name + " " + _item.ID;
                }
                else
                    HintTextBox.Text = "";
            }
            else // when text was deleted
            {
                HintTextBox.Text = "";
            }
        }

        private void TradeTypeChanged(object sender, SelectionChangedEventArgs e)
        {
            if(SellOrBuyComboBox.SelectedItem.ToString() == "Buy")
            {
                BuyItemCountGrid.Visibility = Visibility.Visible;
            }
            else
                BuyItemCountGrid.Visibility = Visibility.Hidden;

        }

        private void DeleteItemFromTradeList(object sender, RoutedEventArgs e)
        {
            if (TradeListBox.SelectedIndex >= 0)
            {
                TradeItem item = TradeListBox.SelectedItem as TradeItem;
                tradeList.Remove(item);
                refreshTradeList();
            }

        }

        private void AddItemToTradeList(object sender, RoutedEventArgs e)
        {
            if(_item != null)
            {
                int amount=-1;
                if (SellOrBuyComboBox.SelectedValue.ToString() == "Buy") 
                {
                    if(int.TryParse(AmountTextBox.Text, out amount))
                    {
                        tradeList.Add(new TradeItem(_item, TradeItem.Action.Buy, amount));
                        refreshTradeList();
                    }
                    else
                    {
                        showPopUpWindow("Amount must be a number bigger than 0");
                    }
                }               
                if(SellOrBuyComboBox.SelectedValue.ToString() == "Sell")
                {
                    tradeList.Add(new TradeItem(_item, TradeItem.Action.Sell));
                    refreshTradeList();
                }
            }
            else
            {
                showPopUpWindow("Item must be specyfied");
            }
            
        }

        private void refreshTradeList()
        {
            TradeListBox.Items.Clear();
            foreach (TradeItem item in tradeList)
            {
                TradeListBox.DisplayMemberPath = "displayName";
                TradeListBox.Items.Add(item);
            }
        }
    }
}
