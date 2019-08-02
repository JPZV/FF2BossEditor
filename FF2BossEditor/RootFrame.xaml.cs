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
    /// Interaction logic for RootFrame.xaml
    /// </summary>
    public partial class RootFrame : Window
    {
        public RootFrame()
        {
            InitializeComponent();
        }

        private Core.Classes.Boss ActualBoss = new Core.Classes.Boss();
        private Core.Classes.Boss PrevBoss = null;
        private string ActualBossPath = "";

        private void RootFrame_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateBossInViews();
        }

        private void UpdateBossInViews()
        {
            BasicInfo.UpdateBoss(ActualBoss);
            Desc.UpdateBoss(ActualBoss);
            Weps.UpdateBoss(ActualBoss);
            Abilities.UpdateBoss(ActualBoss);
            Sounds.UpdateBoss(ActualBoss);
            CustomFiles.UpdateBoss(ActualBoss);
        }

        private Core.Classes.Boss MergeBossesFromViews()
        {
            Core.Classes.Boss neoBoss = new Core.Classes.Boss();
            if(BasicInfo.ActualBoss != null)
            {
                neoBoss.Name = BasicInfo.ActualBoss.Name;
                neoBoss.Class = BasicInfo.ActualBoss.Class;
                neoBoss.Model = BasicInfo.ActualBoss.Model;
                neoBoss.RageDist = BasicInfo.ActualBoss.RageDist;
                neoBoss.RageDamage = BasicInfo.ActualBoss.RageDamage;
                neoBoss.Health = BasicInfo.ActualBoss.Health;
                neoBoss.Speed = BasicInfo.ActualBoss.Speed;
                neoBoss.Lives = BasicInfo.ActualBoss.Lives;
                neoBoss.BlockVoice = BasicInfo.ActualBoss.BlockVoice;
            }
            if(Desc.ActualBoss != null)
            {
                neoBoss.Descriptions = Desc.ActualBoss.Descriptions;
            }
            if(Weps.ActualBoss != null)
            {
                neoBoss.Weapons = Weps.ActualBoss.Weapons;
            }
            if(Abilities.ActualBoss != null)
            {
                neoBoss.Abilities = Abilities.ActualBoss.Abilities;
            }
            if(Sounds.ActualBoss != null)
            {
                neoBoss.Sounds = Sounds.ActualBoss.Sounds;
            }
            if(CustomFiles.ActualBoss != null)
            {
                neoBoss.CustomFiles = CustomFiles.ActualBoss.CustomFiles;
            }

            return neoBoss;
        }

        private async void ExportCFGBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!BasicInfo.CheckTabReady(true) ||
                !Desc.CheckTabReady(true) ||
                !Weps.CheckTabReady(true) ||
                !Abilities.CheckTabReady(true))
                return;

            ActualBoss = MergeBossesFromViews();
            await Core.CFGCore.ExportBoss(ActualBoss);
        }

        private async void NewMI_Click(object sender, RoutedEventArgs e)
        {
            if (!await CheckIfBossHasChanges())
                return;

            ActualBoss = new Core.Classes.Boss();
            ActualBossPath = "";
            PrevBoss = null;
            UpdateBossInViews();
        }

        private async void OpenMI_Click(object sender, RoutedEventArgs e)
        {
            if (!await CheckIfBossHasChanges())
                return;

            Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog()
            {
                DefaultExt = ".ff2boss",
                Filter = "FF2 Boss File (*.ff2boss)|*.ff2boss"
            };

            bool? openResult = openDialog.ShowDialog();
            if (openResult == true)
            {
                Core.Classes.Boss tmpBoss = await Core.StorageCore<Core.Classes.Boss>.GenericGetObject(openDialog.FileName);
                if (tmpBoss != null)
                {
                    ActualBoss = tmpBoss;
                    ActualBossPath = openDialog.FileName;
                    PrevBoss = tmpBoss.Clone();
                    UpdateBossInViews();
                }
            }
        }

        private async void SaveMI_Click(object sender, RoutedEventArgs e)
        {
            ActualBoss = MergeBossesFromViews();
            if (!await SaveBoss(ActualBoss))
                return;
            PrevBoss = ActualBoss.Clone();
        }

        private async void ImportCFGMI_Click(object sender, RoutedEventArgs e)
        {
            Core.Classes.Boss tmpBoss = await Core.CFGCore.ImportBoss();

            if (tmpBoss != null)
            {
                ActualBoss = tmpBoss;
                UpdateBossInViews();
            }
        }

        private void DownloadDDBBMI_Click(object sender, RoutedEventArgs e)
        {
            Windows.DDBBDownloader downloader = new Windows.DDBBDownloader();
            downloader.ShowDialog();
        }

        private void PluginsManagerMI_Click(object sender, RoutedEventArgs e)
        {
            Windows.PluginsManager manager = new Windows.PluginsManager();
            manager.ShowDialog();
        }

        private void PluginsCreatorMI_Click(object sender, RoutedEventArgs e)
        {
            Windows.PluginCreator creator = new Windows.PluginCreator();
            creator.Show();
        }

        private async Task<bool> SaveBoss(Core.Classes.Boss Boss)
        {
            if (!string.IsNullOrWhiteSpace(ActualBossPath))
                return await Core.StorageCore<Core.Classes.Boss>.GenericSaveObject(Boss, ActualBossPath);

            Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog()
            {
                DefaultExt = ".ff2boss",
                Filter = "FF2 Boss File (*.ff2boss)|*.ff2boss"
            };

            bool? saveResult = saveDialog.ShowDialog();
            if (saveResult == true)
            {
                if(await Core.StorageCore<Core.Classes.Boss>.GenericSaveObject(Boss, saveDialog.FileName))
                {
                    ActualBossPath = saveDialog.FileName;
                    return true;
                }
            }
            return false;
        }

        private async void RootFrame_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            await CheckIfBossHasChanges();
        }

        private async Task<bool> CheckIfBossHasChanges()
        {
            Core.Classes.Boss mergedBoss = MergeBossesFromViews();

            if ((PrevBoss == null || !mergedBoss.IsClassEqual(PrevBoss)) && !mergedBoss.IsClassEmpty())
            {
                if (MessageBox.Show("Do you want to save the current Boss Configuration?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    return await SaveBoss(mergedBoss);
            }
            return true;
        }
    }
}