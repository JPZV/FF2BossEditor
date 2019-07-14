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
using static FF2BossEditor.Core.Classes;

namespace FF2BossEditor.Windows
{
    /// <summary>
    /// Interaction logic for AbilityEditor.xaml
    /// </summary>
    public partial class AbilityEditor : Window
    {
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
                        Index = 3,
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
        }.OrderBy(t => t.PublicName).ToList();

        public readonly List<AbilityTemplate> FF2BatTemplates = new List<AbilityTemplate>()
        {
            #region Default Abilities
            new AbilityTemplate()
            {
                PublicName = "Uber",
                Name = "rage_uber",
                Plugin = "ffbat_defaults",
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
                Plugin = "ffbat_defaults",
                Arguments = new System.Collections.ObjectModel.ObservableCollection<Core.Classes.Ability.Argument>()
                {
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 1,
                        Value = "5.0",
                        Alias = "Initial duration of stun"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 2,
                        Value = "5.0",
                        Alias = "Distance of stun"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 3,
                        Value = "0x00c0",
                        Alias = "Stun Flags (Hex)"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 4,
                        Value = "0.0",
                        Alias = "Slowdown effect"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 5,
                        Value = "1",
                        Alias = "Play Stun sound to boss (1/0)"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 6,
                        Value = "yikes_fx",
                        Alias = "Particle effect on victim"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 7,
                        Value = "0",
                        Alias = "Attack on Ubers"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 8,
                        Value = "0",
                        Alias = "Can players of the same team be stunned (1/0 or -1 to use mp_friendlyfire)"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 9,
                        Value = "1",
                        Alias = "Remove parachutes on victims (1/0)"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 10,
                        Value = "0.0",
                        Alias = "Delay before stunning"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 11,
                        Value = "-1.0",
                        Alias = "Maximum duration of stun (-1.0 to copy arg1)"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 12,
                        Value = "0.0",
                        Alias = "Additional duration per player stunned"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 13,
                        Value = "-1.0",
                        Alias = "Duration for stunning the last player (-1.0 to copy arg1)"
                    },
                }
            },
            new AbilityTemplate()
            {
                PublicName = "Sentry Stun",
                Name = "rage_stunsg",
                Plugin = "ffbat_defaults",
                Arguments = new System.Collections.ObjectModel.ObservableCollection<Core.Classes.Ability.Argument>()
                {
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 1,
                        Value = "5.0",
                        Alias = "Duration of stun"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 2,
                        Value = "5.0",
                        Alias = "Distance of stun"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 3,
                        Value = "1.0",
                        Alias = "Building Health Multiplier"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 4,
                        Value = "1.0",
                        Alias = "Building Ammo Multiplier"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 5,
                        Value = "1.0",
                        Alias = "Building Rocket Multiplier"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 6,
                        Value = "yikes_fx",
                        Alias = "Particle effect on building"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 7,
                        Value = "1",
                        Alias = "Affected buildings"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 8,
                        Value = "-1",
                        Alias = "Friendly Fire (-1 to use mp_friendlyfire)"
                    }
                }
            },
            new AbilityTemplate()
            {
                PublicName = "Instant Teleport",
                Name = "rage_instant_teleport",
                Plugin = "ffbat_defaults",
                Arguments = new System.Collections.ObjectModel.ObservableCollection<Core.Classes.Ability.Argument>()
                {
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 1,
                        Value = "2.0",
                        Alias = "Self-stun duration"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 2,
                        Value = "0",
                        Alias = "Can teleport to the same team"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 3,
                        Value = "0x00C0",
                        Alias = "Stun flags (Hex)"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 4,
                        Value = "0.0",
                        Alias = "Slowdown effect (<= 1.0)"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 5,
                        Value = "1",
                        Alias = "Stun sound to player"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 6,
                        Value = "",
                        Alias = "Particle effect on boss"
                    }
                }
            },
            new AbilityTemplate()
            {
                PublicName = "Dissolve player killed",
                Name = "special_dissolve",
                Plugin = "ffbat_defaults",
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
                Plugin = "ffbat_defaults",
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
                Plugin = "ffbat_defaults",
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
                Plugin = "ffbat_defaults",
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
                Plugin = "ffbat_defaults",
                Arguments = new System.Collections.ObjectModel.ObservableCollection<Core.Classes.Ability.Argument>()
                {
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 1,
                        Value = "1",
                        Alias = "Clones use a custom model (1/0)"
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
                        Value = "",
                        Alias = "Clones' Health fórmula (n: Players alive)"
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
                Plugin = "ffbat_defaults",
                Arguments = new System.Collections.ObjectModel.ObservableCollection<Core.Classes.Ability.Argument>()
                {
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 1,
                        Value = "5.0",
                        Alias = "Duration"
                    }
                }
            },
            new AbilityTemplate()
            {
                PublicName = "Christian Brutal Sniper's Bow",
                Name = "rage_cbs_bowrage",
                Plugin = "ffbat_defaults",
                Arguments = new System.Collections.ObjectModel.ObservableCollection<Core.Classes.Ability.Argument>()
                {
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 1,
                        Value = "2 ; 3.0 ; 6 ; 0.5 ; 37 ; 0.0 ; 214 ; 333 ; 280 ; 19",
                        Alias = "Weapon attibutes"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 2,
                        Value = "9",
                        Alias = "Max Weapon Ammo"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 3,
                        Value = "1.0",
                        Alias = "Weapon ammo per player alive"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 4,
                        Value = "1",
                        Alias = "Weapon clip"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 5,
                        Value = "tf_weapon_compound_bow",
                        Alias = "Weapon Class"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 6,
                        Value = "1005",
                        Alias = "Weapon Index"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 7,
                        Value = "101",
                        Alias = "Weapon Level"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 8,
                        Value = "5",
                        Alias = "Weapon quality"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 9,
                        Value = "0",
                        Alias = "Set as active weapon (1/0)"
                    }
                }
            },
            new AbilityTemplate()
            {
                PublicName = "Explosive dance",
                Name = "rage_explosive_dance",
                Plugin = "ffbat_defaults",
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
                Plugin = "ffbat_defaults",
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
                        Alias = "Duration"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 2,
                        Value = "0.1",
                        Alias = "Timescale"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 3,
                        Value = "0.2",
                        Alias = "Time between attacks"
                    }
                }
            },
            new AbilityTemplate()
            {
                PublicName = "Democharge",
                Name = "special_democharge",
                Plugin = "ffbat_defaults",
                Arguments = new System.Collections.ObjectModel.ObservableCollection<Core.Classes.Ability.Argument>()
                {
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 0,
                        Value = "2",
                        Alias = "Slot"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 1,
                        Value = "0.25",
                        Alias = "Duration"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 2,
                        Value = "0.0",
                        Alias = "Cooldown"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 3,
                        Value = "0.0",
                        Alias = "Delay"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 4,
                        Value = "0.4",
                        Alias = "Rage cost"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 5,
                        Value = "10.0",
                        Alias = "Minimum Rage to use arg4"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 6,
                        Value = "90.0",
                        Alias = "Maximum Rage to use arg4"
                    }
                }
            },
            new AbilityTemplate()
            {
                PublicName = "Drop prop on killed player",
                Name = "special_dropprop",
                Plugin = "ffbat_defaults",
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
                        Index = 3,
                        Value = "0",
                        Alias = "Remove player's ragdoll (1/0)"
                    }
                }
            },
            new AbilityTemplate()
            {
                PublicName = "Multiple melee",
                Name = "special_cbs_multimelee",
                Plugin = "ffbat_defaults",
                Arguments = new System.Collections.ObjectModel.ObservableCollection<Core.Classes.Ability.Argument>()
                {
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 0,
                        Value = "68 ; 2 ; 2 ; 3.1 ; 275 ; 1 ; 214 ; 34",
                        Alias = "Weapon Attributes"
                    },
                }
            },
            #endregion
            #region Rage Overlay
            new AbilityTemplate()
            {
                PublicName = "Overlay",
                Name = "rage_overlay",
                Plugin = "ffbat_defaults",
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
            },
            #endregion
            #region FF2Bat Abilities
            new AbilityTemplate()
            {
                PublicName = "New Weapon",
                Name = "rage_new_weapon",
                Plugin = "ffbat_defaults",
                Arguments = new System.Collections.ObjectModel.ObservableCollection<Core.Classes.Ability.Argument>()
                {
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 1,
                        Value = "tf_weapon_compound_bow",
                        Alias = "Weapon Class"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 2,
                        Value = "1005",
                        Alias = "Weapon Index"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 3,
                        Value = "2 ; 3.0",
                        Alias = "Weapon attributes"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 4,
                        Value = "0",
                        Alias = "Weapon Slot"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 5,
                        Value = "9",
                        Alias = "Weapon Ammo"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 6,
                        Value = "1",
                        Alias = "Set as active weapon (1/0)"
                    },
                    new Core.Classes.Ability.Argument()
                    {
                        Index = 7,
                        Value = "1",
                        Alias = "Weapon clip"
                    }
                }
            },
            #endregion
        }.OrderBy(t => t.PublicName).ToList();

        public readonly List<PluginsPkg.AbilityPlugin> AbilitiesPlugins = new List<PluginsPkg.AbilityPlugin>(App.Plugins.AbilityPlugins);

        public AbilityEditor(Ability _Ability)
        {
            InitializeComponent();
            Ability = _Ability;
            DataContext = Ability;
            FF2OfficialMI.DataContext = FF2OfficialTemplates;
            FF2BatMI.DataContext = FF2BatTemplates;
            PluginsMI.DataContext = AbilitiesPlugins;
            PluginsMI.IsEnabled = AbilitiesPlugins.Count > 0;
        }
        
        public Ability Ability = null;

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
            if (((FrameworkElement)sender).DataContext is Ability.Argument arg)
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
            senderElement.Visibility = senderElement.DataContext is Ability.Argument ? Visibility.Visible : Visibility.Collapsed;
        }

        private void FF2TemplateMI_Click(object sender, RoutedEventArgs e)
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
