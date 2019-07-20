using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for SoundsView.xaml
    /// </summary>
    public partial class SoundsView : Core.UserControls.BossTabControl
    {
        public enum BrowseResult
        {
            Found,
            NotFound,
            Canceled
        }

        public class BrowseSoundResponse
        {
            public BrowseResult Result { get; set; } = BrowseResult.NotFound;
            public List<string> PathList { get; set; } = new List<string>();
        }
        public class BrowseMusicResponse
        {
            public BrowseResult Result { get; set; } = BrowseResult.NotFound;
            public List<Core.Classes.SoundPkg.MusicSound> MusicList { get; set; } = new List<Core.Classes.SoundPkg.MusicSound>();
        }

        public SoundsView()
        {
            InitializeComponent();
        }

        private ObservableCollection<Core.Classes.SoundPkg.Sound> GetCollectionByTag(string tag)
        {
            switch(tag)
            {
                default:
                    return null;
                case "startup":
                    return ActualBoss.Sounds.Startup;
                case "victory":
                    return ActualBoss.Sounds.Victory;
                case "death":
                    return ActualBoss.Sounds.Death;
                case "kill":
                    return ActualBoss.Sounds.KillPlayer;
                case "last":
                    return ActualBoss.Sounds.LastMan;
                case "spree":
                    return ActualBoss.Sounds.KillingSpree;
                case "catch":
                    return ActualBoss.Sounds.CatchPhrase;
                case "backstab":
                    return ActualBoss.Sounds.Backstab;
            }
        }

        private void DelSound_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null)
                return;
            if (sender is Button senderBtn)
            {
                if (senderBtn.Tag.ToString() == "music")
                {
                    if (((FrameworkElement)sender).DataContext is Core.Classes.SoundPkg.MusicSound sound)
                        ActualBoss.Sounds.Music.Remove(sound);
                }
                else if (senderBtn.Tag.ToString() == "ability")
                {
                    if (((FrameworkElement)sender).DataContext is Core.Classes.SoundPkg.AbilitySound sound)
                        ActualBoss.Sounds.Ability.Remove(sound);
                }
                else
                {
                    ObservableCollection<Core.Classes.SoundPkg.Sound> pkg = GetCollectionByTag(senderBtn.Tag.ToString());
                    if (pkg != null && ((FrameworkElement)sender).DataContext is Core.Classes.SoundPkg.Sound sound)
                        pkg.Remove(sound);
                }
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null)
                return;
            if(sender is Button senderBtn)
            {
                if(senderBtn.Tag.ToString() == "music")
                {
                    BrowseMusicResponse browseResp = BrowseMusic();
                    if (browseResp.Result == BrowseResult.Found)
                        foreach (Core.Classes.SoundPkg.MusicSound music in browseResp.MusicList)
                            ActualBoss.Sounds.Music.Add(music);
                    else if (browseResp.Result == BrowseResult.NotFound)
                        MessageBox.Show("The music must be located in a folder called sound (without 's').", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                } else if (senderBtn.Tag.ToString() == "ability")
                {
                    BrowseSoundResponse browseResp = BrowseSounds();
                    if (browseResp.Result == BrowseResult.Found)
                        foreach (string path in browseResp.PathList)
                            ActualBoss.Sounds.Ability.Add(new Core.Classes.SoundPkg.AbilitySound()
                            {
                                Path = path
                            });
                    else if (browseResp.Result == BrowseResult.NotFound)
                        MessageBox.Show("The sounds must be located in a folder called sound (without 's').", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                } else
                {
                    ObservableCollection<Core.Classes.SoundPkg.Sound> pkg = GetCollectionByTag(senderBtn.Tag.ToString());
                    if(pkg != null)
                    {
                        BrowseSoundResponse browseResp = BrowseSounds();
                        if (browseResp.Result == BrowseResult.Found)
                            foreach (string path in browseResp.PathList)
                                pkg.Add(new Core.Classes.SoundPkg.Sound()
                                {
                                    Path = path
                                });
                        else if(browseResp.Result == BrowseResult.NotFound)
                            MessageBox.Show("The sounds must be located in a folder called sound (without 's').", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private BrowseSoundResponse BrowseSounds()
        {
            BrowseSoundResponse resp = new BrowseSoundResponse();
            Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog()
            {
                DefaultExt = ".mp3",
                Filter = "MP3 File (*.mp3)|*.mp3|WAV File (*.wav)|*.wav|All files (*.*)|*.*",
                Multiselect = true
            };

            bool? openResult = openDialog.ShowDialog();
            if (openResult == true)
            {
                for(int i = 0; i < openDialog.FileNames.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(openDialog.FileNames[i]))
                        continue;
                    Match soundPathMatch = Regex.Match(openDialog.FileNames[i] + ":", @"\\sound\\(.*?):", RegexOptions.IgnoreCase);
                    if(soundPathMatch.Success)
                    {
                        resp.PathList.Add(string.Format("sound\\{0}", soundPathMatch.Groups[1].Value));
                        resp.Result = BrowseResult.Found;
                    }
                }
            } else
            {
                resp.Result = BrowseResult.Canceled;
            }
            return resp;
        }

        private BrowseMusicResponse BrowseMusic()
        {
            BrowseMusicResponse resp = new BrowseMusicResponse();
            Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog()
            {
                DefaultExt = ".mp3",
                Filter = "MP3 File (*.mp3)|*.mp3|WAV File (*.wav)|*.wav|All files (*.*)|*.*",
                Multiselect = true
            };

            bool? openResult = openDialog.ShowDialog();
            if (openResult == true)
            {
                for (int i = 0; i < openDialog.FileNames.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(openDialog.FileNames[i]))
                        continue;
                    Match soundPathMatch = Regex.Match(openDialog.FileNames[i] + ":", @"\\sound\\(.*?):", RegexOptions.IgnoreCase);
                    if (soundPathMatch.Success)
                    {
                        int musicDuration = 0;
                        if (openDialog.FileNames[i].EndsWith(".wav"))
                            musicDuration = GetWavFileDuration(openDialog.FileNames[i]);
                        else if(openDialog.FileNames[i].EndsWith(".mp3"))
                            musicDuration = GetMp3FileDuration(openDialog.FileNames[i]);

                        resp.MusicList.Add(new Core.Classes.SoundPkg.MusicSound() {
                            Path = string.Format("sound\\{0}", soundPathMatch.Groups[1].Value),
                            Length = musicDuration
                        });
                        resp.Result = BrowseResult.Found;
                    }
                }
            }
            else
            {
                resp.Result = BrowseResult.Canceled;
            }
            return resp;
        }

        private int GetMp3FileDuration(string fileName)
        {
            try
            {
                Mp3FileReader mfr = new Mp3FileReader(fileName);
                return (int)mfr.TotalTime.TotalSeconds;
            } catch
            {
                return 0;
            }
        }

        private int GetWavFileDuration(string fileName)
        {
            try
            {
                WaveFileReader wfr = new WaveFileReader(fileName);
                return (int)wfr.TotalTime.TotalSeconds;
            }
            catch
            {
                return 0;
            }
        }
    }
}
