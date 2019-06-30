using Newtonsoft.Json;
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
        protected override async void OnStartup(StartupEventArgs e)
        {
            await ReloadWeaponsAttributes();
            await ReloadWeaponsTemplates();
            
            base.OnStartup(e);
        }

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
    }
}
