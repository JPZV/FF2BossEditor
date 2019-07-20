using System;
using System.Collections.Generic;
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
    /// Interaction logic for CustomFilesView.xaml
    /// </summary>
    public partial class CustomFilesView : Core.UserControls.BossTabControl
    {
        private readonly string[] allowedFolders = { "materials", "media", "models", "particles", "resource", "sound" };

        public CustomFilesView()
        {
            InitializeComponent();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ActualBoss == null)
                ActualBoss = new Core.Classes.Boss();

            Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog()
            {
                DefaultExt = "*.*",
                Filter = "All Files (*.*)|*.*",
                Multiselect = true
            };

            bool? openResult = openDialog.ShowDialog();
            if (openResult == true)
            {
                for (int file = 0; file < openDialog.FileNames.Length; file++)
                {
                    for (int folder = 0; folder < allowedFolders.Length; folder++)
                    {
                        Match modelPathMatch = Regex.Match(openDialog.FileNames[file] + ":", string.Format(@"\\{0}\\(.*?):", allowedFolders[folder]), RegexOptions.IgnoreCase);
                        if (modelPathMatch.Success)
                        {
                            ActualBoss.CustomFiles.Add(string.Format("{0}\\{1}", allowedFolders[folder], modelPathMatch.Groups[1].Value));
                            break;
                        }
                    }
                }
            }
        }

        private void DelSound_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null)
                return;
            if (((FrameworkElement)sender).DataContext is string path)
            {
                ActualBoss.CustomFiles.Remove(path);
            }
        }
    }
}
