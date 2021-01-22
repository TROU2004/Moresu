using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace Moresu.Component.Profile
{
    /// <summary>
    /// Profile.xaml 的交互逻辑
    /// </summary>
    public partial class Profile : UserControl
    {
        public string ProfileName { get; set; }
        public readonly DateTime CreateTime;
        public int PlayTime { get; set; }

        private Profile()
        {
            InitializeComponent();
        }

        private void Save()
        {
            if (!Directory.Exists(Path.Combine(Profiles.ProfilesDir, ProfileName)))
            {
                Directory.CreateDirectory(Path.Combine(Profiles.ProfilesDir, ProfileName));
            }
            File.WriteAllText(Path.Combine(Profiles.ProfilesDir, ProfileName, "profile.json"), JsonConvert.SerializeObject(JObject.FromObject(this), Formatting.Indented));
        }

        public static Profile CreateProfile(string name)
        {
            var profile = new Profile
            {
                ProfileName = name
            };
            profile.Save();
            Profiles.ProfileList.Add(profile);
            return profile;
        }

        public static Profile LoadProfile(string path)
        {
            if (File.Exists(Path.Combine(path)))
            {
                return JObject.Parse(File.ReadAllText(Path.Combine(path))).ToObject<Profile>();
            }
            return null;
        }
    }
}
