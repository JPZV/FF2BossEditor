using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace FF2BossEditor.Core
{
    public class ExpandedTabControl : UserControl, INotifyPropertyChanged
    {
        public ExpandedTabControl()
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
            ActualBoss.Descriptions = NeoBoss.Descriptions;

            //Weapons
            ActualBoss.Weapons = NeoBoss.Weapons;

            //Abilities
            ActualBoss.Abilities = NeoBoss.Abilities;

            //Sounds
            ActualBoss.Sounds = NeoBoss.Sounds;

            //Custom files
            ActualBoss.CustomFiles = NeoBoss.CustomFiles;
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
