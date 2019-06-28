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
    /// Interaction logic for AbilityEditor.xaml
    /// </summary>
    public partial class AbilityEditor : Window
    {
        public class AbilityTemplate : Core.Classes.Ability
        {
            private string _PublicName = "";
            public string PublicName
            {
                get => _PublicName;
                set
                {
                    _PublicName = value;
                    OnPropertyChanged("PublicName");
                }
            }
        }

        public readonly List<AbilityTemplate> FF2OfficialTemplates = new List<AbilityTemplate>()
        {
            #region Default Abilities
            new AbilityTemplate()
            {
                PublicName = "Uber",
                Name = "rage_uber",
                Plugin = "default_abilities",
                Arguments = new System.Collections.ObjectModel.ObservableCollection<Core.Classes.Ability.Argument>()
                {
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 1,
                        Value = "5.0",
                        Alias = "Duration of uber"
                    }
                }
            },
            new AbilityTemplate()
            {
                PublicName = "Stun",
                Name = "rage_stun",
                Plugin = "default_abilities",
                Arguments = new System.Collections.ObjectModel.ObservableCollection<Core.Classes.Ability.Argument>()
                {
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 1,
                        Value = "5.0",
                        Alias = "Duration of stun"
                    }
                }
            },
            new AbilityTemplate()
            {
                PublicName = "Sentry Stun",
                Name = "rage_stunsg",
                Plugin = "default_abilities",
                Arguments = new System.Collections.ObjectModel.ObservableCollection<Core.Classes.Ability.Argument>()
                {
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 1,
                        Value = "7.0",
                        Alias = "Duration of stun"
                    }
                }
            },
            new AbilityTemplate()
            {
                PublicName = "Instant Teleport",
                Name = "rage_instant_teleport",
                Plugin = "default_abilities"
            },
            new AbilityTemplate()
            {
                PublicName = "Brave Jump",
                Name = "charge_bravejump",
                Plugin = "default_abilities",
                Arguments = new System.Collections.ObjectModel.ObservableCollection<Core.Classes.Ability.Argument>()
                {
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 0,
                        Value = "1",
                        Alias = "Slot"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 1,
                        Value = "1.5",
                        Alias = "Seconds to full charge"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 2,
                        Value = "5.0",
                        Alias = "Seconds to reload"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 3,
                        Value = "1.0",
                        Alias = "Force multiplier"
                    }
                }
            },
            new AbilityTemplate()
            {
                PublicName = "Teleport",
                Name = "charge_teleport",
                Plugin = "default_abilities",
                Arguments = new System.Collections.ObjectModel.ObservableCollection<Core.Classes.Ability.Argument>()
                {
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 0,
                        Value = "1",
                        Alias = "Slot"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 1,
                        Value = "1.5",
                        Alias = "Seconds to full charge"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 2,
                        Value = "5.0",
                        Alias = "Seconds to reload"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 3,
                        Value = "",
                        Alias = "Teleporter effect"
                    }
                }
            },
            new AbilityTemplate()
            {
                PublicName = "Weightdown",
                Name = "charge_weightdown",
                Plugin = "default_abilities",
                Arguments = new System.Collections.ObjectModel.ObservableCollection<Core.Classes.Ability.Argument>()
                {
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 0,
                        Value = "3",
                        Alias = "Slot"
                    }
                }
            },
            new AbilityTemplate()
            {
                PublicName = "Dissolve player killed",
                Name = "special_dissolve",
                Plugin = "default_abilities",
                Arguments = new System.Collections.ObjectModel.ObservableCollection<Core.Classes.Ability.Argument>()
                {
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 0,
                        Value = "4",
                        Alias = "Slot"
                    }
                }
            },
            #endregion
            #region Easter Abilities
            new AbilityTemplate()
            {
                PublicName = "Projectile replace",
                Name = "model_projectile_replace",
                Plugin = "easter_abilities",
                Arguments = new System.Collections.ObjectModel.ObservableCollection<Core.Classes.Ability.Argument>()
                {
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 1,
                        Value = "tf_projectile_pipe",
                        Alias = "Projectile to replace"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 2,
                        Value = "models/player/saxton_hale/w_easteregg.mdl",
                        Alias = "New model for the replacement"
                    }
                }
            },
            new AbilityTemplate()
            {
                PublicName = "Many objects on player death",
                Name = "spawn_many_objects_on_kill",
                Plugin = "easter_abilities",
                Arguments = new System.Collections.ObjectModel.ObservableCollection<Core.Classes.Ability.Argument>()
                {
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 1,
                        Value = "tf_ammo_pack",
                        Alias = "Object to spawn"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 2,
                        Value = "models/player/saxton_hale/w_easteregg.mdl",
                        Alias = "Model for the object"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 3,
                        Value = "1",
                        Alias = "Model's Skin"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 4,
                        Value = "5",
                        Alias = "Number of objects"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 5,
                        Value = "30",
                        Alias = "Distance from the player"
                    }
                }
            },
            new AbilityTemplate()
            {
                PublicName = "Many objects on boss death",
                Name = "spawn_many_objects_on_death",
                Plugin = "easter_abilities",
                Arguments = new System.Collections.ObjectModel.ObservableCollection<Core.Classes.Ability.Argument>()
                {
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 1,
                        Value = "tf_ammo_pack",
                        Alias = "Object to spawn"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 2,
                        Value = "models/player/saxton_hale/w_easteregg.mdl",
                        Alias = "Model for the object"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 3,
                        Value = "1",
                        Alias = "Model's Skin"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 4,
                        Value = "5",
                        Alias = "Number of objects"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 5,
                        Value = "30",
                        Alias = "Distance from the boss"
                    }
                }
            },
            #endregion
            #region FF2 1st Set Abilities
            new AbilityTemplate()
            {
                PublicName = "Spawn Clones",
                Name = "rage_cloneattack",
                Plugin = "ff2_1st_set_abilities",
                Arguments = new System.Collections.ObjectModel.ObservableCollection<Core.Classes.Ability.Argument>()
                {
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 1,
                        Value = "1",
                        Alias = "Clones looks like the boss (1/0)"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 2,
                        Value = "1",
                        Alias = "Clones with weapons (1/0)"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 3,
                        Value = "",
                        Alias = "Clones' model path"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 4,
                        Value = "",
                        Alias = "Clones' class number"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 5,
                        Value = "0",
                        Alias = "Clones' respawn ratio"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 6,
                        Value = "tf_weapon_bottle",
                        Alias = "Clones' Weapon Class"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 7,
                        Value = "191",
                        Alias = "Clones' Weapon Index"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 8,
                        Value = "68 ; -1",
                        Alias = "Clones' Weapon Attributes"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 9,
                        Value = "0",
                        Alias = "Clones' Weapon Ammo"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 10,
                        Value = "0",
                        Alias = "Clones' Weapon Clip"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 11,
                        Value = "0",
                        Alias = "Clones' Health"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 12,
                        Value = "0",
                        Alias = "Slay clones on boss death (1/0)"
                    }
                }
            },
            new AbilityTemplate()
            {
                PublicName = "Demopan's Trade Spam",
                Name = "rage_tradespam",
                Plugin = "ff2_1st_set_abilities"
            },
            new AbilityTemplate()
            {
                PublicName = "Christian Brutal Sniper's Bow",
                Name = "rage_cbs_bowrage",
                Plugin = "ff2_1st_set_abilities"
            },
            new AbilityTemplate()
            {
                PublicName = "Explosive dance",
                Name = "rage_explosive_dance",
                Plugin = "ff2_1st_set_abilities",
                Arguments = new System.Collections.ObjectModel.ObservableCollection<Core.Classes.Ability.Argument>()
                {
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 1,
                        Value = "",
                        Alias = "Sound"
                    }
                }
            },
            new AbilityTemplate()
            {
                PublicName = "Slo-mo attack",
                Name = "rage_matrix_attack",
                Plugin = "ff2_1st_set_abilities",
                Arguments = new System.Collections.ObjectModel.ObservableCollection<Core.Classes.Ability.Argument>()
                {
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 0,
                        Value = "-1",
                        Alias = "Slot"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 1,
                        Value = "2",
                        Alias = "Duration + 1"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 2,
                        Value = "0.1",
                        Alias = "Timescale"
                    }
                }
            },
            new AbilityTemplate()
            {
                PublicName = "Democharge",
                Name = "special_democharge",
                Plugin = "ff2_1st_set_abilities",
                Arguments = new System.Collections.ObjectModel.ObservableCollection<Core.Classes.Ability.Argument>()
                {
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 0,
                        Value = "2",
                        Alias = "Slot"
                    }
                }
            },
            new AbilityTemplate()
            {
                PublicName = "Drop prop on killed player",
                Name = "special_dropprop",
                Plugin = "ff2_1st_set_abilities",
                Arguments = new System.Collections.ObjectModel.ObservableCollection<Core.Classes.Ability.Argument>()
                {
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 0,
                        Value = "4",
                        Alias = "Slot"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 1,
                        Value = "",
                        Alias = "Model"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 2,
                        Value = "5",
                        Alias = "Drop's Lifespan"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 2,
                        Value = "0",
                        Alias = "Remove player's ragdoll (1/0)"
                    }
                }
            },
            new AbilityTemplate()
            {
                PublicName = "Multiple melee",
                Name = "special_cbs_multimelee",
                Plugin = "ff2_1st_set_abilities"
            },
            #endregion
            #region Rage Overlay
            new AbilityTemplate()
            {
                PublicName = "Overlay",
                Name = "rage_overlay",
                Plugin = "rage_overlay",
                Arguments = new System.Collections.ObjectModel.ObservableCollection<Core.Classes.Ability.Argument>()
                {
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 1,
                        Value = "",
                        Alias = "Path relative to 'materials' folder"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 2,
                        Value = "6",
                        Alias = "Duration"
                    }
                }
            }
            #endregion
        };

        //TODO: Add Templates
        public AbilityEditor(Core.Classes.Ability _Ability)
        {
            InitializeComponent();
            Ability = _Ability;
            DataContext = Ability;
            FF2OfficialMI.DataContext = FF2OfficialTemplates;
        }

        public Core.Classes.Ability Ability = null;

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Ability.Name))
            {
                MessageBox.Show("Please insert the ability's name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            } else if (string.IsNullOrWhiteSpace(Ability.Plugin))
            {
                MessageBox.Show("Please insert the ability's plugin.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            } else if (Ability.Plugin.EndsWith(".ff2"))
            {
                MessageBox.Show("The ability's plugin name shouldn't ends with the .ff2 extension.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            } else if (Ability.Arguments.GroupBy(t => t.Index).Where(c => c.Count() > 1).Any())
            {
                MessageBox.Show("There are two arguments with the same index.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DialogResult = true;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void DelArg_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null)
                return;
            if (((FrameworkElement)sender).DataContext is Core.Classes.Ability.Argument arg)
            {
                Ability.Arguments.Remove(arg);
                //This is needed when deleting the new row
                ArgsDataGrid.CanUserAddRows = false;
                ArgsDataGrid.CanUserAddRows = true;
            }
        }

        private void DelArg_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender == null)
                return;

            FrameworkElement senderElement = ((FrameworkElement)sender);
            senderElement.Visibility = senderElement.DataContext is Core.Classes.Ability.Argument ? Visibility.Visible : Visibility.Collapsed;
        }

        private void FF2OfficialMI_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null)
                return;
            if (((FrameworkElement)e.OriginalSource).DataContext is AbilityTemplate template)
            {
                Ability.Name = template.Name;
                Ability.Plugin = template.Plugin;
                Ability.Arguments = template.Arguments;
            }
        }
    }
}
