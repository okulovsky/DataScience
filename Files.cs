using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace DataScience
{
    public class Json
    {
        public static T Read<T>(string filename)
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(filename));
        }

        public static void Write(string filename, object t)
        {
            File.WriteAllText(filename, JsonConvert.SerializeObject(t, Newtonsoft.Json.Formatting.Indented));
        }
    }

    public class Bin
    {
        public static T Read<T>(string filename)
        {
            using (var file = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                return (T)new BinaryFormatter().Deserialize(file);
        }

        public static T Read<T>(string filename, Func<T> typeDefinition)
        {
            return Read<T>(filename);
        }

        public static void Write(string filename, object t)
        {
            using (var file = File.Open(filename, FileMode.OpenOrCreate, FileAccess.Write))
                new BinaryFormatter().Serialize(file, t);

        }
    }

    
}
