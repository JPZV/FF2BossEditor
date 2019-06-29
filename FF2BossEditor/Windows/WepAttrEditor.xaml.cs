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
    /// Interaction logic for WepAttrEditor.xaml
    /// </summary>
    public partial class WepAttrEditor : Window
    {
        public WepAttrEditor(Core.Classes.Weapon.Attribute _Attribute)
        {
            InitializeComponent();
            Attribute = _Attribute;
            DataContext = Attribute;
        }

        private void WepAttrEditor_Loaded(object sender, RoutedEventArgs e)
        {
            AutoAttrDataGrid.DataContext = App.WeaponsAttributes;
            if (!string.IsNullOrWhiteSpace(Attribute.Name))
            {
                AutoAttrDataGrid.SelectedIndex = App.WeaponsAttributes.FindIndex(t => t.Name == Attribute.Name);
                AutoAttrDataGrid.ScrollIntoView(AutoAttrDataGrid.SelectedItem);
            }
        }

        public Core.Classes.Weapon.Attribute Attribute = null;

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if(WepAttrTabs.SelectedIndex == 0) //Pre-existing
            {
                if (AutoAttrDataGrid.SelectedIndex < 0)
                {
                    MessageBox.Show("Please select an Attribute.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Attribute.ID = App.WeaponsAttributes[AutoAttrDataGrid.SelectedIndex].ID;
                Attribute.Name = App.WeaponsAttributes[AutoAttrDataGrid.SelectedIndex].Name;

                DialogResult = true;
            } else //Custom
            {
                if(string.IsNullOrWhiteSpace(Attribute.Name))
                {
                    MessageBox.Show("Please insert an alias for the Attribute.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                } else if (Attribute.ID <= 0)
                {
                    MessageBox.Show("Please insert the attribute's ID (it must be greater than 0).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                DialogResult = true;
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
