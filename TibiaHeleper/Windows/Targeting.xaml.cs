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
            target = new Target();
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
            if (list.IndexOf(target)>-1 && list.IndexOf(target) < list.Count()-1 )
            {
                int index = list.IndexOf(target);
                list.RemoveAt(index);
                list.Insert(index + 1, target);
            }
            fillList();
        }

        private void Up(object sender, RoutedEventArgs e)
        {
            if (list.IndexOf(target) > 0)
            {
                int index = list.IndexOf(target);
                list.RemoveAt(index);
                list.Insert(index - 1, target);
            }
            fillList();
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            target = new Target();
            try
            {
                assignTarget();
                list.Add(target);
                fillList();
                target = new Target();
                clearAllTextBoxes();
            }
            catch (Exception)
            {
                ErrorLabel.Content = "Unacceptable value";
                Error.Visibility = Visibility.Visible;
            }
        }

        private void assignTarget()
        {
            target.name = Name.Text;
            target.maxHP = int.Parse(maxHP.Text);
            target.minHP = int.Parse(minHP.Text);
            target.action = Action.Text;
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
            maxHP.Text = "";
            minHP.Text = "";
            Action.Text = "";
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
            ModulesManager.targeting.setTargetList(list);
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
