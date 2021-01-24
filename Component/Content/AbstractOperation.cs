using Moresu.Component.Client;
using Moresu.Component.Profile;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Moresu.Component.Content
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class AbstractOperation
    {
        [JsonProperty]
        public string ProfileName;
        public abstract string GetFileName();
        public abstract void CreateEmpty();
        [JsonProperty]
        public bool Independent = true;



        public void Operate()
        {
            var a = Independent ? Path.Combine(Profiles.ProfilesDir, ProfileName, GetFileName()) : Path.Combine(Profiles.ProfilesDir, "global", GetFileName());
            var b = Path.Combine(GameClient.ClientDir, GetFileName());
            if (File.Exists(b)) File.Delete(b);
            File.Copy(a, b);
        }

        public void OperateReverse()
        {
            var a = Independent ? Path.Combine(Profiles.ProfilesDir, ProfileName, GetFileName()) : Path.Combine(Profiles.ProfilesDir, "global", GetFileName());
            var b = Path.Combine(GameClient.ClientDir, GetFileName());
            if (File.Exists(a)) File.Delete(a);
            File.Move(b, a);
        }
    }
}
