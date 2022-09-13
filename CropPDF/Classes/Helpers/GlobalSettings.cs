using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CropPDF.Classes.Helpers
{
    public static class GlobalSettings
    {
        private static readonly string filePath;
        private static Dictionary<string, object> properties = new Dictionary<string, object>();

        static GlobalSettings()
        {
            filePath = Path.Combine(Environment.CurrentDirectory, "settings.json");
            properties = new Dictionary<string, object>();
            if (File.Exists(filePath))
            {
                try
                {
                    using (var sw = new StreamReader(filePath, false))
                    {
                        properties = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(sw.ReadToEnd());
                    }
                }
                catch
                {

                }
            }
        }

        public static void Set<T>(string propertyName, T value) => InternalSet(propertyName, value);
        public static T Get<T>(string propertyName, T defaultValue = default, bool requireValue = false) => InternalGet(propertyName, defaultValue, requireValue);

        private static void InternalSet<T>(string propertyName, T value)
        {
            try
            {
                properties[propertyName] = value;
                using (var sw = new StreamWriter(filePath, false))
                {
                    sw.Write(Newtonsoft.Json.JsonConvert.SerializeObject(properties));
                }
            }
            catch
            {

            }
        }

        private static T InternalGet<T>(string propertyName, T defaultValue = default, bool requireValue = false)
        {
            try
            {
                if (!properties.ContainsKey(propertyName))
                {
                    InternalSet(propertyName, defaultValue);
                }

                return (T)Convert.ChangeType(properties[propertyName], typeof(T));
            }
            catch (Exception ex)
            {
                InternalSet(propertyName, defaultValue);
                return defaultValue;
            }
        }
    }
}
