using Moresu.Component.Profile;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Moresu.Component.Common
{
    class Configuration
    {
        public void Save()
        {
            File.WriteAllText(Path.Combine(Profiles.ProfilesDir, "config.json"), JsonConvert.SerializeObject(JObject.FromObject(this), Formatting.Indented));
        }

        public static Configuration Load()
        {
            if (File.Exists(Path.Combine(Profiles.ProfilesDir, "config.json")))
            {
                return JObject.Parse(File.ReadAllText(Path.Combine(Profiles.ProfilesDir, "config.json"))).ToObject<Configuration>();
            }
            return new Configuration();
        }
    }
}
