using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace FF2BossEditor.Windows
{
    /// <summary>
    /// Interaction logic for PluginsManager.xaml
    /// </summary>
    public partial class PluginsManager : Window
    {
        public PluginsManager()
        {
            InitializeComponent();
            DataContext = App.Plugins;
        }

        private void DelPlugin_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null)
                return;
            if (((FrameworkElement)sender).DataContext is Core.Classes.Plugin plugin)
            {
                try
                {
                    if (File.Exists(plugin.PluginPath))
                        File.Delete(plugin.PluginPath);
                    RefreshBtn_Click(sender, e);
                } catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            }
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            PluginCreator creator = new PluginCreator();
            creator.Show();
        }

        private async void ImportBtn_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Create Busy Screen
            Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog()
            {
                DefaultExt = ".ffbeplugin",
                Filter = "Plugin File (*.ffbeplugin)|*.ffbeplugin"
            };

            bool? openResult = openDialog.ShowDialog();
            if (openResult == true)
            {
                using (StreamReader sr = new StreamReader(openDialog.FileName))
                {
                    string json = await sr.ReadToEndAsync();
                    JObject jObj = JsonConvert.DeserializeObject<JObject>(json);
                    if (jObj["PluginName"] == null)
                    {
                        MessageBox.Show("This plugin seems to be corrupted.\nMissing Plugin's Name (\"PluginName\").", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (jObj["AbilityTemplates"] == null || !(jObj["AbilityTemplates"] is JArray abilities))
                    {
                        MessageBox.Show("This plugin seems to be corrupted.\nMissing Abilities Templates (\"AbilityTemplates\").", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                string pluginPath = AppDomain.CurrentDomain.BaseDirectory + "/Plugins/" + openDialog.SafeFileName;

                if (File.Exists(pluginPath) && MessageBox.Show("This plugin already exists in the Editor. Do you want to overwrite?.", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                    return;

                File.Copy(openDialog.FileName, pluginPath , true);
                RefreshBtn_Click(this, e);
            }
        }

        private async void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Create Busy Screen
            await App.ReloadPlugins();
        }
    }

    public class IsListEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is IEnumerable<object> enumerableValue)
            {
                return enumerableValue.Count() > 0;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
