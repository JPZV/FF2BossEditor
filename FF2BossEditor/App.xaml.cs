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
        protected override async void OnStartup(StartupEventArgs e)
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
            base.OnStartup(e);
        }
    }
}
