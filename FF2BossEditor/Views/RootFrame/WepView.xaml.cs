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
    /// Interaction logic for WepView.xaml
    /// </summary>
    public partial class WepView : Core.UserControls.BossTabControl
    {
        public WepView()
        {
            InitializeComponent();
        }

        private void WepView_Loaded(object sender, RoutedEventArgs e)
        {
            ActualBoss.Weapons.CollectionChanged -= Weapons_CollectionChanged;
            ActualBoss.Weapons.CollectionChanged += Weapons_CollectionChanged;
        }

        private void Weapons_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (object item in e.NewItems)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged += delegate { OnPropertyChanged("IsTabReady"); };
                    if(item is Core.Classes.Weapon weaponItem)
                        weaponItem.Attributes.CollectionChanged += Weapons_CollectionChanged;
                }
            }
            OnPropertyChanged("IsTabReady");
        }

        public override bool CheckTabReady(bool ShowError)
        {
            if (ActualBoss.Weapons.Count == 0)
            {
                if (ShowError)
                    MessageBox.Show("Please add at least one weapon.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            for(int i = 0; i < ActualBoss.Weapons.Count; i++)
            {
                if (ActualBoss.Weapons[i].Index < 0)
                {
                    if (ShowError)
                        MessageBox.Show(string.Format("The weapon's Index must be greater than or equal to 0 (Zero).\nWeapon {0}", i + 1), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                else if (string.IsNullOrWhiteSpace(ActualBoss.Weapons[i].Class))
                {
                    if (ShowError)
                        MessageBox.Show(string.Format("Please insert the weapon's class.\nWeapon {0}", i + 1), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                foreach(Core.Classes.Weapon.Attribute attr in ActualBoss.Weapons[i].Attributes)
                {
                    if (attr.ID <= 0)
                    {
                        if (ShowError)
                            MessageBox.Show(string.Format("The attribute's Index must be greater than 0 (Zero).\nWeapon {0}", i + 1), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
            }

            return true;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ActualBoss == null)
                ActualBoss = new Core.Classes.Boss();
            if (ActualBoss.Weapons.Count < 3)
                ActualBoss.Weapons.Add(new Core.Classes.Weapon());
            AddBtn.IsEnabled = ActualBoss.Weapons.Count < 3;
        }

        private void AddAttrBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null)
                return;
            if (((FrameworkElement)sender).DataContext is Core.Classes.Weapon wep)
            {
                Windows.WepAttrEditor attrEditor = new Windows.WepAttrEditor(new Core.Classes.Weapon.Attribute(wep));
                if (attrEditor.ShowDialog() == true)
                {
                    wep.Attributes.Add(attrEditor.Attribute);
                }
            }
        }

        private void EditAttr_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null)
                return;
            if (((FrameworkElement)sender).DataContext is Core.Classes.Weapon.Attribute attr)
            {
                Windows.WepAttrEditor attrEditor = new Windows.WepAttrEditor(attr.Clone(attr.Parent));
                if (attrEditor.ShowDialog() == true)
                {
                    attr.Arg = attrEditor.Attribute.Arg;
                    attr.ID = attrEditor.Attribute.ID;
                    attr.Name = attrEditor.Attribute.Name;
                }
            }
        }

        private void DelAttr_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null)
                return;
            if (((FrameworkElement)sender).DataContext is Core.Classes.Weapon.Attribute attr && attr.Parent != null)
            {
                attr.Parent.Attributes.Remove(attr);
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null)
                return;
            if (((FrameworkElement)sender).DataContext is Core.Classes.Weapon wep)
            {
                if (ActualBoss == null)
                    ActualBoss = new Core.Classes.Boss();
                ActualBoss.Weapons.Remove(wep);
                AddBtn.IsEnabled = ActualBoss.Weapons.Count < 3;
            }
        }

        private void DataGridNoWheel_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender == null)
                return;

            if (sender is DataGrid dataGrid)
            {
                var sv = dataGrid.Template.FindName("DG_ScrollViewer", dataGrid);
                if (sv is ScrollViewer scrollViewer)
                {
                    scrollViewer.MouseWheel += (s, arg) =>
                    {
                        arg.Handled = true;
                    };
                }
            }
        }

        private void SelectWeaponBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null)
                return;
            if (((FrameworkElement)sender).DataContext is Core.Classes.Weapon wep)
            {
                Windows.WepSelector wepSelector = new Windows.WepSelector(wep.Index);
                if (wepSelector.ShowDialog() == true)
                {
                    wep.Class = wepSelector.SelectedTemplate.Class;
                    wep.Index = wepSelector.SelectedTemplate.Index;
                }
            }
        }
    }

    public class WeaponIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value is int valueInt)
            {
                return string.Format("Weapon {0}", valueInt + 1);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
