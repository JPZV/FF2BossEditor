using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace FF2BossEditor.Core.UserControls
{
    public class BossTabControl : UserControl, INotifyPropertyChanged
    {
        public BossTabControl()
        {
            DataContext = ActualBoss;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public bool IsTabReady
        {
            get => CheckTabReady(false);
        }

        public virtual bool CheckTabReady(bool ShowError) => true;
        public Classes.Boss ActualBoss = new Classes.Boss();

        public void UpdateBoss(Classes.Boss NeoBoss)
        {
            if (NeoBoss == null)
                return;
            if (ActualBoss == null)
            {
                ActualBoss = new Classes.Boss();
                DataContext = ActualBoss;
            }

            //Basic Info
            ActualBoss.Name = NeoBoss.Name;
            ActualBoss.Class = NeoBoss.Class;
            ActualBoss.Model = NeoBoss.Model;
            ActualBoss.RageDist = NeoBoss.RageDist;
            ActualBoss.RageDamage = NeoBoss.RageDamage;
            ActualBoss.Health = NeoBoss.Health;
            ActualBoss.Speed = NeoBoss.Speed;
            ActualBoss.Lives = NeoBoss.Lives;
            ActualBoss.BlockVoice = NeoBoss.BlockVoice;

            //Descriptions
            ActualBoss.Descriptions.Clear();
            foreach (Classes.Description desc in NeoBoss.Descriptions)
                ActualBoss.Descriptions.Add(desc);

            //Weapons
            ActualBoss.Weapons.Clear();
            foreach (Classes.Weapon wep in NeoBoss.Weapons)
                ActualBoss.Weapons.Add(wep);

            //Abilities
            ActualBoss.Abilities.Clear();
            foreach (Classes.Ability abi in NeoBoss.Abilities)
                ActualBoss.Abilities.Add(abi);

            //Sounds
            ActualBoss.Sounds = NeoBoss.Sounds;

            //Custom files
            ActualBoss.CustomFiles.Clear();
            foreach (string file in NeoBoss.CustomFiles)
                ActualBoss.CustomFiles.Add(file);
        }
    }

    public class PluginTabControl : UserControl
    {
        public PluginTabControl()
        {
            DataContext = ActualPlugin;
        }

        public virtual bool CheckTabReady(bool ShowError) => true;
        public Classes.Plugin ActualPlugin = new Classes.Plugin();

        public void UpdatePlugin(Classes.Plugin NeoPlugin)
        {
            if (NeoPlugin == null)
                return;
            if (ActualPlugin == null)
            {
                ActualPlugin = new Classes.Plugin();
                DataContext = ActualPlugin;
            }

            //Misc
            ActualPlugin.PluginName = NeoPlugin.PluginName;
            ActualPlugin.PluginAuthor = NeoPlugin.PluginAuthor;
            ActualPlugin.PluginPath = NeoPlugin.PluginPath;

            //Abilities
            ActualPlugin.AbilityTemplates = NeoPlugin.AbilityTemplates;
        }
    }

    public class TabColorFromBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool valueBool)
            {
                return new SolidColorBrush(valueBool ? Colors.Green : Colors.Red);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is SolidColorBrush valueBrush)
            {
                return valueBrush.Color == Colors.Green;
            }

            return value;
        }
    }
}
