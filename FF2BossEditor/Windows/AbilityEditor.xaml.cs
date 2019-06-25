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

namespace FF2BossEditor.Windows
{
    /// <summary>
    /// Interaction logic for AbilityEditor.xaml
    /// </summary>
    public partial class AbilityEditor : Window
    {
        //TODO: Add Templates
        public AbilityEditor(Core.Classes.Ability _Ability)
        {
            InitializeComponent();
            Ability = _Ability;
            DataContext = Ability;
        }

        public Core.Classes.Ability Ability = null;

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Ability.Name))
            {
                MessageBox.Show("Please insert the ability's name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            } else if (string.IsNullOrWhiteSpace(Ability.Plugin))
            {
                MessageBox.Show("Please insert the ability's plugin.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            } else if (Ability.Plugin.EndsWith(".ff2"))
            {
                MessageBox.Show("The ability's plugin name shouldn't ends with the .ff2 extension.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            } else if (Ability.Arguments.GroupBy(t => t.Index).Where(c => c.Count() > 1).Any())
            {
                MessageBox.Show("There are two arguments with the same index.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DialogResult = true;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void DelArg_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null)
                return;
            if (((FrameworkElement)sender).DataContext is Core.Classes.Ability.Argument arg)
            {
                Ability.Arguments.Remove(arg);
                //This is needed when deleting the new row
                ArgsDataGrid.CanUserAddRows = false;
                ArgsDataGrid.CanUserAddRows = true;
            }
        }

        private void DelArg_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender == null)
                return;

            FrameworkElement senderElement = ((FrameworkElement)sender);
            senderElement.Visibility = senderElement.DataContext is Core.Classes.Ability.Argument ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
