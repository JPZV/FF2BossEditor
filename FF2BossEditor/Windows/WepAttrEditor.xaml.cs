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

        public Core.Classes.Weapon.Attribute Attribute = null;

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if(WepAttrTabs.SelectedIndex == 0) //Pre-existing
            {
                //TODO: Automatic mode
                DialogResult = false; //TODO: Change to TRUE
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
