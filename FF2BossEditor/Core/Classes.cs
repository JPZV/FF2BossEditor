using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF2BossEditor.Core
{
    public static class Classes
    {
        public interface IClassFunctions
        {
            bool IsClassEmpty();
            bool IsClassEqual(IClassFunctions Obj); //I'm using this to avoid replacing the == operator
        }

        #region App
        public class AbilityTemplate : Ability
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

        public class ObservableString : INotifyPropertyChanged
        {
            private string _Value = "";
            public string Value
            {
                get => _Value;
                set
                {
                    _Value = value;
                    OnPropertyChanged("Value");
                }
            }

            protected void OnPropertyChanged(string name)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }

        public class PluginsPkg
        {
            public class AbilityPlugin
            {
                public string PluginName { get; set; } = "";
                public List<AbilityTemplate> AbilityTemplates { get; set; } = new List<AbilityTemplate>();
            }

            public List<AbilityPlugin> AbilityPlugins { get; set; } = new List<AbilityPlugin>();

            public void Clear()
            {
                AbilityPlugins.Clear();
            }

            public PluginsPkg Clone()
            {
                PluginsPkg neoPkg = new PluginsPkg
                {
                    AbilityPlugins = new List<AbilityPlugin>(AbilityPlugins)
                };
                return neoPkg;
            }
        }
        #endregion

        #region Boss
        public class Ability : INotifyPropertyChanged, IClassFunctions
        {
            public class Argument : INotifyPropertyChanged, IClassFunctions
            {
                private int _Index = 1;
                private string _Value = "";
                private string _Alias = "";

                public int Index
                {
                    get => _Index;
                    set
                    {
                        _Index = value;
                        OnPropertyChanged("Index");
                    }
                }
                public string Value
                {
                    get => _Value;
                    set
                    {
                        _Value = value;
                        OnPropertyChanged("Value");
                    }
                }
                public string Alias
                {
                    get => _Alias;
                    set
                    {
                        _Alias = value;
                        OnPropertyChanged("Alias");
                    }
                }
                
                public event PropertyChangedEventHandler PropertyChanged;

                protected void OnPropertyChanged(string name)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
                }

                public Argument Clone()
                {
                    Argument clone = new Argument()
                    {
                        Index = Index,
                        Value = Value,
                        Alias = Alias
                    };
                    return clone;
                }

                public bool IsClassEmpty()
                {
                    return string.IsNullOrWhiteSpace(Value) && string.IsNullOrWhiteSpace(Alias);
                }

                public bool IsClassEqual(IClassFunctions Obj)
                {
                    if(Obj is Argument arg)
                    {
                        return Index == arg.Index &&
                               Value == arg.Value &&
                               Alias == arg.Alias;
                    }
                    return false;
                }
            }

            private string _Name = "";
            private string _Plugin = "";
            private ObservableCollection<Argument> _Arguments = new ObservableCollection<Argument>();

            public string Name
            {
                get => _Name;
                set
                {
                    _Name = value;
                    OnPropertyChanged("Name");
                }
            }
            public string Plugin
            {
                get => _Plugin;
                set
                {
                    _Plugin = value;
                    OnPropertyChanged("Plugin");
                }
            }
            public ObservableCollection<Argument> Arguments
            {
                get => _Arguments;
                set
                {
                    _Arguments = value;
                    OnPropertyChanged("Arguments");
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged(string name)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }

            public Ability Clone()
            {
                Ability clone = new Ability()
                {
                    Name = Name,
                    Plugin = Plugin
                };
                foreach (Argument arg in Arguments)
                    clone.Arguments.Add(arg.Clone());
                return clone;
            }

            public bool IsClassEmpty()
            {
                foreach(IClassFunctions classF in Arguments)
                    if (!classF.IsClassEmpty())
                        return false;

                return string.IsNullOrWhiteSpace(Name) && string.IsNullOrWhiteSpace(Plugin) && Arguments.Count == 0;
            }

            public bool IsClassEqual(IClassFunctions Obj)
            {
                if (Obj is Ability abi)
                {
                    if (Name != abi.Name || Plugin != abi.Plugin)
                        return false;

                    if (!CheckClassListEquality(abi.Arguments, Arguments))
                        return false;

                    return true;
                }
                return false;
            }
        }

        public class Boss : INotifyPropertyChanged, IClassFunctions
        {
            private const string DEFAULTRAGEDAMAGE = "1900";
            private const string DEFAULTHEALTH = "(((760+n)*n)^1.04)";

            private string _Name = "";
            private int _Class = 1;
            private string _Model = "";
            private int _RageDist = 800;
            private string _RageDamage = DEFAULTRAGEDAMAGE;
            private string _Health = DEFAULTHEALTH;
            private int _Speed = 340;
            private int _Lives = 1;
            private bool _BlockVoice = false;
            private ObservableCollection<Description> _Descriptions = new ObservableCollection<Description>();
            private ObservableCollection<Weapon> _Weapons = new ObservableCollection<Weapon>();
            private ObservableCollection<Ability> _Abilities = new ObservableCollection<Ability>();
            private SoundPkg _Sounds = new SoundPkg();
            private ObservableCollection<string> _CustomFiles = new ObservableCollection<string>();

            public string Name
            {
                get => _Name;
                set
                {
                    _Name = value;
                    OnPropertyChanged("Name");
                }
            }
            public int Class
            {
                get => _Class;
                set
                {
                    _Class = value;
                    OnPropertyChanged("Class");
                }
            }
            public string Model
            {
                get => _Model;
                set
                {
                    _Model = value;
                    OnPropertyChanged("Model");
                }
            }
            public int RageDist
            {
                get => _RageDist;
                set
                {
                    _RageDist = value;
                    OnPropertyChanged("RageDist");
                }
            }
            public string RageDamage
            {
                get => _RageDamage;
                set
                {
                    _RageDamage = value;
                    OnPropertyChanged("RageDamage");
                }
            }
            public string Health
            {
                get => _Health;
                set
                {
                    _Health = value;
                    OnPropertyChanged("Health");
                }
            }
            public int Speed
            {
                get => _Speed;
                set
                {
                    _Speed = value;
                    OnPropertyChanged("Speed");
                }
            }
            public int Lives
            {
                get => _Lives;
                set
                {
                    _Lives = value;
                    OnPropertyChanged("Lives");
                }
            }
            public bool BlockVoice
            {
                get => _BlockVoice;
                set
                {
                    _BlockVoice = value;
                    OnPropertyChanged("BlockVoice");
                }
            }
            public ObservableCollection<Description> Descriptions
            {
                get => _Descriptions;
                set
                {
                    _Descriptions = value;
                    OnPropertyChanged("Descriptions");
                }
            }
            public ObservableCollection<Weapon> Weapons
            {
                get => _Weapons;
                set
                {
                    _Weapons = value;
                    OnPropertyChanged("Weapons");
                }
            }
            public ObservableCollection<Ability> Abilities
            {
                get => _Abilities;
                set
                {
                    _Abilities = value;
                    OnPropertyChanged("Abilities");
                }
            }
            public SoundPkg Sounds
            {
                get => _Sounds;
                set
                {
                    _Sounds = value;
                    OnPropertyChanged("Sounds");
                }
            }
            public ObservableCollection<string> CustomFiles
            {
                get => _CustomFiles;
                set
                {
                    _CustomFiles = value;
                    OnPropertyChanged("CustomFiles");
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged(string name)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }

            public Boss Clone()
            {
                Boss clone = new Boss()
                {
                    Name = Name,
                    Class = Class,
                    Model = Model,
                    RageDist = RageDist,
                    RageDamage = RageDamage,
                    Health = Health,
                    Speed = Speed,
                    Lives = Lives,
                    BlockVoice = BlockVoice
                };

                foreach (Description desc in Descriptions)
                    clone.Descriptions.Add(desc.Clone());

                foreach (Weapon wep in Weapons)
                    clone.Weapons.Add(wep.Clone());

                foreach (Ability abi in Abilities)
                    clone.Abilities.Add(abi.Clone());

                clone.Sounds = Sounds.Clone();
                clone.CustomFiles = new ObservableCollection<string>(CustomFiles);

                return clone;
            }

            public bool IsClassEmpty()
            {
                foreach (IClassFunctions classF in Descriptions)
                    if (!classF.IsClassEmpty())
                        return false;

                foreach (IClassFunctions classF in Weapons)
                    if (!classF.IsClassEmpty())
                        return false;

                foreach (IClassFunctions classF in Abilities)
                    if (!classF.IsClassEmpty())
                        return false;

                foreach (string customFile in CustomFiles)
                    if (!string.IsNullOrWhiteSpace(customFile))
                        return false;

                return string.IsNullOrWhiteSpace(Name) &&
                       string.IsNullOrWhiteSpace(Model) &&
                       (string.IsNullOrWhiteSpace(RageDamage) || RageDamage == DEFAULTRAGEDAMAGE) &&
                       (string.IsNullOrWhiteSpace(Health) || Health == DEFAULTHEALTH) &&
                       Descriptions.Count == 0 &&
                       Weapons.Count == 0 &&
                       Abilities.Count == 0 &&
                       Sounds.IsClassEmpty() && 
                       CustomFiles.Count == 0;
            }

            public bool IsClassEqual(IClassFunctions Obj)
            {
                if (Obj is Boss boss)
                {
                    if (Name != boss.Name ||
                        Class != boss.Class ||
                        Model != boss.Model ||
                        RageDist != boss.RageDist ||
                        RageDamage != boss.RageDamage ||
                        Health != boss.Health ||
                        Speed != boss.Speed ||
                        Lives != boss.Lives ||
                        BlockVoice != boss.BlockVoice)
                        return false;

                    if (!CheckClassListEquality(boss.Descriptions, Descriptions))
                        return false;
                    if (!CheckClassListEquality(boss.Weapons, Weapons))
                        return false;
                    if (!CheckClassListEquality(boss.Abilities, Abilities))
                        return false;

                    if (!Sounds.IsClassEqual(boss.Sounds))
                        return false;

                    if (CustomFiles.Count != boss.CustomFiles.Count)
                        return false;
                    for (int i = 0; i < CustomFiles.Count; i++)
                        if(CustomFiles[i] != boss.CustomFiles[i])
                            return false;

                    return true;
                }
                return false;
            }
        }

        public class Description : INotifyPropertyChanged, IClassFunctions
        {
            private string _Lang = "";
            private string _Text = "";

            public string Lang
            {
                get => _Lang;
                set
                {
                    _Lang = value;
                    OnPropertyChanged("Lang");
                }
            }
            public string Text
            {
                get => _Text;
                set
                {
                    _Text = value;
                    OnPropertyChanged("Text");
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged(string name)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }

            public Description Clone()
            {
                Description clone = new Description()
                {
                    Lang = Lang,
                    Text = Text
                };
                return clone;
            }

            public bool IsClassEmpty()
            {
                return string.IsNullOrWhiteSpace(Lang) && string.IsNullOrWhiteSpace(Text);
            }

            public bool IsClassEqual(IClassFunctions Obj)
            {
                if (Obj is Description desc)
                {
                    return Lang == desc.Lang && Text == desc.Text;
                }
                return false;
            }
        }

        public class SoundPkg : INotifyPropertyChanged, IClassFunctions
        {
            public class Sound : INotifyPropertyChanged, IClassFunctions
            {
                private string _Path = "";

                public string Path
                {
                    get => _Path;
                    set
                    {
                        _Path = value;
                        OnPropertyChanged("Path");
                    }
                }

                public event PropertyChangedEventHandler PropertyChanged;

                protected void OnPropertyChanged(string name)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
                }

                public Sound Clone()
                {
                    Sound clone = new Sound()
                    {
                        Path = Path
                    };
                    return clone;
                }

                public bool IsClassEmpty()
                {
                    return string.IsNullOrWhiteSpace(Path);
                }

                public virtual bool IsClassEqual(IClassFunctions Obj)
                {
                    if (Obj is Sound sound)
                    {
                        return Path == sound.Path;
                    }
                    return false;
                }
            }
            public class AbilitySound : Sound
            {
                private int _Slot = 0;

                public int Slot
                {
                    get => _Slot;
                    set
                    {
                        _Slot = value;
                        OnPropertyChanged("Slot");
                    }
                }

                public new AbilitySound Clone()
                {
                    AbilitySound clone = new AbilitySound()
                    {
                        Path = Path,
                        Slot = Slot
                    };
                    return clone;
                }

                public override bool IsClassEqual(IClassFunctions Obj)
                {
                    if (Obj is AbilitySound sound)
                    {
                        return Path == sound.Path && Slot == sound.Slot;
                    }
                    return false;
                }
            }
            public class MusicSound : Sound
            {
                private int _Length = 0;

                public int Length
                {
                    get => _Length;
                    set
                    {
                        _Length = value;
                        OnPropertyChanged("Length");
                    }
                }

                public new MusicSound Clone()
                {
                    MusicSound clone = new MusicSound()
                    {
                        Path = Path,
                        Length = Length
                    };
                    return clone;
                }

                public override bool IsClassEqual(IClassFunctions Obj)
                {
                    if (Obj is MusicSound sound)
                    {
                        return Path == sound.Path && Length == sound.Length;
                    }
                    return false;
                }
            }

            private ObservableCollection<Sound> _Startup = new ObservableCollection<Sound>();
            private ObservableCollection<Sound> _Victory = new ObservableCollection<Sound>();
            private ObservableCollection<Sound> _Death = new ObservableCollection<Sound>();
            private ObservableCollection<Sound> _KillPlayer = new ObservableCollection<Sound>();
            private ObservableCollection<Sound> _LastMan = new ObservableCollection<Sound>();
            private ObservableCollection<Sound> _KillingSpree = new ObservableCollection<Sound>();
            private ObservableCollection<AbilitySound> _Ability = new ObservableCollection<AbilitySound>();
            private ObservableCollection<Sound> _CatchPhrase = new ObservableCollection<Sound>();
            private ObservableCollection<MusicSound> _Music = new ObservableCollection<MusicSound>();
            private ObservableCollection<Sound> _Backstab = new ObservableCollection<Sound>();

            public ObservableCollection<Sound> Startup
            {
                get => _Startup;
                set
                {
                    _Startup = value;
                    OnPropertyChanged("Startup");
                }
            }
            public ObservableCollection<Sound> Victory
            {
                get => _Victory;
                set
                {
                    _Victory = value;
                    OnPropertyChanged("Victory");
                }
            }
            public ObservableCollection<Sound> Death
            {
                get => _Death;
                set
                {
                    _Death = value;
                    OnPropertyChanged("Death");
                }
            }
            public ObservableCollection<Sound> KillPlayer
            {
                get => _KillPlayer;
                set
                {
                    _KillPlayer = value;
                    OnPropertyChanged("KillPlayer");
                }
            }
            public ObservableCollection<Sound> LastMan
            {
                get => _LastMan;
                set
                {
                    _LastMan = value;
                    OnPropertyChanged("LastMan");
                }
            }
            public ObservableCollection<Sound> KillingSpree
            {
                get => _KillingSpree;
                set
                {
                    _KillingSpree = value;
                    OnPropertyChanged("KillingSpree");
                }
            }
            public ObservableCollection<AbilitySound> Ability
            {
                get => _Ability;
                set
                {
                    _Ability = value;
                    OnPropertyChanged("Ability");
                }
            }
            public ObservableCollection<Sound> CatchPhrase
            {
                get => _CatchPhrase;
                set
                {
                    _CatchPhrase = value;
                    OnPropertyChanged("CatchPhrase");
                }
            }
            public ObservableCollection<MusicSound> Music
            {
                get => _Music;
                set
                {
                    _Music = value;
                    OnPropertyChanged("Music");
                }
            }
            public ObservableCollection<Sound> Backstab
            {
                get => _Backstab;
                set
                {
                    _Backstab = value;
                    OnPropertyChanged("Backstab");
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged(string name)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }

            public SoundPkg Clone()
            {
                SoundPkg clone = new SoundPkg();

                foreach (Sound sound in Startup)
                    clone.Startup.Add(sound.Clone());

                foreach (Sound sound in Victory)
                    clone.Victory.Add(sound.Clone());

                foreach (Sound sound in Death)
                    clone.Death.Add(sound.Clone());

                foreach (Sound sound in KillPlayer)
                    clone.KillPlayer.Add(sound.Clone());

                foreach (Sound sound in LastMan)
                    clone.LastMan.Add(sound.Clone());

                foreach (Sound sound in KillingSpree)
                    clone.KillingSpree.Add(sound.Clone());

                foreach (AbilitySound sound in Ability)
                    clone.Ability.Add(sound.Clone());

                foreach (Sound sound in CatchPhrase)
                    clone.CatchPhrase.Add(sound.Clone());

                foreach (MusicSound sound in Music)
                    clone.Music.Add(sound.Clone());

                foreach (Sound sound in Backstab)
                    clone.Backstab.Add(sound.Clone());

                return clone;
            }

            public bool IsClassEmpty()
            {
                foreach (IClassFunctions classF in Startup)
                    if (!classF.IsClassEmpty())
                        return false;
                foreach (IClassFunctions classF in Victory)
                    if (!classF.IsClassEmpty())
                        return false;
                foreach (IClassFunctions classF in Death)
                    if (!classF.IsClassEmpty())
                        return false;
                foreach (IClassFunctions classF in KillPlayer)
                    if (!classF.IsClassEmpty())
                        return false;
                foreach (IClassFunctions classF in LastMan)
                    if (!classF.IsClassEmpty())
                        return false;
                foreach (IClassFunctions classF in KillingSpree)
                    if (!classF.IsClassEmpty())
                        return false;
                foreach (IClassFunctions classF in Ability)
                    if (!classF.IsClassEmpty())
                        return false;
                foreach (IClassFunctions classF in CatchPhrase)
                    if (!classF.IsClassEmpty())
                        return false;
                foreach (IClassFunctions classF in Music)
                    if (!classF.IsClassEmpty())
                        return false;
                foreach (IClassFunctions classF in Backstab)
                    if (!classF.IsClassEmpty())
                        return false;

                return Startup.Count == 0 &&
                       Victory.Count == 0 &&
                       Death.Count == 0 &&
                       KillPlayer.Count == 0 &&
                       LastMan.Count == 0 &&
                       KillingSpree.Count == 0 &&
                       Ability.Count == 0 &&
                       CatchPhrase.Count == 0 &&
                       Music.Count == 0 &&
                       Backstab.Count == 0;
            }

            public bool IsClassEqual(IClassFunctions Obj)
            {
                if (Obj is SoundPkg pkg)
                {
                    if (!CheckClassListEquality(pkg.Startup, Startup))
                        return false;
                    if (!CheckClassListEquality(pkg.Victory, Victory))
                        return false;
                    if (!CheckClassListEquality(pkg.Death, Death))
                        return false;
                    if (!CheckClassListEquality(pkg.KillPlayer, KillPlayer))
                        return false;
                    if (!CheckClassListEquality(pkg.LastMan, LastMan))
                        return false;
                    if (!CheckClassListEquality(pkg.KillingSpree, KillingSpree))
                        return false;
                    if (!CheckClassListEquality(pkg.Ability, Ability))
                        return false;
                    if (!CheckClassListEquality(pkg.CatchPhrase, CatchPhrase))
                        return false;
                    if (!CheckClassListEquality(pkg.Music, Music))
                        return false;
                    if (!CheckClassListEquality(pkg.Backstab, Backstab))
                        return false;
                    return true;
                }
                return false;
            }
        }

        public class Weapon : INotifyPropertyChanged, IClassFunctions
        {
            public class Attribute : INotifyPropertyChanged, IClassFunctions
            {
                public Attribute(Weapon parent)
                {
                    Parent = parent;
                }

                private string _Name = "";
                private int _ID = 1;
                private double _Arg = 0;

                public string Name
                {
                    get => _Name;
                    set
                    {
                        _Name = value;
                        OnPropertyChanged("Name");
                    }
                }
                public int ID
                {
                    get => _ID;
                    set
                    {
                        _ID = value;
                        OnPropertyChanged("ID");
                    }
                }
                public double Arg
                {
                    get => _Arg;
                    set
                    {
                        _Arg = value;
                        OnPropertyChanged("Arg");
                    }
                }
                [Newtonsoft.Json.JsonIgnore]
                public Weapon Parent { get; } = null;

                public event PropertyChangedEventHandler PropertyChanged;

                protected void OnPropertyChanged(string name)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
                }

                public Attribute Clone(Weapon _Parent)
                {
                    Attribute clone = new Attribute(_Parent)
                    {
                        Name = Name,
                        ID = ID,
                        Arg = Arg
                    };
                    return clone;
                }

                public bool IsClassEmpty()
                {
                    return string.IsNullOrWhiteSpace(Name);
                }

                public bool IsClassEqual(IClassFunctions Obj)
                {
                    if (Obj is Attribute attr)
                    {
                        return Name == attr.Name && ID == attr.ID && Arg == attr.Arg;
                    }
                    return false;
                }
            }

            private int _Index = -1;
            private string _Class = "";
            private bool _Visible = false;
            private ObservableCollection<Attribute> _Attributes = new ObservableCollection<Attribute>();

            public int Index
            {
                get => _Index;
                set
                {
                    _Index = value;
                    OnPropertyChanged("Index");
                }
            }
            public string Class
            {
                get => _Class;
                set
                {
                    _Class = value;
                    OnPropertyChanged("Class");
                }
            }
            public bool Visible
            {
                get => _Visible;
                set
                {
                    _Visible = value;
                    OnPropertyChanged("Visible");
                }
            }
            public ObservableCollection<Attribute> Attributes
            {
                get => _Attributes;
                set
                {
                    _Attributes = value;
                    OnPropertyChanged("Attributes");
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged(string name)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }

            public Weapon Clone()
            {
                Weapon clone = new Weapon()
                {
                    Index = Index,
                    Class = Class,
                    Visible = Visible
                };
                foreach (Attribute attr in Attributes)
                    clone.Attributes.Add(attr.Clone(clone));
                return clone;
            }

            public bool IsClassEmpty()
            {
                foreach (IClassFunctions classF in Attributes)
                    if (!classF.IsClassEmpty())
                        return false;

                return string.IsNullOrWhiteSpace(Class) && Attributes.Count == 0;
            }

            public bool IsClassEqual(IClassFunctions Obj)
            {
                if (Obj is Weapon wep)
                {
                    if (Index != wep.Index || Class != wep.Class || Visible != wep.Visible)
                        return false;

                    if (!CheckClassListEquality(wep.Attributes, Attributes))
                        return false;
                    return true;
                }
                return false;
            }
        }

        public class WeaponTemplate : Weapon
        {
            private List<int> _Characters = new List<int>();
            private string _Name = "";

            public List<int> Characters
            {
                get => _Characters;
                set
                {
                    _Characters = value;
                    OnPropertyChanged("Characters");
                }
            }
            public string Name
            {
                get => _Name;
                set
                {
                    _Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        #endregion

        public static bool CheckClassListEquality(IEnumerable<IClassFunctions> List1, IEnumerable<IClassFunctions> List2)
        {
            if (List1.Count() != List2.Count())
                return false;
            int count = List1.Count();
            for(int i = 0; i < count; i++)
                if (!List1.ElementAt(i).IsClassEqual(List2.ElementAt(i)))
                    return false;
            return true;
        }
    }
}
