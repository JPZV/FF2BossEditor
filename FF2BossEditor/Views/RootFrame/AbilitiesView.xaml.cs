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
