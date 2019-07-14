using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FF2BossEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static List<Core.Classes.Weapon.Attribute> WeaponsAttributes = new List<Core.Classes.Weapon.Attribute>();
        public static List<Core.Classes.WeaponTemplate> WeaponsTemplates = new List<Core.Classes.WeaponTemplate>();
        public static Core.Classes.PluginsPkg Plugins = new Core.Classes.PluginsPkg();

        public static async Task ReloadWeaponsAttributes()
        {
            try
            {
                string attrJsonPath = AppDomain.CurrentDomain.BaseDirectory + "/Data/attributes.json";
                if (File.Exists(attrJsonPath))
                {
                    using (StreamReader sr = new StreamReader(attrJsonPath))
                    {
                        string json = await sr.ReadToEndAsync();
                        WeaponsAttributes = JsonConvert.DeserializeObject<List<Core.Classes.Weapon.Attribute>>(json);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        public static async Task ReloadWeaponsTemplates()
        {
            try
            {
                string wepJsonPath = AppDomain.CurrentDomain.BaseDirectory + "/Data/weapons.json";
                if (File.Exists(wepJsonPath))
                {
                    using (StreamReader sr = new StreamReader(wepJsonPath))
                    {
                        string json = await sr.ReadToEndAsync();
                        WeaponsTemplates = JsonConvert.DeserializeObject<List<Core.Classes.WeaponTemplate>>(json);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        public static async Task ReloadPlugins()
        {
            Core.Classes.PluginsPkg tmpPkg = new Core.Classes.PluginsPkg();
            if(Plugins != null)
                tmpPkg = Plugins.Clone();
            try
            {
                Plugins.Clear();
                string pluginsPath = AppDomain.CurrentDomain.BaseDirectory + "/Plugins/";
                if (!Directory.Exists(pluginsPath))
                    Directory.CreateDirectory(pluginsPath);
                string[] pluginsFiles = Directory.GetFiles(pluginsPath, "*.ffbeplugin", SearchOption.TopDirectoryOnly);
                for(int file = 0; file < pluginsFiles.Length; file++)
                {
                    try
                    {
                        using (StreamReader sr = new StreamReader(pluginsFiles[file]))
                        {
                            string json = await sr.ReadToEndAsync();
                            JObject jObj = JsonConvert.DeserializeObject<JObject>(json);
                            if (jObj["name"] == null)
                            {
                                System.Diagnostics.Debug.WriteLine(string.Format("{0} doesn't has a name!", pluginsFiles[file]));
                                continue;
                            }
                            string plugName = jObj["name"].ToString();
                            if (jObj["abilities"] != null && jObj["abilities"] is JArray abilities)
                            {
                                Core.Classes.PluginsPkg.AbilityPlugin abiPlugin = new Core.Classes.PluginsPkg.AbilityPlugin()
                                {
                                    PluginName = plugName
                                };
                                
                                for(int abi = 0; abi < abilities.Count; abi++)
                                {
                                    Core.Classes.AbilityTemplate template = abilities[abi].ToObject<Core.Classes.AbilityTemplate>();
                                    abiPlugin.AbilityTemplates.Add(template);
                                }

                                Plugins.AbilityPlugins.Add(abiPlugin);
                            }
                        }
                    } catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", pluginsFiles[file], ex.ToString()));
                    }
                }
            } catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                Plugins = tmpPkg;
            }
        }
    }
}
