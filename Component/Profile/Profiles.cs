using Moresu.Component.Content;
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
            if (!HasProfile("global"))
            {
                ConstructGlobalProfile();
            }
        }

        private static void ConstructGlobalProfile()
        {
            Directory.CreateDirectory(Path.Combine(ProfilesDir, "global"));
            var profile = new Profile("global");
            profile.dockPanel_Controls.IsEnabled = false;
            profile.SetDisplayName("全局配置档");
            profile.BeatmapData.CreateEmpty();
            profile.ScoreData.CreateEmpty();
            profile.CollectionData.CreateEmpty();
            Host.Home.wrapPanel_Items.Children.Insert(0, profile);
            profile.Save();
        }
    }
}
