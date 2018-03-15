using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TibiaHeleper.Modules;
using TibiaHeleper.Modules.Targeting;

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
        private Target target;

        private void Load(object sender, RoutedEventArgs e)
        {
            list = ModulesManager.targeting.getTargetListCopy();
            fillList();
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

        private void assignTarget()
        {
            try
            {
                target.name = Name.Text;
                target.maxHP = int.Parse(maxHP.Text);
                target.minHP = int.Parse(minHP.Text);
                target.action = Action.Text;
                target.maxDistance = int.Parse(maxDistance.Text);
            }
            catch (Exception)
            {
                ErrorLabel.Content = "Unacceptable value";
                Error.Visibility = Visibility.Visible;
            }

        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            list.Remove(target);
            fillList();
            clearAllTextBoxes();
        }

        private void clearAllTextBoxes()
        {
            Name.Text = "";
            maxHP.Text = "100";
            minHP.Text = "0";
            Action.Text = "";
            maxDistance.Text = "11";
        }

        private void setAllTextboxes()
        {
            Name.Text = target.name;
            maxHP.Text = target.maxHP.ToString();
            minHP.Text = target.minHP.ToString();
            Action.Text = target.action;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            bool wasWorking = ModulesManager.targeting.working;
            if (wasWorking) ModulesManager.TargetingDisable();

            ModulesManager.targeting.setTargetList(list);

            if (wasWorking) ModulesManager.TargetingEnable();

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
    }
}
