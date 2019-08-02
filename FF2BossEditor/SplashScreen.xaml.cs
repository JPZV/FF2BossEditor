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

namespace FF2BossEditor
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        public SplashScreen()
        {
            InitializeComponent();
            DataContext = this;
        }

        public Core.Classes.ObservableString Status { get; set; } = new Core.Classes.ObservableString();

        private async void SplashScreen_Loaded(object sender, RoutedEventArgs e)
        {
            Status.Value = "Loading weapons attributes...";
            await App.ReloadWeaponsAttributes();
            Status.Value = "Loading weapons templates...";
            await App.ReloadWeaponsTemplates();
            Status.Value = "Loading plugins...";
            await App.ReloadPlugins();
            Status.Value = "Ready!";
            await Task.Delay(1500);
            RootFrame root = new RootFrame();
            Application.Current.MainWindow = root;
            Close();
            root.Show();
        }
    }
}
