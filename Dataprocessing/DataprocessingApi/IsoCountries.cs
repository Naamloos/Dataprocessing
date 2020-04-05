using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DataprocessingApi
{
    /// <summary>
    /// Helper class to convert ISO country codes
    /// </summary>
    public class IsoCountries
    {
        /// <summary>
        /// List of countries
        /// </summary>
        public Dictionary<string, string> Countries { get; private set; } = new Dictionary<string, string>();
        private IsoCountries()
        {

        }

        /// <summary>
        /// Loads the iso 3166 json
        /// </summary>
        /// <returns>A new IsoCountries object</returns>
        public static IsoCountries Load()
        {
            var iso = new IsoCountries();

            // Because this is a json object and not an array we'll have to manually map this to a dictionary.
            var jobject = JObject.Parse(File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "json/iso3166.json")));
            foreach(var token in jobject)
            {
                iso.Countries.Add(token.Key, token.Value.ToString());
            }

            return iso;
        }
    }
}
