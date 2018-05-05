using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TibiaHeleper.MemoryOperations;
using TibiaHeleper.Modules;
using TibiaHeleper.Modules.Targeting;
using TibiaHeleper.Modules.TargetingModule;
using TibiaHeleper.Storage;

namespace TibiaHeleper.Windows
{
    /// <summary>
    /// Interaction logic for Targeting.xaml
    /// </summary>
    public partial class Targeting : System.Windows.Window
    {
        public Targeting()
        {
            InitializeComponent();
        }

        private List<Target> _list;
        private List<LootItem> _lootList;
        private List<Item> _foodList;
        private Target _target;
        private Item _item;
        private Item _lootContainer;
        private Item _nextContainer;

        private void Load(object sender, RoutedEventArgs e)
        {
            _list = ModulesManager.targeting.getTargetListCopy();
            _foodList = Storage.Storage.Copy(ModulesManager.targeting.foodList) as List<Item>;
            _lootList = Storage.Storage.Copy(ModulesManager.targeting.lootList) as List<LootItem>;
            _lootContainer = new Item("Default container", 0);
            _nextContainer = ModulesManager.targeting.nextContainer;
            OpenNextContainerCheckBox.IsChecked = ModulesManager.targeting.openNextContainer;

            fillAllLists();
            _target = new Target();
            clearAllTextBoxes();
        }

        private void fillList()
        {
            listBox.Items.Clear();
            foreach (Target item in _list)
            {
                listBox.DisplayMemberPath = "name";
                listBox.Items.Add(item);
            }
        }
        private void fillLootList()
        {
            LootListBox.Items.Clear();
            foreach (LootItem item in _lootList)
            {
                LootListBox.DisplayMemberPath = "displayedName";
                LootListBox.Items.Add(item);
            }
        }
        private void fillFoodList()
        {
            FoodListBox.Items.Clear();
            foreach (Item item in _foodList)
            {
                FoodListBox.DisplayMemberPath = "name";
                FoodListBox.Items.Add(item);
            }
        }
        private void fillAllLists()
        {
            fillList();
            fillLootList();
            fillFoodList();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            WindowsManager.menu.Show();
            this.Hide();
        }
        private void Down(object sender, RoutedEventArgs e)
        {
            if (_list.IndexOf(_target) > -1 && _list.IndexOf(_target) < _list.Count() - 1)
            {
                int index = _list.IndexOf(_target);
                _list.RemoveAt(index);
                _list.Insert(index + 1, _target);
                fillList();
                listBox.SelectedIndex = index + 1;
            }
        }
        private void Up(object sender, RoutedEventArgs e)
        {
            if (_list.IndexOf(_target) > 0)
            {
                int index = _list.IndexOf(_target);
                _list.RemoveAt(index);
                _list.Insert(index - 1, _target);
                fillList();
                listBox.SelectedIndex = index - 1;
            }

        }

        private void Add(object sender, RoutedEventArgs e)
        {
            _target = new Target();

            assignTarget();
            _list.Add(_target);
            fillList();
            _target = new Target();
            clearAllTextBoxes();

        }
        private void Delete(object sender, RoutedEventArgs e)
        {
            _list.Remove(_target);
            fillList();
            clearAllTextBoxes();
        }

        public void showPopUpWindow(string errorMessage = "Unacceptable value")
        {
            ErrorLabel.Content = errorMessage;
            Error.Visibility = Visibility.Visible;
        }

        private void assignTarget()
        {
            try
            {
                _target.name = Name.Text;
                _target.maxHP = int.Parse(maxHP.Text);
                _target.minHP = int.Parse(minHP.Text);
                _target.action = Action.Text;
                _target.maxDistance = int.Parse(maxDistance.Text);
                _target.followTarget = FollowTargetCheckBox.IsChecked.Value;
                _target.diagonal = DiagonalCheckBox.IsChecked.Value;
                _target.lookForFood = LookForFoodCheckBox.IsChecked.Value;
                _target.loot = LootCheckBox.IsChecked.Value;
            }
            catch (Exception)
            {
                showPopUpWindow();
            }

        }


        private void clearAllTextBoxes()
        {
            Name.Text = "";
            maxHP.Text = "100";
            minHP.Text = "0";
            Action.Text = "";
            maxDistance.Text = "11";
            FollowTargetCheckBox.IsChecked = false;
            LookForFoodCheckBox.IsChecked = false;
            LootCheckBox.IsChecked = false;

            ActualNextContainerLabel.Content = "Next backpack: " + _nextContainer.name;

        }
        private void setAllTextboxes()
        {
            Name.Text = _target.name;
            maxHP.Text = _target.maxHP.ToString();
            minHP.Text = _target.minHP.ToString();
            Action.Text = _target.action;
            FollowTargetCheckBox.IsChecked = _target.followTarget;
            LookForFoodCheckBox.IsChecked = _target.lookForFood;
            maxDistance.Text = _target.maxDistance.ToString();
            LootCheckBox.IsChecked = _target.loot;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            bool wasWorking = ModulesManager.targeting.working;
            if (wasWorking) ModulesManager.TargetingDisable();

            ModulesManager.targeting.setTargetList(_list);
            ModulesManager.targeting.setFoodList(_foodList);
            ModulesManager.targeting.setLootList(_lootList);
            ModulesManager.targeting.openNextContainer = OpenNextContainerCheckBox.IsChecked.Value;
            ModulesManager.targeting.nextContainer = _nextContainer;

            _foodList = Storage.Storage.Copy(ModulesManager.targeting.foodList) as List<Item>;
            _lootList = Storage.Storage.Copy(ModulesManager.targeting.lootList) as List<LootItem>;
            _list = ModulesManager.targeting.getTargetListCopy();

            if (wasWorking) ModulesManager.TargetingEnable();

            fillAllLists();
            showPopUpWindow("Saved succesfully");

        }

        private void SelectedAction(object sender, RoutedEventArgs e)
        {
            _target = listBox.SelectedItem as Target;
            if (_target != null)
                setAllTextboxes();
            else
                clearAllTextBoxes();
        }

        private void HideErrorGrid(object sender, RoutedEventArgs e)
        {
            Error.Visibility = Visibility.Hidden;
        }

        private void Close(object sender, System.ComponentModel.CancelEventArgs e)
        {
            WindowsManager.menu.Show();
            WindowsManager.targeting = new Targeting();
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            assignTarget();
            fillList();
        }

        private void CheckForHint(object sender, System.Windows.Controls.TextChangedEventArgs e)
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

        private void AddToLootList(object sender, RoutedEventArgs e)
        {
            if (_item != null)
            {
                LootItem lootItem = new LootItem(_item.name, _item.ID, _lootContainer);

                _lootList.Add(lootItem);
                ItemTextBox.Text = "";
                fillLootList();
            }
        }
        private void AddToFoodList(object sender, RoutedEventArgs e)
        {
            if (_item != null)
            {
                _foodList.Add(_item);
                ItemTextBox.Text = "";
                fillFoodList();
            }
        }
        private void DeleteFromLootList(object sender, RoutedEventArgs e)
        {
            LootItem it = LootListBox.SelectedItem as LootItem;
            if (it != null)
            {
                _lootList.Remove(it);
                fillLootList();
            }
        }
        private void DeleteFromFoodList(object sender, RoutedEventArgs e)
        {
            Item it = FoodListBox.SelectedItem as Item;
            if (it != null)
            {
                _foodList.Remove(it);
                fillLootList();
            }
        }

        private void SetLootContainer(object sender, RoutedEventArgs e)
        {
            if (_item != null)
            {
                _lootContainer = _item;
                ItemTextBox.Text = "";
                ActualLootContainerLabel.Content = "Actual loot container: " + _lootContainer.name;
            }
            else
                _lootContainer = new Item("Default container", 0);

        }

        private void SetNextContainer(object sender, RoutedEventArgs e)
        {
            if (_item != null)
            {
                _nextContainer = _item;
                ItemTextBox.Text = "";
                ActualNextContainerLabel.Content = "Next backpack: " + _nextContainer.name;
            }
            else
                _nextContainer = ItemList.Items.First(item => item.name == "Backpack");
        }
    }
}
