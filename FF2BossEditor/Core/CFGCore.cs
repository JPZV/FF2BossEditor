using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FF2BossEditor.Core
{
    public class CFGCore
    {
        private static readonly string NEWLINE = Environment.NewLine;

        public static async Task<Classes.Boss> ImportBoss()
        {
            Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog()
            {
                DefaultExt = ".cfg",
                Filter = "FF2 Config Boss File (*.cfg)|*.cfg"
            };

            bool? openResult = openDialog.ShowDialog();
            if (openResult == true)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(openDialog.FileName))
                    {
                        string cfg = await sr.ReadToEndAsync();
                        string json = "{" + NEWLINE;
                        foreach(string line in cfg.Split(new string[] { NEWLINE }, StringSplitOptions.None))
                        {
                            string neoLine = line.Replace("\\", "\\\\");
                            if (neoLine.Replace(" ", "").Replace("\t", "").StartsWith("//"))
                                continue;
                            Match keyValueRegex = Regex.Match(neoLine, "\"(.+?)\"[ \t]+\"(.*?)\"");
                            if(keyValueRegex.Success)
                            {
                                json += string.Format("\"{0}\": \"{1}\",{2}", keyValueRegex.Groups[1]?.Value.ToLower(), keyValueRegex.Groups[2], NEWLINE);
                                continue;
                            }
                            Match keyRegex = Regex.Match(neoLine, "\"(.+?)\"");
                            if(keyRegex.Success)
                            {
                                json += string.Format("\"{0}\":{1}", keyRegex.Groups[1]?.Value.ToLower(), NEWLINE);
                                continue;
                            }
                            if (neoLine.Replace(" ", "").Replace("\t", "").StartsWith("}"))
                            {
                                json += "}," + NEWLINE;
                                continue;
                            }
                            json += neoLine + NEWLINE;
                        }
                        json += NEWLINE + "}";

                        JObject openJson = JsonConvert.DeserializeObject<JObject>(json);

                        if (openJson["character"] is JObject boss)
                        {
                            Classes.Boss openedBoss = new Classes.Boss();
                            List<string> downloadList = new List<string>();
                            //Extracted from https://stackoverflow.com/questions/6522358/how-can-i-get-a-list-of-keys-from-json-net
                            foreach (string token in boss.Properties().Select(p => p.Name))
                            {
                                if(token == "name")
                                {
                                    openedBoss.Name = JTokenToString(boss[token]);
                                }
                                else if (token == "class")
                                {
                                    openedBoss.Class = JTokenToInt(boss[token]);
                                }
                                else if (token == "model")
                                {
                                    openedBoss.Model = JTokenToString(boss[token]);
                                }
                                else if (token == "ragedamage")
                                {
                                    openedBoss.RageDamage = JTokenToString(boss[token]);
                                }
                                else if (token == "ragedist")
                                {
                                    openedBoss.RageDist = JTokenToInt(boss[token]);
                                }
                                else if (token == "health_formula")
                                {
                                    openedBoss.Health = JTokenToString(boss[token]);
                                }
                                else if (token == "lives")
                                {
                                    openedBoss.Lives = JTokenToInt(boss[token]);
                                }
                                else if (token == "maxspeed")
                                {
                                    openedBoss.Speed = JTokenToInt(boss[token]);
                                }
                                else if (token == "sound_block_vo")
                                {
                                    openedBoss.BlockVoice = JTokenToInt(boss[token]) == 1;
                                } else if(token.StartsWith("description"))
                                {
                                    Match descMatch = Regex.Match(token, "description_([a-z][a-z])");
                                    if(descMatch.Success && descMatch.Groups[1] != null && !string.IsNullOrWhiteSpace(descMatch.Groups[1].Value))
                                    {
                                        openedBoss.Descriptions.Add(new Classes.Description()
                                        {
                                            Lang = descMatch.Groups[1].Value,
                                            Text = JTokenToString(boss[token]).Replace("\\n\\n", "\\n").Replace("\\n", "\r\n")
                                        });
                                    }
                                } else if(token.StartsWith("weapon") && boss[token] is JObject weapon)
                                {
                                    Classes.Weapon neoWeapon = new Classes.Weapon();
                                    foreach (string wepToken in weapon.Properties().Select(p => p.Name))
                                    {
                                        if (wepToken == "name")
                                        {
                                            neoWeapon.Class = JTokenToString(weapon[wepToken]);
                                        }
                                        else if (wepToken == "index")
                                        {
                                            neoWeapon.Index = JTokenToInt(weapon[wepToken]);
                                        }
                                        else if (wepToken == "show")
                                        {
                                            neoWeapon.Visible = JTokenToInt(weapon[wepToken]) == 1;
                                        }
                                        else if (wepToken == "attributes")
                                        {
                                            string[] attrs = JTokenToString(weapon[wepToken]).Split(';');
                                            for(int i = 0; i < attrs.Length; i += 2)
                                            {
                                                Classes.Weapon.Attribute neoAttr = new Classes.Weapon.Attribute(neoWeapon)
                                                {
                                                    ID = StringToInt(attrs[i])
                                                };
                                                if (i + 1 < attrs.Length && attrs[i + 1].IndexOf(".") >= 0)
                                                    neoAttr.Arg = StringToDouble(attrs[i + 1]);
                                                neoWeapon.Attributes.Add(neoAttr);
                                            }
                                        }
                                    }
                                    openedBoss.Weapons.Add(neoWeapon);
                                } else if (token.StartsWith("ability") && boss[token] is JObject ability)
                                {
                                    Classes.Ability neoAbility = new Classes.Ability();
                                    foreach (string abiToken in ability.Properties().Select(p => p.Name))
                                    {
                                        if (abiToken == "name")
                                        {
                                            neoAbility.Name = JTokenToString(ability[abiToken]);
                                        }
                                        else if (abiToken == "plugin_name")
                                        {
                                            neoAbility.Plugin = JTokenToString(ability[abiToken]);
                                        } else if(abiToken.StartsWith("arg"))
                                        {
                                            Match indexMatch = Regex.Match(abiToken, "arg([0-9]+)");

                                            if (indexMatch.Success && indexMatch.Groups[1] != null && int.TryParse(indexMatch.Groups[1].Value, out int abiIndex))
                                            {
                                                Classes.Ability.Argument neoArg = new Classes.Ability.Argument()
                                                {
                                                    Index = abiIndex,
                                                    Value = JTokenToString(ability[abiToken])
                                                };
                                                neoAbility.Arguments.Add(neoArg);
                                            }
                                        }
                                    }
                                    openedBoss.Abilities.Add(neoAbility);
                                } else if (token.StartsWith("sound_bgm") && boss[token] is JObject music)
                                {
                                    List<Classes.SoundPkg.MusicSound> neoMusicList = new List<Classes.SoundPkg.MusicSound>();
                                    foreach (string musicToken in music.Properties().Select(p => p.Name))
                                    {
                                        if(musicToken.StartsWith("path"))
                                        {
                                            Match indexMatch = Regex.Match(musicToken, "path([0-9]+)");
                                            if (indexMatch.Success && indexMatch.Groups[1] != null && int.TryParse(indexMatch.Groups[1].Value, out int musicIndex))
                                            {
                                                for (int i = 0; neoMusicList.Count < musicIndex; i++)
                                                    neoMusicList.Add(new Classes.SoundPkg.MusicSound());
                                                neoMusicList[musicIndex - 1].Path = "sound\\" + JTokenToString(music[musicToken]);
                                            }
                                        } else if (musicToken.StartsWith("time"))
                                        {
                                            Match indexMatch = Regex.Match(musicToken, "time([0-9]+)");
                                            if (indexMatch.Success && indexMatch.Groups[1] != null && int.TryParse(indexMatch.Groups[1].Value, out int musicIndex))
                                            {
                                                for (int i = 0; neoMusicList.Count < musicIndex; i++)
                                                    neoMusicList.Add(new Classes.SoundPkg.MusicSound());
                                                neoMusicList[musicIndex - 1].Length = JTokenToInt(music[musicToken]);
                                            }
                                        }
                                    }
                                    openedBoss.Sounds.Music = new System.Collections.ObjectModel.ObservableCollection<Classes.SoundPkg.MusicSound>(neoMusicList);
                                } else if (token.StartsWith("sound_begin") && boss[token] is JObject begin)
                                {
                                    foreach (string beginPath in begin.Properties().Select(p => p.Value))
                                    {
                                        openedBoss.Sounds.Startup.Add(new Classes.SoundPkg.Sound() { Path = "sound\\" + beginPath });
                                    }
                                } else if (token.StartsWith("sound_win") && boss[token] is JObject win)
                                {
                                    foreach (string winPath in win.Properties().Select(p => p.Value))
                                    {
                                        openedBoss.Sounds.Victory.Add(new Classes.SoundPkg.Sound() { Path = "sound\\" + winPath });
                                    }
                                } else if (token.StartsWith("sound_lastman") && boss[token] is JObject last)
                                {
                                    foreach (string lastPath in last.Properties().Select(p => p.Value))
                                    {
                                        openedBoss.Sounds.LastMan.Add(new Classes.SoundPkg.Sound() { Path = "sound\\" + lastPath });
                                    }
                                } else if (token.StartsWith("sound_death") && boss[token] is JObject death)
                                {
                                    foreach (string deathPath in death.Properties().Select(p => p.Value))
                                    {
                                        openedBoss.Sounds.Death.Add(new Classes.SoundPkg.Sound() { Path = "sound\\" + deathPath });
                                    }
                                } else if (token.StartsWith("sound_hit") && boss[token] is JObject kill)
                                {
                                    foreach (string killPath in kill.Properties().Select(p => p.Value))
                                    {
                                        openedBoss.Sounds.KillPlayer.Add(new Classes.SoundPkg.Sound() { Path = "sound\\" + killPath });
                                    }
                                } else if (token.StartsWith("sound_kspree") && boss[token] is JObject spree)
                                {
                                    foreach (string spreePath in spree.Properties().Select(p => p.Value))
                                    {
                                        openedBoss.Sounds.KillingSpree.Add(new Classes.SoundPkg.Sound() { Path = "sound\\" + spreePath });
                                    }
                                } else if (token.StartsWith("catch_phrase") && boss[token] is JObject phrase)
                                {
                                    foreach (string phrasePath in phrase.Properties().Select(p => p.Value))
                                    {
                                        openedBoss.Sounds.CatchPhrase.Add(new Classes.SoundPkg.Sound() { Path = "sound\\" + phrasePath });
                                    }
                                } else if (token.StartsWith("sound_stabbed") && boss[token] is JObject stabbed)
                                {
                                    foreach (string stabbedPath in stabbed.Properties().Select(p => p.Value))
                                    {
                                        openedBoss.Sounds.Backstab.Add(new Classes.SoundPkg.Sound() { Path = "sound\\" + stabbedPath });
                                    }
                                } else if (token.StartsWith("sound_ability") && boss[token] is JObject abiSound)
                                {
                                    List<Classes.SoundPkg.AbilitySound> neoAbilitySoundList = new List<Classes.SoundPkg.AbilitySound>();
                                    foreach (string abilityToken in abiSound.Properties().Select(p => p.Name))
                                    {
                                        if (abilityToken.StartsWith("slot"))
                                        {
                                            Match indexMatch = Regex.Match(abilityToken, "slot([0-9]+)");
                                            if (indexMatch.Success && indexMatch.Groups[1] != null && int.TryParse(indexMatch.Groups[1].Value, out int slotIndex))
                                            {
                                                for (int i = 0; neoAbilitySoundList.Count < slotIndex; i++)
                                                    neoAbilitySoundList.Add(new Classes.SoundPkg.AbilitySound());
                                                neoAbilitySoundList[slotIndex - 1].Slot = JTokenToInt(abiSound[abilityToken]);
                                            }
                                        }
                                        else
                                        {
                                            neoAbilitySoundList.Add(new Classes.SoundPkg.AbilitySound() { Path = "sound\\" + JTokenToString(abiSound[abilityToken]) });
                                        }
                                    }
                                    openedBoss.Sounds.Ability = new System.Collections.ObjectModel.ObservableCollection<Classes.SoundPkg.AbilitySound>(neoAbilitySoundList);
                                } else if(token == "download" && boss[token] is JObject download)
                                {
                                    foreach (string filePath in download.Properties().Select(p => p.Value))
                                    {
                                        if(!openedBoss.Sounds.Ability.Any(t => t.Path == filePath) &&
                                           !openedBoss.Sounds.Backstab.Any(t => t.Path == filePath) &&
                                           !openedBoss.Sounds.CatchPhrase.Any(t => t.Path == filePath) &&
                                           !openedBoss.Sounds.Death.Any(t => t.Path == filePath) &&
                                           !openedBoss.Sounds.KillingSpree.Any(t => t.Path == filePath) &&
                                           !openedBoss.Sounds.KillPlayer.Any(t => t.Path == filePath) &&
                                           !openedBoss.Sounds.LastMan.Any(t => t.Path == filePath) &&
                                           !openedBoss.Sounds.Music.Any(t => t.Path == filePath) &&
                                           !openedBoss.Sounds.Startup.Any(t => t.Path == filePath) &&
                                           !openedBoss.Sounds.Victory.Any(t => t.Path == filePath) &&
                                           !downloadList.Any(t => t == filePath))
                                        {
                                            downloadList.Add(filePath);
                                        }
                                    }
                                }
                            }
                            openedBoss.CustomFiles = new System.Collections.ObjectModel.ObservableCollection<string>(downloadList);
                            return openedBoss;
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            }
            return null;
        }

        public static async Task<bool> GenericSaveObject(Classes.Boss Boss)
        {
            Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog()
            {
                DefaultExt = ".cfg",
                Filter = "FF2 Config Boss File (*.cfg)|*.cfg"
            };

            bool? saveResult = saveDialog.ShowDialog();
            if (saveResult == true)
            {
                try
                {
                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            }
            return false;
        }

        private static string JTokenToString(JToken token)
        {
            if (token == null)
                return "";
            return (string)token;
        }

        private static int JTokenToInt(JToken token, int DefVal = 0)
        {
            if (token == null)
                return DefVal;
            if (int.TryParse((string)token, out int res))
                return res;
            return DefVal;
        }

        private static double JTokenToDouble(JToken token, double DefVal = 0)
        {
            if (token == null)
                return DefVal;
            if (double.TryParse((string)token, NumberStyles.Any, CultureInfo.InvariantCulture, out double res))
                return res;
            return DefVal;
        }

        private static int StringToInt(string str, int DefVal = 0)
        {
            if (str == null)
                return DefVal;
            if (int.TryParse(str, out int res))
                return res;
            return DefVal;
        }

        private static double StringToDouble(string str, double DefVal = 0)
        {
            if (str == null)
                return DefVal;
            if (double.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out double res))
                return res;
            return DefVal;
        }
    }
}
