using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF2BossEditor.Core
{
    public class StorageCore<T>
    {
        public static async Task<T> GenericGetObject(string DefaultExt, string ExtFilter)
        {
            Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog()
            {
                DefaultExt = DefaultExt,
                Filter = ExtFilter
            };

            bool? openResult = openDialog.ShowDialog();
            if (openResult == true)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(openDialog.FileName))
                    {
                        string json = await sr.ReadToEndAsync();
                        T openObj = JsonConvert.DeserializeObject<T>(json);
                        return openObj;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            }
            return default;
        }

        public static async Task<bool> GenericSaveObject(T Obj, string DefaultExt, string ExtFilter)
        {
            Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog()
            {
                DefaultExt = DefaultExt,
                Filter = ExtFilter
            };

            bool? saveResult = saveDialog.ShowDialog();
            if (saveResult == true)
            {
                try
                {
                    using (var file = File.CreateText(saveDialog.FileName))
                    {
                        string json = JsonConvert.SerializeObject(Obj, Formatting.Indented);
                        await file.WriteAsync(json);
                        return true;
                    }
                } catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            }
            return false;
        }
    }
}
