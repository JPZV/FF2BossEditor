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
    /// Interaction logic for PluginCreator.xaml
    /// </summary>
    public partial class PluginCreator : Window
    {
        public PluginCreator()
        {
            InitializeComponent();
            DataContext = ActualPlugin;
        }

        private Core.Classes.Plugin _ActualPlugin = new Core.Classes.Plugin();
        private Core.Classes.Plugin PrevPlugin = null;
        private string ActualPluginPath = "";

        private Core.Classes.Plugin ActualPlugin
        {
            get => _ActualPlugin;
            set
            {
                _ActualPlugin = value;
                DataContext = ActualPlugin;
            }
        }

        private Core.Classes.Plugin MergePluginFromViews()
        {
            Core.Classes.Plugin neoPlugin = new Core.Classes.Plugin();
            if(ActualPlugin != null)
            {
                neoPlugin.PluginName = ActualPlugin.PluginName;
                neoPlugin.PluginAuthor = ActualPlugin.PluginAuthor;
            }

            if (Abilities.ActualPlugin != null)
            {
                neoPlugin.AbilityTemplates = Abilities.ActualPlugin.AbilityTemplates;
            }

            return neoPlugin;
        }

        private void UpdatePluginInViews()
        {
            Abilities.UpdatePlugin(ActualPlugin);
        }

        private void PluginCreator_Loaded(object sender, RoutedEventArgs e)
        {
            UpdatePluginInViews();
        }

        private async void NewMI_Click(object sender, RoutedEventArgs e)
        {
            if (!await CheckIfPluginHasChanges())
                return;

            ActualPlugin = new Core.Classes.Plugin();
            ActualPluginPath = "";
            PrevPlugin = null;
            UpdatePluginInViews();
        }

        private async void OpenMI_Click(object sender, RoutedEventArgs e)
        {
            if (!await CheckIfPluginHasChanges())
                return;

            Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog()
            {
                DefaultExt = ".ffbeplugin",
                Filter = "Plugin File (*.ffbeplugin)|*.ffbeplugin"
            };

            bool? openResult = openDialog.ShowDialog();
            if (openResult == true)
            {
                Core.Classes.Plugin tmpPlugin = await Core.StorageCore<Core.Classes.Plugin>.GenericGetObject(openDialog.FileName);
                if (tmpPlugin != null)
                {
                    ActualPlugin = tmpPlugin;
                    ActualPluginPath = openDialog.FileName;
                    PrevPlugin = tmpPlugin.Clone();
                    UpdatePluginInViews();
                }
            }
        }

        private async void SaveMI_Click(object sender, RoutedEventArgs e)
        {
            ActualPlugin = MergePluginFromViews();
            if (!await SavePlugin(ActualPlugin))
                return;
            PrevPlugin = ActualPlugin.Clone();
        }

        private async void PluginCreator_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!await CheckIfPluginHasChanges())
                e.Cancel = true;
        }

        private async Task<bool> SavePlugin(Core.Classes.Plugin Plugin)
        {
            if (!Abilities.CheckTabReady(true))
                return false;

            if (Plugin.AbilityTemplates.Count == 0)
            {
                MessageBox.Show("A plugin cannot be empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(ActualPluginPath))
                return await Core.StorageCore<Core.Classes.Plugin>.GenericSaveObject(Plugin, ActualPluginPath);

            Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog()
            {
                DefaultExt = ".ffbeplugin",
                Filter = "Plugin File (*.ffbeplugin)|*.ffbeplugin"
            };

            bool? saveResult = saveDialog.ShowDialog();
            if (saveResult == true)
            {
                if (await Core.StorageCore<Core.Classes.Plugin>.GenericSaveObject(Plugin, saveDialog.FileName))
                {
                    ActualPluginPath = saveDialog.FileName;
                    return true;
                }
            }
            return false;
        }

        private async Task<bool> CheckIfPluginHasChanges()
        {
            Core.Classes.Plugin mergedPlugin = MergePluginFromViews();

            if ((PrevPlugin == null || !mergedPlugin.IsClassEqual(PrevPlugin)) && !mergedPlugin.IsClassEmpty())
            {
                if (MessageBox.Show("Do you want to save the current Plugin?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    return await SavePlugin(mergedPlugin);
            }
            return true;
        }
    }
}
