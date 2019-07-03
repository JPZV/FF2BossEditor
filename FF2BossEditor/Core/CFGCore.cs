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

        public static async Task<bool> ExportBoss(Classes.Boss Boss)
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
                    using (var file = File.CreateText(saveDialog.FileName))
                    {
                        string cfg = "";

                        //"CFG Generated by" comment
                        cfg += "//This Configuration File was made using JPZV's FF2 Boss Editor" + NEWLINE;

                        //Root Start
                        cfg += "\"character\"" + NEWLINE + "{" + NEWLINE;

                        //Boss basic info
                        cfg += "//Boss Basic Info" + NEWLINE;
                        cfg += NodeCreator("name", Boss.Name, 1, 14, "Boss public name.");
                        cfg += NodeCreator("class", Boss.Class.ToString(), 1, 14, "Boss based TF2 class.");
                        cfg += NodeCreator("model", Boss.Model, 1, 14, "Boss model.");
                        cfg += NodeCreator("ragedamage", Boss.RageDamage.ToString(), 1, 14, "Boss rage damage formula. (Accepted operators in FreakFortressBat: n, +, -, *, /, ^)");
                        cfg += NodeCreator("ragedist", Boss.RageDist.ToString(), 1, 14, "Boss rage distance.");
                        cfg += NodeCreator("health_formula", Boss.Health, 1, 14, "Boss health formula. (Accepted operators: n, +, -, *, /, ^)");
                        cfg += NodeCreator("maxspeed", Boss.Speed.ToString(), 1, 14, "Boss max speed.");
                        cfg += NEWLINE;

                        //Boss description
                        cfg += "//Boss Description(s)" + NEWLINE;
                        foreach(Classes.Description desc in Boss.Descriptions)
                            cfg += NodeCreator(string.Format("description_{0}", desc.Lang), desc.Text.Replace("\r\n", "\n").Replace("\n", "\\n"), 1, 14);
                        cfg += NEWLINE;

                        //Weapons
                        int wepCount = 1;
                        cfg += "//Boss Weapon(s)" + NEWLINE;
                        foreach (Classes.Weapon wep in Boss.Weapons)
                        {
                            cfg += string.Format("\t\"weapon{0}\"", wepCount) + NEWLINE;
                            cfg += "\t{" + NEWLINE;
                            cfg += NodeCreator("name", wep.Class, 2, 10, "Weapon Class (Name).");
                            cfg += NodeCreator("index", wep.Index.ToString(), 2, 10, "Weapon Index.");
                            string attrStr = "";
                            foreach(Classes.Weapon.Attribute attr in wep.Attributes)
                            {
                                if (attrStr != "")
                                    attrStr += " ; ";
                                attrStr += string.Format("{0} ; {1}", attr.ID, attr.Arg);
                            }
                            cfg += NodeCreator("attributes", attrStr, 2, 10, "Weapon attributes.");
                            cfg += NodeCreator("show", wep.Visible ? "1" : "0", 2, 10, "Weapon visibility. (0: Invisible. 1: Visible)");
                            cfg += "\t}" + NEWLINE;
                            wepCount++;
                        }
                        cfg += NEWLINE;

                        //Abilities
                        int abiCount = 1;
                        cfg += "//Boss Ability(s)" + NEWLINE;
                        foreach (Classes.Ability abi in Boss.Abilities)
                        {
                            cfg += string.Format("\t\"ability{0}\"", abiCount) + NEWLINE;
                            cfg += "\t{" + NEWLINE;

                            int maxArgIndex = abi.Arguments.Max(t => t.Index);
                            int interSepCount = maxArgIndex.ToString().Length + 3 > 11 ? maxArgIndex.ToString().Length + 3 : 11;

                            cfg += NodeCreator("name", abi.Name, 2, interSepCount, "Ability Name.") + NEWLINE;

                            foreach (Classes.Ability.Argument arg in abi.Arguments)
                                cfg += NodeCreator("arg" + arg.Index, arg.Value, 2, interSepCount, arg.Alias);

                            cfg += NEWLINE;

                            cfg += NodeCreator("plugin_name", abi.Plugin, 2, interSepCount, "Ability Plugin (without the extension).");
                            cfg += "\t}" + NEWLINE;
                            abiCount++;
                        }
                        cfg += NEWLINE;

                        //Sounds
                        //Music
                        cfg += "//Sound(s)" + NEWLINE;
                        int i;
                        List<string> allSoundsPath = new List<string>();
                        cfg += "\t\"sound_bgm\"" + NEWLINE;
                        cfg += "\t{" + NEWLINE;
                        int maxMusicIndex = Boss.Sounds.Music.Count.ToString().Length + 4;
                        for(i = 0; i < Boss.Sounds.Music.Count; i++)
                        {
                            cfg += NodeCreator("path" + (i + 1).ToString(), RemoveSoundFromPath(Boss.Sounds.Music[i].Path), 2, maxMusicIndex, "Path relative to 'sound' folder.");
                            cfg += NodeCreator("time" + (i + 1).ToString(), Boss.Sounds.Music[i].Length.ToString(), 2, maxMusicIndex, "Music length in seconds.");
                            allSoundsPath.Add(Boss.Sounds.Music[i].Path);
                        }
                        cfg += "\t}" + NEWLINE;

                        //Startup
                        cfg += "\t\"sound_begin\"" + NEWLINE;
                        cfg += "\t{" + NEWLINE;
                        for (i = 0; i < Boss.Sounds.Startup.Count; i++)
                        {
                            cfg += NodeCreator((i + 1).ToString(), RemoveSoundFromPath(Boss.Sounds.Startup[i].Path), 2, 3, "Path relative to 'sound' folder.");
                            allSoundsPath.Add(Boss.Sounds.Startup[i].Path);
                        }
                        cfg += "\t}" + NEWLINE;

                        //Victory
                        cfg += "\t\"sound_win\"" + NEWLINE;
                        cfg += "\t{" + NEWLINE;
                        for (i = 0; i < Boss.Sounds.Victory.Count; i++)
                        {
                            cfg += NodeCreator((i + 1).ToString(), RemoveSoundFromPath(Boss.Sounds.Victory[i].Path), 2, 3, "Path relative to 'sound' folder.");
                            allSoundsPath.Add(Boss.Sounds.Victory[i].Path);
                        }
                        cfg += "\t}" + NEWLINE;

                        //LastMan
                        cfg += "\t\"sound_lastman\"" + NEWLINE;
                        cfg += "\t{" + NEWLINE;
                        for (i = 0; i < Boss.Sounds.LastMan.Count; i++)
                        {
                            cfg += NodeCreator((i + 1).ToString(), RemoveSoundFromPath(Boss.Sounds.LastMan[i].Path), 2, 3, "Path relative to 'sound' folder.");
                            allSoundsPath.Add(Boss.Sounds.LastMan[i].Path);
                        }
                        cfg += "\t}" + NEWLINE;

                        //Death
                        cfg += "\t\"sound_death\"" + NEWLINE;
                        cfg += "\t{" + NEWLINE;
                        for (i = 0; i < Boss.Sounds.Death.Count; i++)
                        {
                            cfg += NodeCreator((i + 1).ToString(), RemoveSoundFromPath(Boss.Sounds.Death[i].Path), 2, 3, "Path relative to 'sound' folder.");
                            allSoundsPath.Add(Boss.Sounds.Death[i].Path);
                        }
                        cfg += "\t}" + NEWLINE;

                        //Kill
                        cfg += "\t\"sound_hit\"" + NEWLINE;
                        cfg += "\t{" + NEWLINE;
                        for (i = 0; i < Boss.Sounds.KillPlayer.Count; i++)
                        {
                            cfg += NodeCreator((i + 1).ToString(), RemoveSoundFromPath(Boss.Sounds.KillPlayer[i].Path), 2, 3, "Path relative to 'sound' folder.");
                            allSoundsPath.Add(Boss.Sounds.KillPlayer[i].Path);
                        }
                        cfg += "\t}" + NEWLINE;

                        //Kill
                        cfg += "\t\"sound_kspree\"" + NEWLINE;
                        cfg += "\t{" + NEWLINE;
                        for (i = 0; i < Boss.Sounds.KillingSpree.Count; i++)
                        {
                            cfg += NodeCreator((i + 1).ToString(), RemoveSoundFromPath(Boss.Sounds.KillingSpree[i].Path), 2, 3, "Path relative to 'sound' folder.");
                            allSoundsPath.Add(Boss.Sounds.KillingSpree[i].Path);
                        }
                        cfg += "\t}" + NEWLINE;

                        //Ability
                        cfg += "\t\"sound_ability\"" + NEWLINE;
                        cfg += "\t{" + NEWLINE;
                        int maxAbiIndex = Boss.Sounds.Ability.Count.ToString().Length + 4;
                        for (i = 0; i < Boss.Sounds.Ability.Count; i++)
                        {
                            cfg += NodeCreator((i + 1).ToString(), RemoveSoundFromPath(Boss.Sounds.Ability[i].Path), 2, maxAbiIndex, "Path relative to 'sound' folder.");
                            if(Boss.Sounds.Ability[i].Slot != 0)
                                cfg += NodeCreator("slot" + (i + 1).ToString(), Boss.Sounds.Ability[i].Slot.ToString(), 2, maxAbiIndex, string.Format("Ability Slot {0}", Boss.Sounds.Ability[i].Slot));
                            allSoundsPath.Add(Boss.Sounds.Ability[i].Path);
                        }
                        cfg += "\t}" + NEWLINE;

                        //Catch
                        cfg += "\t\"catch_phrase\"" + NEWLINE;
                        cfg += "\t{" + NEWLINE;
                        for (i = 0; i < Boss.Sounds.CatchPhrase.Count; i++)
                        {
                            cfg += NodeCreator((i + 1).ToString(), RemoveSoundFromPath(Boss.Sounds.CatchPhrase[i].Path), 2, 3, "Path relative to 'sound' folder.");
                            allSoundsPath.Add(Boss.Sounds.CatchPhrase[i].Path);
                        }
                        cfg += "\t}" + NEWLINE;

                        //Backstab
                        cfg += "\t\"sound_stabbed\"" + NEWLINE;
                        cfg += "\t{" + NEWLINE;
                        for (i = 0; i < Boss.Sounds.Backstab.Count; i++)
                        {
                            cfg += NodeCreator((i + 1).ToString(), RemoveSoundFromPath(Boss.Sounds.Backstab[i].Path), 2, 3, "Path relative to 'sound' folder.");
                            allSoundsPath.Add(Boss.Sounds.Backstab[i].Path);
                        }
                        cfg += "\t}" + NEWLINE;

                        //Pre-Cache
                        cfg += "\t\"sound_precache\"" + NEWLINE;
                        cfg += "\t{" + NEWLINE;
                        for (i = 0; i < Boss.Sounds.Music.Count; i++)
                            cfg += NodeCreator((i + 1).ToString(), RemoveSoundFromPath(Boss.Sounds.Music[i].Path), 2, 3, "Path relative to 'sound' folder.");
                        cfg += "\t}" + NEWLINE;

                        cfg += NEWLINE;

                        //Downloads
                        List<string> allDownloadsPath = new List<string>(Boss.CustomFiles);
                        allDownloadsPath.AddRange(allSoundsPath);
                        List<string> fixedDownloadsPath = allDownloadsPath.GroupBy(t => t).Select(x => x.First()).ToList();
                        List<string> modDownloadPath = new List<string>()
                        {
                            RemoveRootSlashFromPath(Boss.Model)
                        };
                        cfg += "//download(s)" + NEWLINE;
                        cfg += "\t\"download\"" + NEWLINE;
                        cfg += "\t{" + NEWLINE;
                        int maxDownloadIndex = fixedDownloadsPath.Count.ToString().Length;
                        for (i = 0; i < fixedDownloadsPath.Count; i++)
                        {
                            string fixedPath = fixedDownloadsPath[i].StartsWith("/") || fixedDownloadsPath[i].StartsWith("\\") ? RemoveRootSlashFromPath(fixedDownloadsPath[i]) : fixedDownloadsPath[i];
                            if (!fixedPath.StartsWith("models"))
                                cfg += NodeCreator((i + 1).ToString(), fixedPath, 2, maxDownloadIndex);
                            else if (fixedPath.EndsWith(".mdl"))
                                modDownloadPath.Add(fixedPath);
                        }
                        cfg += "\t}" + NEWLINE;
                        cfg += NEWLINE;

                        //Mod Download
                        cfg += "\t\"mod_download\"" + NEWLINE;
                        cfg += "\t{" + NEWLINE;
                        for (i = 0; i < modDownloadPath.Count; i++)
                        {
                            if (modDownloadPath[i].StartsWith("models"))
                            {
                                int mdlPos = modDownloadPath[i].IndexOf(".mdl");
                                if (mdlPos > 0)
                                {
                                    cfg += NodeCreator((i + 1).ToString(), modDownloadPath[i].Substring(0, mdlPos), 2, 3);
                                }
                            }
                        }
                        cfg += "\t}" + NEWLINE;

                        //Root ending
                        cfg += "}" + NEWLINE;

                        await file.WriteAsync(cfg);
                        return true;
                    }
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

#pragma warning disable IDE0051 // Remove unused private members
        private static double JTokenToDouble(JToken token, double DefVal = 0)
#pragma warning restore IDE0051 // Remove unused private members
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

        private static string NodeCreator(string Key, string Value, int PreTabsCount, int MinKeyLength = 1, string Comments = "")
        {
            string output = "";
            for (int i = 0; i < PreTabsCount; i++)
                output += "\t";
            string keyStr = string.Format("\"{0}\"", Key);
            string valStr = string.Format("\"{0}\"", Value);
            int tmpMinKeyLength = (MinKeyLength + 2) % 4 == 0 ? MinKeyLength + 3 : MinKeyLength + 2; //+2 to include the "". +3 to void "key""value"
            int fixedMinKeyLength = (int)Math.Ceiling(tmpMinKeyLength / 4.0) * 4;
            output += string.Format("{0,-" + (fixedMinKeyLength).ToString() + "}", keyStr).Replace("    ", "\t").Replace("   ", "\t").Replace("  ", "\t").Replace(" ", "\t");
            output += valStr;
            if (!string.IsNullOrWhiteSpace(Comments))
                output += string.Format(" //{0}", Comments);
            output += NEWLINE;
            return output;
        }

        private static string RemoveSoundFromPath(string input)
        {
            Match pathMatch = Regex.Match(":" + input + ":", @":[\\]?sound\\(.+?):", RegexOptions.IgnoreCase);
            if (pathMatch.Success && pathMatch.Groups[1] != null)
                return pathMatch.Groups[1].Value;
            return input;
        }

        private static string RemoveRootSlashFromPath(string input)
        {
            Match pathMatch = Regex.Match(":" + input + ":", @":[\\/](.+?):");
            if (pathMatch.Success && pathMatch.Groups[1] != null)
                return pathMatch.Groups[1].Value;
            return input;
        }
    }
}
