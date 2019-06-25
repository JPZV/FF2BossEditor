using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FF2BossEditor.Core
{
    public class ExpandedTabControl : UserControl
    {
        public ExpandedTabControl()
        {
            DataContext = ActualBoss;
        }

        public virtual bool IsTabReady() => true;
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
}
