using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DataprocessingApi
{
    /// <summary>
    /// Represents a Config file.
    /// </summary>
    public class ConfigFile
    {
        private ConfigFile() { }

        /// <summary>
        /// Database Host
        /// </summary>
        [JsonProperty("dbhost")]
        public string DbHost { get; private set; } = "localhost";

        /// <summary>
        /// Database Name
        /// </summary>
        [JsonProperty("dbname")]
        public string DbName { get; private set; } = "dataprocessing";

        /// <summary>
        /// Database user
        /// </summary>
        [JsonProperty("dbuser")]
        public string DbUser { get; private set; } = "root";

        /// <summary>
        /// Database password
        /// </summary>
        [JsonProperty("dbpass")]
        public string DbPass { get; private set; } = "";

        /// <summary>
        /// Loads the config file.
        /// </summary>
        /// <returns></returns>
        public static ConfigFile Load()
        {
            if(!File.Exists("config.json"))
            {
                File.Create("config.json").Close();
                File.WriteAllText("config.json", JsonConvert.SerializeObject(new ConfigFile()));
                Console.WriteLine("New config generated. Please fill in your config and restart.");
                Console.WriteLine("Press any button (not the on/off button or a mouse button) to exit.");
                Console.ReadKey();
                Environment.Exit(0);
            }

            var cfg = File.ReadAllText("config.json");
            return JsonConvert.DeserializeObject<ConfigFile>(cfg);
        }
    }
}
