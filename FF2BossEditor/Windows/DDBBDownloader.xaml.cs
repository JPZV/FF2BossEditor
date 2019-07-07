using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    /// Interaction logic for DDBBDownloader.xaml
    /// </summary>
    public partial class DDBBDownloader : Window
    {
        public DDBBDownloader()
        {
            InitializeComponent();
        }

        private async void DDBBDownloader_Loaded(object sender, RoutedEventArgs e)
        {
            WebClient client = new WebClient();
            DownloadBar.Value = 0;
            try
            {
                System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "/Data/");
                await client.DownloadFileTaskAsync("https://raw.githubusercontent.com/JPZV/FF2BossEditor/master/FF2BossEditor/Data/attributes.json", AppDomain.CurrentDomain.BaseDirectory + "/Data/attributes.json");
                DownloadBar.Value = 45;
                await client.DownloadFileTaskAsync("https://raw.githubusercontent.com/JPZV/FF2BossEditor/master/FF2BossEditor/Data/weapons.json", AppDomain.CurrentDomain.BaseDirectory + "/Data/weapons.json");
                DownloadBar.Value = 90;

                await App.ReloadWeaponsAttributes();
                DownloadBar.Value = 95;
                await App.ReloadWeaponsTemplates();
                DownloadBar.Value = 100;

                MessageBox.Show("The DataBase was updated successfully", "Ok", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
            } catch (Exception ex)
            {
                if(MessageBox.Show("An error ocurred while downloading the DataBase.\nShow the exception?", "Error", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
                    MessageBox.Show(ex.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                DialogResult = false;
            }
        }
    }
}
