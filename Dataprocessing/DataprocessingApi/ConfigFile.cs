using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DataprocessingApi
{
    public class ConfigFile
    {
        private ConfigFile() { }

        [JsonProperty("dbhost")]
        public string DbHost { get; private set; } = "localhost";

        [JsonProperty("dbname")]
        public string DbName { get; private set; } = "dataprocessing";

        [JsonProperty("dbuser")]
        public string DbUser { get; private set; } = "root";

        [JsonProperty("dbpass")]
        public string DbPass { get; private set; } = "";

        public static ConfigFile Load()
        {
            if(!File.Exists("config.json"))
            {
                File.Create("config.json").Close();
                File.WriteAllText("config.json", JsonConvert.SerializeObject(new ConfigFile()));
            }

            var cfg = File.ReadAllText("config.json");
            return JsonConvert.DeserializeObject<ConfigFile>(cfg);
        }
    }
}
