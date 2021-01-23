using Moresu.Component.Client.ClientBuild;
using OsuParsers.Database;
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
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace Moresu.Component.Profile
{
    /// <summary>
    /// ProfileCreate.xaml 的交互逻辑
    /// </summary>
    public partial class ProfileCreate : UserControl
    {
        private readonly DateTime CreateTime;

        public ProfileCreate()
        {
            InitializeComponent();
            CreateTime = DateTime.Now;
            textBox_createTime.Text = CreateTime.ToString();
        }

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            var profile = Profile.CreateProfile(textBox_profileName.Text);
            if (profile != null)
            {
                profile.CreateTime = CreateTime;
                profile.PlayTimeSpan = new TimeSpan();
                profile.BuildProperties = GetProperties();
                profile.Save();
                File.Create(Path.Combine(Profiles.ProfilesDir, profile.ProfileName, "osu!." + Environment.UserName + ".cfg")).Close();
                new OsuDatabase().Save(Path.Combine(Profiles.ProfilesDir, profile.ProfileName,"osu!.db"));
                new ScoresDatabase().Save(Path.Combine(Profiles.ProfilesDir, profile.ProfileName, "scores.db"));
                new CollectionDatabase().Save(Path.Combine(Profiles.ProfilesDir, profile.ProfileName, "collection.db"));
            }
            
        }

        private BuildProperties GetProperties()
        {
            var properties = new BuildProperties();
            if ((bool)checkBox_osudb.IsChecked)
            {
                properties.AddOperate(new FileOperate(textBox_profileName.Text, "osu!.db", false, true));
            }
            if ((bool)checkBox_scoresdb.IsChecked)
            {
                properties.AddOperate(new FileOperate(textBox_profileName.Text, "scores.db", false, true));
            }
            if ((bool)checkBox_collectiondb.IsChecked)
            {
                properties.AddOperate(new FileOperate(textBox_profileName.Text, "collection.db", false, true));
            }
            if ((bool)checkBox_osudb.IsChecked)
            {
                properties.AddOperate(new FileOperate(textBox_profileName.Text, "osu!." + Environment.UserName + ".cfg", false, true));
            }
            return properties;
        }
    }
}
