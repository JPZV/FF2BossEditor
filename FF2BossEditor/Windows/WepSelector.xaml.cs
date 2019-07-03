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
    /// Interaction logic for WepSelector.xaml
    /// </summary>
    public partial class WepSelector : Window
    {
        public WepSelector(int _SelWepIndex)
        {
            InitializeComponent();
            if (App.WeaponsTemplates != null)
                SelectedTemplate = App.WeaponsTemplates.Find(t => t.Index == _SelWepIndex);
            if (SelectedTemplate == null)
                SelectedTemplate = new Core.Classes.WeaponTemplate() { Index = -1 };
            DataContext = SelectedTemplate;
        }

        private DataGrid[] CharactersDG;

        public Core.Classes.WeaponTemplate SelectedTemplate = new Core.Classes.WeaponTemplate()
        {
            Index = -1
        };

        private void WepSelector_Loaded(object sender, RoutedEventArgs e)
        {
            CharactersDG = new DataGrid[] { ScoutWepsDG, SniperWepsDG, SoldierWepsDG, DemoWepsDG, MedicWepsDG, HeavyWepsDG, PyroWepsDG, SpyWepsDG, EngiWepsDG };

            if(App.WeaponsTemplates != null && App.WeaponsTemplates.Count > 0)
            {
                for(int i = 0; i < CharactersDG.Length; i++)
                    CharactersDG[i].DataContext = App.WeaponsTemplates.FindAll(t => t.Characters.Any(c => c == i + 1));
            } else
            {
                MessageBox.Show("Weapons DataBase not found. Please download it and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                DialogResult = false;
            }
        }

        private void SelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTemplate.Index == -1)
            {
                MessageBox.Show("Please select an weapon.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DialogResult = true;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void CharacterDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == null || e.AddedItems == null || e.AddedItems.Count == 0 || !(e.AddedItems[0] is Core.Classes.WeaponTemplate wepTemp))
                return;

            SelectedTemplate.Class = wepTemp.Class;
            SelectedTemplate.Index = wepTemp.Index;
            SelectedTemplate.Name = wepTemp.Name;

            for(int i = 0; i < CharactersDG.Length; i++)
            {
                if (sender != CharactersDG[i])
                    CharactersDG[i].SelectedIndex = -1;
            }
        }
    }
}
