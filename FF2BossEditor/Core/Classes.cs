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
        public class Ability : INotifyPropertyChanged
        {
            public class Argument : INotifyPropertyChanged
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
        }

        public class Boss : INotifyPropertyChanged
        {
            private string _Name = "";
            private int _Class = 1;
            private string _Model = "";
            private int _RageDist = 800;
            private string _RageDamage = "1900";
            private string _Health = "(((760+n)*n)^1.04)";
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
        }

        public class Description : INotifyPropertyChanged
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
        }

        public class SoundPkg : INotifyPropertyChanged
        {
            public class Sound : INotifyPropertyChanged
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
        }

        public class Weapon : INotifyPropertyChanged
        {
            public class Attribute : INotifyPropertyChanged
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
            }

            private int _Index = 0;
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
    }
}
