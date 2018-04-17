using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TibiaHeleper.MemoryOperations;
using TibiaHeleper.Modules;
using TibiaHeleper.Modules.Targeting;
using TibiaHeleper.Storage;

namespace TibiaHeleper.Windows
{
    /// <summary>
    /// Interaction logic for Targeting.xaml
    /// </summary>
    public partial class Targeting : Window
    {
        public Targeting()
        {
            InitializeComponent();
        }

        private List<Target> list;
        private List<Item> lootList;
        private List<Item> foodList;
        private Target target;
        private Item item;

        private void Load(object sender, RoutedEventArgs e)
        {
            list = ModulesManager.targeting.getTargetListCopy();
            foodList = Storage.Storage.Copy(ModulesManager.targeting.foodList) as List<Item>;
            lootList = Storage.Storage.Copy(ModulesManager.targeting.lootList) as List<Item>;

            fillAllLists();
            target = new Target();
            clearAllTextBoxes();
        }

        private void fillList()
        {
            listBox.Items.Clear();
            foreach (Target item in list)
            {
                listBox.DisplayMemberPath = "name";
                listBox.Items.Add(item);
            }
        }
        private void fillLootList()
        {
            LootListBox.Items.Clear();
            foreach (Item item in lootList)
            {
                LootListBox.DisplayMemberPath = "name";
                LootListBox.Items.Add(item);
            }
        }
        private void fillFoodList()
        {
            FoodListBox.Items.Clear();
            foreach (Item item in foodList)
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
            if (list.IndexOf(target) > -1 && list.IndexOf(target) < list.Count() - 1)
            {
                int index = list.IndexOf(target);
                list.RemoveAt(index);
                list.Insert(index + 1, target);
                fillList();
                listBox.SelectedIndex = index + 1;
            }
        }
        private void Up(object sender, RoutedEventArgs e)
        {
            if (list.IndexOf(target) > 0)
            {
                int index = list.IndexOf(target);
                list.RemoveAt(index);
                list.Insert(index - 1, target);
                fillList();
                listBox.SelectedIndex = index - 1;
            }

        }

        private void Add(object sender, RoutedEventArgs e)
        {
            target = new Target();

            assignTarget();
            list.Add(target);
            fillList();
            target = new Target();
            clearAllTextBoxes();

        }
        private void Delete(object sender, RoutedEventArgs e)
        {
            list.Remove(target);
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
                target.name = Name.Text;
                target.maxHP = int.Parse(maxHP.Text);
                target.minHP = int.Parse(minHP.Text);
                target.action = Action.Text;
                target.maxDistance = int.Parse(maxDistance.Text);
                target.followTarget = (bool)FollowTargetCheckBox.IsChecked;
                target.diagonal = (bool)DiagonalCheckBox.IsChecked;
                target.lookForFood = (bool)LookForFoodCheckBox.IsChecked;
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
        }
        private void setAllTextboxes()
        {
            Name.Text = target.name;
            maxHP.Text = target.maxHP.ToString();
            minHP.Text = target.minHP.ToString();
            Action.Text = target.action;
            FollowTargetCheckBox.IsChecked = target.followTarget;
            LookForFoodCheckBox.IsChecked = target.lookForFood;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            bool wasWorking = ModulesManager.targeting.working;
            if (wasWorking) ModulesManager.TargetingDisable();

            ModulesManager.targeting.setTargetList(list);
            ModulesManager.targeting.setFoodList(foodList);
            ModulesManager.targeting.setLootList(lootList);

            foodList = Storage.Storage.Copy(ModulesManager.targeting.foodList) as List<Item>;
            lootList = Storage.Storage.Copy(ModulesManager.targeting.lootList) as List<Item>;
            list = ModulesManager.targeting.getTargetListCopy();

            if (wasWorking) ModulesManager.TargetingEnable();

            fillAllLists();
            showPopUpWindow("Saved succesfully");

        }

        private void SelectedAction(object sender, RoutedEventArgs e)
        {
            target = listBox.SelectedItem as Target;
            if (target != null)
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
                item = ItemList.Items.FirstOrDefault(i => i.name.ToUpper().StartsWith(text));
                if (item != null)
                {
                    HintTextBox.Text = item.name + " " + item.ID;
                }
            }
            else
            {
                HintTextBox.Text = "";
            }
        }

        private void AddToLootList(object sender, RoutedEventArgs e)
        {
            if (item != null)
            {
                lootList.Add(item);
                ItemTextBox.Text = "";
                fillLootList();
            }
        }
        private void AddToFoodList(object sender, RoutedEventArgs e)
        {
            if (item != null)
            {
                foodList.Add(item);
                ItemTextBox.Text = "";
                fillFoodList();
            }
        }
        private void DeleteFromLootList(object sender, RoutedEventArgs e)
        {
            Item it = LootListBox.SelectedItem as Item;
            if (it != null)
            {
                lootList.Remove(it);
                fillLootList();
            }
        }
        private void DeleteFromFoodList(object sender, RoutedEventArgs e)
        {
            Item it = FoodListBox.SelectedItem as Item;
            if (it != null)
            {
                foodList.Remove(it);
                fillLootList();
            }
        }
    }
}
