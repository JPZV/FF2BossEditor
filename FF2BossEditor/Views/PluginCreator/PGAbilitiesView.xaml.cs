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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FF2BossEditor.Views.PluginCreator
{
    /// <summary>
    /// Interaction logic for PGAbilitiesView.xaml
    /// </summary>
    public partial class PGAbilitiesView : Core.UserControls.PluginTabControl
    {
        public PGAbilitiesView()
        {
            InitializeComponent();
        }

        public override bool CheckTabReady(bool ShowError)
        {
            foreach (Core.Classes.Ability ability in ActualPlugin.AbilityTemplates)
            {
                int dupArgs = 0;
                int emptyArgs = 0;
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
                else if (ability.Arguments.GroupBy(t => t.Index).Where(g => (dupArgs = g.Count()) > 1).Count() > 0)
                {
                    if (ShowError)
                        MessageBox.Show(string.Format("There are {0} arguments with the same index.\nAbility: {1}", dupArgs, ability.Name), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                else if ((emptyArgs = ability.Arguments.Count(t => string.IsNullOrWhiteSpace(t.Alias))) > 0)
                {
                    if (ShowError)
                        MessageBox.Show(string.Format("There are {0} arguments without an alias.\nAbility: {1}", emptyArgs, ability.Name), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            return true;
        }

        private void AddAbiBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ActualPlugin == null)
                ActualPlugin = new Core.Classes.Plugin();
            ActualPlugin.AbilityTemplates.Add(new Core.Classes.AbilityTemplate());
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null)
                return;
            if (((FrameworkElement)sender).DataContext is Core.Classes.AbilityTemplate abi)
            {
                if (ActualPlugin == null)
                    ActualPlugin = new Core.Classes.Plugin();
                ActualPlugin.AbilityTemplates.Remove(abi);
            }
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null)
                return;
            if (((FrameworkElement)sender).DataContext is Core.Classes.Ability ability)
            {
                Windows.AbilityEditor abilityEditor = new Windows.AbilityEditor(ability.Clone());
                if (abilityEditor.ShowDialog() == true)
                {
                    ability.Name = abilityEditor.Ability.Name;
                    ability.Plugin = abilityEditor.Ability.Plugin;
                    ability.Arguments = abilityEditor.Ability.Arguments;
                }
            }
        }
    }
}
