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
    public class IsoCountries
    {
        public Dictionary<string, string> Countries { get; private set; } = new Dictionary<string, string>();
        private IsoCountries()
        {

        }

        public static IsoCountries Load()
        {
            var iso = new IsoCountries();

            var jobject = JObject.Parse(File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "json/iso3166.json")));
            foreach(var token in jobject)
            {
                iso.Countries.Add(token.Key, token.Value.ToString());
            }

            return iso;
        }
    }
}
