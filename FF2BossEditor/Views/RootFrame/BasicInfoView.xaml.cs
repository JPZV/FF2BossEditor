using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for BasicInfoView.xaml
    /// </summary>
    public partial class BasicInfoView : Core.UserControls.BossTabControl
    {
        public BasicInfoView()
        {
            InitializeComponent();
        }

        private void BasicInfoView_Loaded(object sender, RoutedEventArgs e)
        {
            ActualBoss.PropertyChanged -= ActualBoss_PropertyChanged;
            ActualBoss.PropertyChanged += ActualBoss_PropertyChanged;
        }

        private void ActualBoss_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged("IsTabReady");
        }

        public override bool CheckTabReady(bool ShowError)
        {
            if(string.IsNullOrWhiteSpace(ActualBoss.Name))
            {
                if(ShowError)
                    MessageBox.Show("Please insert the boss' name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            } else if(ActualBoss.Class <= 0)
            {
                if (ShowError)
                    MessageBox.Show("Please select the boss' class.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            } else if (string.IsNullOrWhiteSpace(ActualBoss.Model))
            {
                if (ShowError)
                    MessageBox.Show("Please browse for the boss' model.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            } else if (string.IsNullOrWhiteSpace(ActualBoss.RageDamage))
            {
                if (ShowError)
                    MessageBox.Show("Please insert the boss' Rage Damage.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            } else if (string.IsNullOrWhiteSpace(ActualBoss.Health))
            {
                if (ShowError)
                    MessageBox.Show("Please insert the boss' health.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void BrowseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ActualBoss == null)
                ActualBoss = new Core.Classes.Boss();
            
            Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog()
            {
                DefaultExt = ".mdl",
                Filter = "Model File (*.mdl)|*.mdl"
            };

            bool? openResult = openDialog.ShowDialog();
            if (openResult == true)
            {
                Match modelPathMatch = Regex.Match(openDialog.FileName + ":", @"\\models\\(.*?):", RegexOptions.IgnoreCase);
                if (modelPathMatch.Success)
                    ActualBoss.Model = string.Format("models\\{0}", modelPathMatch.Groups[1].Value);
                else
                    MessageBox.Show("The model must be located in a folder called models.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Formula_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender == null || e == null || e.Text == null)
                return;

            e.Handled = !Core.CommonFunctions.IsStringInFormat(e.Text, "0123456789.n+-*/^");
        }
    }

    public class ClassIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is int valueInt)
            {
                return valueInt - 1;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is int valueInt)
            {
                return valueInt + 1;
            }

            return value;
        }
    }
}
