using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TF2WepDDBBDownloader
{
    class Program
    {
        private static readonly string NEWLINE = Environment.NewLine;

        public enum Process
        {
            Attribute,
            Weapons
        }

        public class Attribute
        {
            public string Name { get; set; }
            public int ID { get; set; }
        }

        public class Weapon
        {
            public int Index { get; set; }
            public string Class { get; set; }
            public string Name { get; set; }
            public List<int> Characters { get; set; } = new List<int>();
        }

        private static readonly string[,] WEAPONSNAMEFILTER = new string[,]
        {
            { "TF_WEAPON_BAT", "Bat" },
            { "TF_WEAPON_BOTTLE", "Bottle" },
            { "TF_WEAPON_FIREAXE", "Fire Axe" },
            { "TF_WEAPON_CLUB", "Kukri" },
            { "TF_WEAPON_KNIFE", "Knife" },
            { "TF_WEAPON_FISTS", "Fists" },
            { "TF_WEAPON_SHOVEL", "Shovel" },
            { "TF_WEAPON_WRENCH", "Wrench" },
            { "TF_WEAPON_BONESAW", "Bonesaw" },
            { "TF_WEAPON_SHOTGUN_PRIMARY", "Engineer's Shotgun" },
            { "TF_WEAPON_SHOTGUN_SOLDIER", "Soldier's Shotgun" },
            { "TF_WEAPON_SHOTGUN_HWG", "Heavy's Shotgun" },
            { "TF_WEAPON_SHOTGUN_PYRO", "Pyro's Shotgun" },
            { "TF_WEAPON_SCATTERGUN", "Scattergun" },
            { "TF_WEAPON_SNIPERRIFLE", "Sniper Rifle" },
            { "TF_WEAPON_MINIGUN", "Minigun" },
            { "TF_WEAPON_SMG", "SMG" },
            { "TF_WEAPON_SYRINGEGUN_MEDIC", "Syringe Gun" },
            { "TF_WEAPON_ROCKETLAUNCHER", "Rocket Launcher" },
            { "TF_WEAPON_GRENADELAUNCHER", "Grenade Launcher" },
            { "TF_WEAPON_PIPEBOMBLAUNCHER", "Stickybomb Launcher" },
            { "TF_WEAPON_PISTOL", "Engineer's Pistol" },
            { "TF_WEAPON_PISTOL_SCOUT", "Scout's Pistol" },
            { "TF_WEAPON_REVOLVER", "Revolver" },
            { "TF_WEAPON_PDA_ENGINEER_BUILD", "Construction PDA" },
            { "TF_WEAPON_PDA_ENGINEER_DESTROY", "Destruction PDA" },
            { "TF_WEAPON_PDA_SPY", "Disguise Kit PDA" },
            { "TF_WEAPON_BUILDER", "PDA" },
            { "TF_WEAPON_MEDIGUN", "Medi Gun" },
            { "TF_WEAPON_INVIS", "Invis Watch" },
            { "TF_WEAPON_BUILDER_SPY", "Sapper" },
            { "TF_WEAPON_SPELLBOOK", "Spellbook Magazine (Stock)" },
            { "TF_WEAPON_GRAPPLINGHOOK", "Grappling Hook" },
        };

        private static readonly string ATTRIBUTESDEFURL = "https://wiki.teamfortress.com/wiki/List_of_item_attributes";
        private static readonly string ATTRIBUTESDEFAULTPATH = Directory.GetCurrentDirectory() + "/attributes.json";
        private static readonly string WEAPONSDEFAULTPATH = Directory.GetCurrentDirectory() + "/weapons.json";
        private static readonly string WEAPONSCACHEPATH = Directory.GetCurrentDirectory() + "/weapons-cache.json";

        private static readonly string ERRORJSONTEXT = "ERROR";

#if DEBUG
        private static bool ISDEBUG = true;
#else
        private static bool ISDEBUG = false;
#endif

        static async Task<int> Main(string[] args)
        {
            string outputFilePath = ATTRIBUTESDEFAULTPATH;
            string inputFilePath = "";
            Process process = Process.Attribute;

            for(int i = 0; i < args.Length; i++)
            {
                string lowArg = args[i].ToLower();

                if(lowArg == "-output" || lowArg == "-o")
                {
                    if (i + 1 < args.Length)
                    {
                        outputFilePath = args[i + 1];

                        if (string.IsNullOrWhiteSpace(outputFilePath))
                        {
                            Console.WriteLine("ERROR: Please, insert a valid path.");
                            return -1;
                        }

                        try
                        {
                            outputFilePath = Path.GetFullPath(outputFilePath);
                        } catch(Exception ex)
                        {
                            Console.WriteLine("ERROR: " + ex.ToString());
                            return -1;
                        }
                    }
                    else
                    {
                        Console.WriteLine("ERROR: Please, insert the output file Path.");
                        return -1;
                    }
                    i++;
                } else if(lowArg == "-weapons" || lowArg == "-wep" || lowArg == "-w")
                {
                    process = Process.Weapons;
                    if (outputFilePath == ATTRIBUTESDEFAULTPATH)
                        outputFilePath = WEAPONSDEFAULTPATH;
                } else if (lowArg == "-input" || lowArg == "-i")
                {
                    if (i + 1 < args.Length)
                    {
                        inputFilePath = args[i + 1];

                        if (string.IsNullOrWhiteSpace(inputFilePath))
                        {
                            Console.WriteLine("ERROR: Please, insert a valid path.");
                            return -1;
                        }

                        if (!inputFilePath.EndsWith("items_game.txt"))
                        {
                            Console.WriteLine("ERROR: The input file MUST be items_game.txt extracted from /tf/scripts/items/");
                            return -1;
                        }

                        try
                        {
                            inputFilePath = Path.GetFullPath(inputFilePath);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("ERROR: " + ex.ToString());
                            return -1;
                        }
                    }
                    else
                    {
                        Console.WriteLine("ERROR: Please, insert the input file Path.");
                        return -1;
                    }
                    i++;
                } else if(lowArg == "-c" || lowArg == "-clean")
                {
                    File.Delete(WEAPONSCACHEPATH);
                } else if(lowArg == "-d" || lowArg == "-debug")
                {
                    ISDEBUG = true;
                }
            }

            if(process == Process.Weapons && string.IsNullOrWhiteSpace(inputFilePath))
            {
                Console.WriteLine("ERROR: Please, insert the input file Path (with the -input flag)");
                return -1;
            }

            string outputJson;
            if (process == Process.Attribute)
                outputJson = DownloadAttributesDefinition();
            else
                outputJson = await GenerateWeaponsDefinition(inputFilePath);

            if (string.IsNullOrWhiteSpace(outputJson) || outputJson == ERRORJSONTEXT)
                return -2;

            using (StreamWriter outputWriter = File.CreateText(outputFilePath))
            {
                outputWriter.Write(outputJson);
            }

            return 0;
        }

        private static string DownloadAttributesDefinition()
        {
            try
            {
                HtmlWeb web = new HtmlWeb();
                HtmlDocument htmlDoc = web.Load(ATTRIBUTESDEFURL, "GET");

                HtmlNode tBody = htmlDoc.DocumentNode.Descendants("table").FirstOrDefault();
                if(tBody == null)
                {
                    Console.WriteLine("ERROR: table not found.");
                    return ERRORJSONTEXT;
                }
                List<Attribute> attrList = new List<Attribute>();
                foreach(HtmlNode trNode in tBody.ChildNodes)
                {
                    if (trNode.ChildNodes.Count < 2)
                        continue;

                    Attribute attr = new Attribute();
                    HtmlNode idTdNode = trNode.ChildNodes[1];
                    if (int.TryParse(idTdNode.InnerText, out int id))
                        attr.ID = id;
                    else
                        continue;

                    HtmlNode nameTdNode = trNode.ChildNodes[3];
                    attr.Name = nameTdNode.InnerText.Replace("\n", "").Trim();
                    if (string.IsNullOrWhiteSpace(attr.Name))
                        continue;

                    Console.WriteLine(string.Format("Attribute found ({0}: {1})", attr.ID, attr.Name));
                    attrList.Add(attr);
                }

                if(attrList.Count == 0)
                {
                    Console.WriteLine("ERROR: No attribute found.");
                    return ERRORJSONTEXT;
                }

                return JsonConvert.SerializeObject(attrList, Formatting.Indented);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.ToString());
                return ERRORJSONTEXT;
            }
        }

        private static async Task<string> GenerateWeaponsDefinition(string InputFile)
        {
            try
            {
                string json = "";

                if (File.Exists(WEAPONSCACHEPATH))
                {
                    using (StreamReader sr = new StreamReader(WEAPONSCACHEPATH))
                    {
                        json = await sr.ReadToEndAsync();
                    }
                }
                else
                {
                    using (StreamReader sr = new StreamReader(InputFile))
                    {
                        string cfg = await sr.ReadToEndAsync();
                        json = "{" + NEWLINE;
                        string[] lines = cfg.Split(new string[] { NEWLINE }, StringSplitOptions.None);
                        Console.WriteLine("Converting CFG to JSON... (This could take a while)");
                        for (int i = 0; i < lines.Length; i++)
                        {
                            string neoLine = lines[i].Replace("\\", "\\\\");
                            if (neoLine.Replace(" ", "").Replace("\t", "").StartsWith("//"))
                                continue;
                            Match keyValueRegex = Regex.Match(neoLine, "\"(.+?)\"([ \t]+)?\"(.*?)\"");
                            if (keyValueRegex.Success)
                            {
                                json += string.Format("\"{0}\": \"{1}\",{2}", keyValueRegex.Groups[1]?.Value.ToLower(), keyValueRegex.Groups[3], NEWLINE);
                                continue;
                            }
                            Match keyRegex = Regex.Match(neoLine, "\"(.+?)\"");
                            if (keyRegex.Success)
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
                            DrawTextProgressBar(i, lines.Length);
                        }
                        json += NEWLINE + "}";

                        using (StreamWriter cacheWriter = File.CreateText(WEAPONSCACHEPATH))
                        {
                            cacheWriter.Write(json);
                        }
                    }
                }

                List<Weapon> wepList = new List<Weapon>();
                JObject openJson = JsonConvert.DeserializeObject<JObject>(json);
                if (openJson["items_game"] is JObject root)
                {
                    if (root["items"] is JObject items)
                    {
                        foreach (JProperty itemProp in items.Children())
                        {
                            if (!int.TryParse(itemProp.Name, out int index))
                                continue;
                            JObject item = (JObject)itemProp.Value;

                            Weapon neoWep = new Weapon()
                            {
                                Index = index,
                                Name = ReplacePlaceholderName(JTokenToString(item["name"]))
                            };

                            List<JObject> prefabsList = new List<JObject>()
                            {
                                item
                            };
                            while(prefabsList[prefabsList.Count - 1]["prefab"] != null)
                            {
                                string prefabName = JTokenToString(prefabsList[prefabsList.Count - 1]["prefab"]);
                                if (string.IsNullOrWhiteSpace(prefabName) || prefabsList.Any(t => t.Properties().Where(p => p.Name == prefabName).Count() > 0))
                                    break;
                                if (prefabName.Contains(' '))
                                {
                                    string[] prefArray = prefabName.Split(' ');
                                    for(int i = 0; i < prefArray.Length; i++)
                                    {
                                        if (prefArray[i].StartsWith("weapon"))
                                        {
                                            prefabName = prefArray[i];
                                            break;
                                        }
                                    }
                                }
                                    

                                if (!(root["prefabs"][prefabName] is JObject prefab))
                                    break;

                                prefabsList.Add(prefab);
                            }

                            foreach(JObject prefab in prefabsList)
                            {
                                if (prefab["item_class"] != null)
                                {
                                    neoWep.Class = JTokenToString(prefab["item_class"]);
                                    break;
                                }
                            }
                            if (string.IsNullOrWhiteSpace(neoWep.Class))
                                continue;

                            if (!neoWep.Class.StartsWith("tf_weapon") && !neoWep.Class.StartsWith("saxxy"))
                                continue;

                            foreach (JObject prefab in prefabsList)
                            {
                                if (prefab["used_by_classes"] != null)
                                {
                                    foreach (JProperty characterProp in prefab["used_by_classes"].Children())
                                    {
                                        if (JTokenToString(characterProp.Value) != "1")
                                            continue;
                                        switch (characterProp.Name)
                                        {
                                            case "scout":
                                                neoWep.Characters.Add(1);
                                                break;
                                            case "sniper":
                                                neoWep.Characters.Add(2);
                                                break;
                                            case "soldier":
                                                neoWep.Characters.Add(3);
                                                break;
                                            case "demoman":
                                                neoWep.Characters.Add(4);
                                                break;
                                            case "medic":
                                                neoWep.Characters.Add(5);
                                                break;
                                            case "heavy":
                                                neoWep.Characters.Add(6);
                                                break;
                                            case "pyro":
                                                neoWep.Characters.Add(7);
                                                break;
                                            case "spy":
                                                neoWep.Characters.Add(8);
                                                break;
                                            case "engineer":
                                                neoWep.Characters.Add(9);
                                                break;
                                        }
                                    }
                                    break;
                                }
                            }

                            wepList.Add(neoWep);
                        }
                    }
                }

                if (wepList.Count == 0)
                {
                    Console.WriteLine("ERROR: No weapon found.");
                    return ERRORJSONTEXT;
                }

                if(!ISDEBUG)
                    File.Delete(WEAPONSCACHEPATH);
                return JsonConvert.SerializeObject(wepList, Formatting.Indented);

            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.ToString());
                return ERRORJSONTEXT;
            }
        }

        private static string JTokenToString(JToken token)
        {
            if (token == null)
                return "";
            return (string)token;
        }

        //FROM: https://stackoverflow.com/questions/24918768/progress-bar-in-console-application
        private static void DrawTextProgressBar(int progress, int total)
        {
            Console.CursorLeft = 0;
            Console.Write("[");
            float onechunk = 30.0f / total;

            //draw filled part
            int position = 1;
            for (int i = 0; i < onechunk * progress; i++)
            {
                Console.CursorLeft = position++;
                Console.Write("#");
            }

            //draw unfilled part
            for (int i = position; i <= 31; i++)
            {
                Console.CursorLeft = position++;
                Console.Write("");
            }

            Console.Write("] ");
            Console.Write(progress.ToString() + " of " + total.ToString() + "    "); //blanks at the end remove any excess
        }

        private static string ReplacePlaceholderName(string input)
        {
            for(int i = 0; i < WEAPONSNAMEFILTER.Length / 2; i++)
            {
                if (input.ToLower().StartsWith("upgradeable") && input.ToUpper().EndsWith(WEAPONSNAMEFILTER[i, 0]))
                    return WEAPONSNAMEFILTER[i, 1] + " (Renamed/Strange)";
                else if (input.ToUpper() == WEAPONSNAMEFILTER[i, 0])
                    return WEAPONSNAMEFILTER[i, 1];
            }
            return input;
        }
    }
}
