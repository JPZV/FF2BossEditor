using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FF2BossEditor.Views.RootFrame
{
    /// <summary>
    /// Interaction logic for AbilitiesView.xaml
    /// </summary>
    public partial class AbilitiesView : Core.ExpandedTabControl
    {
        public AbilitiesView()
        {
            InitializeComponent();
        }

        private void AbilitiesView_Loaded(object sender, RoutedEventArgs e)
        {
            ActualBoss.Abilities.CollectionChanged -= Abilities_CollectionChanged;
            ActualBoss.Abilities.CollectionChanged += Abilities_CollectionChanged;
        }

        private void Abilities_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (object item in e.NewItems)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged += delegate { OnPropertyChanged("IsTabReady"); };
                    if (item is Core.Classes.Ability abiItem)
                        abiItem.Arguments.CollectionChanged += Abilities_CollectionChanged;
                }
            }
            OnPropertyChanged("IsTabReady");
        }

        public override bool CheckTabReady(bool ShowError)
        {
            foreach(Core.Classes.Ability ability in ActualBoss.Abilities)
            {
                int dupArgs = 0;
                if (string.IsNullOrWhiteSpace(ability.Name))
                {
                    if (ShowError)
                        MessageBox.Show("Please insert the ability's name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                else if (string.IsNullOrWhiteSpace(ability.Plugin))
                {
                    if (ShowError)
                        MessageBox.Show(string.Format("Please insert the ability's name.\nAbility: {0}", ability.Name), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                else if(ability.Arguments.GroupBy(t => t.Index).Where(g => (dupArgs = g.Count()) > 1).Count() > 0)
                {
                    if (ShowError)
                        MessageBox.Show(string.Format("There are {0} arguments with the same index.\nAbility: {1}", dupArgs, ability.Name), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            return true;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            Windows.AbilityEditor abilityEditor = new Windows.AbilityEditor(new Core.Classes.Ability());
            if (abilityEditor.ShowDialog() == true)
            {
                ActualBoss.Abilities.Add(abilityEditor.Ability);
            }
        }

        private void EditAbility_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null)
                return;
            if (((FrameworkElement)sender).DataContext is Core.Classes.Ability ability)
            {
                Windows.AbilityEditor abilityEditor = new Windows.AbilityEditor(new Core.Classes.Ability()
                {
                    Name = ability.Name,
                    Plugin = ability.Plugin,
                    Arguments = ability.Arguments
                });
                if (abilityEditor.ShowDialog() == true)
                {
                    ability.Name = abilityEditor.Ability.Name;
                    ability.Plugin = abilityEditor.Ability.Plugin;
                    ability.Arguments = abilityEditor.Ability.Arguments;
                }
            }
        }

        private void DelAbility_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null)
                return;
            if (((FrameworkElement)sender).DataContext is Core.Classes.Ability ability)
            {
                ActualBoss.Abilities.Remove(ability);
            }
        }
    }
}
