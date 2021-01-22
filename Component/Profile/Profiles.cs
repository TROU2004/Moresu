using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Moresu.Component.Profile
{
    class Profiles
    {
        public static readonly string ProfilesDir = Path.Combine(Directory.GetCurrentDirectory(), "profile");
        public static List<Profile> ProfileList = new List<Profile>();

        public static Profile GetProfileFromName(string name)
        {
            foreach (var profile in ProfileList)
            {
                if (profile.ProfileName == name)
                {
                    return profile;
                }
            }
            return null;
        }

        public static bool HasProfile(string name)
        {
            return GetProfileFromName(name) != null;
        }

        public static void LoadAllProfiles()
        {
            foreach (var path in Directory.GetDirectories(ProfilesDir))
            {
                ProfileList.Add(Profile.LoadProfile(Path.Combine(path, "profile.json")));
            }
        }
    }
}
