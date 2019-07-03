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
    /// Interaction logic for WepView.xaml
    /// </summary>
    public partial class WepView : Core.ExpandedTabControl
    {
        public WepView()
        {
            InitializeComponent();
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
                Windows.WepAttrEditor attrEditor = new Windows.WepAttrEditor(new Core.Classes.Weapon.Attribute(attr.Parent)
                {
                    Arg = attr.Arg,
                    ID = attr.ID,
                    Name = attr.Name
                });
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
