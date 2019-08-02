using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
    /// Interaction logic for DescView.xaml
    /// </summary>
    public partial class DescView : Core.UserControls.BossTabControl
    {
        public DescView()
        {
            InitializeComponent();
        }

        private void DescView_Loaded(object sender, RoutedEventArgs e)
        {
            ActualBoss.Descriptions.CollectionChanged -= Descriptions_CollectionChanged;
            ActualBoss.Descriptions.CollectionChanged += Descriptions_CollectionChanged;
        }

        private void Descriptions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (object item in e.NewItems)
                    ((INotifyPropertyChanged)item).PropertyChanged += delegate { OnPropertyChanged("IsTabReady"); };
            OnPropertyChanged("IsTabReady");
        }

        public override bool CheckTabReady(bool ShowError)
        {
            if (ActualBoss.Descriptions.Count == 0)
            {
                if (ShowError)
                    MessageBox.Show("Please insert a boss description.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (ActualBoss.Descriptions.Any(t => string.IsNullOrWhiteSpace(t.Text)))
            {
                if (ShowError)
                    MessageBox.Show("There cannot be any description without text.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if (ActualBoss.Descriptions.Any(t => string.IsNullOrWhiteSpace(t.Lang)))
            {
                if (ShowError)
                    MessageBox.Show("There cannot be any description without a language.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if(ActualBoss.Descriptions.GroupBy(t => t.Lang).Where(g => g.Count() > 1).Count() > 0)
            {
                if (ShowError)
                    MessageBox.Show("There cannot be two descriptions with the same language.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ActualBoss == null)
                ActualBoss = new Core.Classes.Boss();

            ActualBoss.Descriptions.Add(new Core.Classes.Description());
        }

        private void DescText_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (sender == null || e == null)
                return;

            if (e.Key == Key.Enter)
            {
                TextBox senderBox = sender as TextBox;
                if (senderBox == null)
                    return;

                int count = senderBox.Text.Count(t => t == '\n');

                e.Handled = count > 5;
            }
        }

        private void LangBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender == null)
                return;

            if (sender is ComboBox comboBox)
            {
                if (comboBox.Template.FindName("PART_EditableTextBox", comboBox) is TextBox textBox)
                {
                    textBox.MaxLength = 2;
                }
            }
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null)
                return;
            if (((FrameworkElement)sender).DataContext is Core.Classes.Description desc)
            {
                ActualBoss.Descriptions.Remove(desc);
            }
        }
    }
}
