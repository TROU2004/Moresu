using MaterialDesignThemes.Wpf;
using Moresu.Component.Client;
using Moresu.Component.Content.Beatmaps;
using Moresu.Component.Content.Collections;
using Moresu.Component.Content.Scores;
using Moresu.Component.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Path = System.IO.Path;

namespace Moresu.Component.Profile
{
    /// <summary>
    /// Profile.xaml 的交互逻辑
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public partial class Profile : UserControl
    {
        [JsonProperty]
        public string ProfileName { get; private set; }
        [JsonProperty]
        public string DisplayName;
        [JsonProperty]
        public DateTime CreateTime;
        [JsonProperty]
        private TimeSpan PlayTimeSpan = new TimeSpan();
        [JsonProperty]
        public ScoreData ScoreData;
        [JsonProperty]
        public BeatmapData BeatmapData;
        [JsonProperty]
        public CollectionData CollectionData;

        private Profile()
        {
            InitializeComponent();
        }

        public Profile(string name)
        {
            InitializeComponent();
            ProfileName = name;
            ScoreData = new ScoreData(name);
            BeatmapData = new BeatmapData(name);
            CollectionData = new CollectionData(name);
        }

        public void Save()
        {
            if (!Directory.Exists(Path.Combine(Profiles.ProfilesDir, ProfileName)))
            {
                Directory.CreateDirectory(Path.Combine(Profiles.ProfilesDir, ProfileName));
            }
            File.WriteAllText(Path.Combine(Profiles.ProfilesDir, ProfileName, "profile.json"), JsonConvert.SerializeObject(JObject.FromObject(this), Formatting.Indented));
        }

        public static Profile CreateProfile(string name)
        {
            foreach (var profile1 in Profiles.ProfileList)
            {
                if (profile1.ProfileName == name)
                {
                    Host.ShowEasyDialog("该名称的配置档已存在");
                    return null;
                }
            }
            if (name != "")
            {
                var profile = new Profile
                {
                    ProfileName = name
                };
                profile.SetDisplayName(name);
                Profiles.ProfileList.Add(profile);
                Host.Home.wrapPanel_Items.Children.Add(profile);
                profile.Save();
                profile.ScoreData = new ScoreData(name);
                profile.BeatmapData = new BeatmapData(name);
                profile.CollectionData = new CollectionData(name);
                return profile;
            }
            return null;
        }

        public void SetDisplayName(string name)
        {
            DisplayName = name;
            textBlock_ProfileName.Text = name;
        }

        public void Rename(string name)
        {
            SetDisplayName(name);
            Save();
        }

        public void AddPlayTime(TimeSpan timeSpan)
        {
            if (ProfileName != "global")
            {
                Profiles.GetProfileFromName("global").AddPlayTime(timeSpan);
            }
            PlayTimeSpan = PlayTimeSpan.Add(timeSpan);
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                textBlock_PlayTime.Text = ((int)Math.Ceiling(PlayTimeSpan.TotalHours)).ToString() + "小时";
            }));
            Save();
        }

        public void ApplyToGlobal()
        {
            if (ProfileName != "global")
            {
                var global = Profiles.GetProfileFromName("global");
                BeatmapData.ReadBeatmap();
                ScoreData.ReadScore();
                global.BeatmapData.ReadBeatmap();
                global.ScoreData.ReadScore();
                global.BeatmapData.UnionBeatmap(BeatmapData.Beatmaps);
                global.ScoreData.UnionScore(ScoreData.Scores);
                global.BeatmapData.SaveBeatmaps();
                global.ScoreData.SaveScores();
            }
        }

        public void Remove()
        {
            Host.Home.dialogHost_Root.ShowDialog(new DualButtonDialog("确定要删除" + ProfileName + (ProfileName == DisplayName ? "" : "(" + DisplayName + ")") + "吗?", () =>
            {
                try
                {
                    Host.Home.wrapPanel_Items.Children.Remove(this);
                    Directory.Delete(Path.Combine(Profiles.ProfilesDir, ProfileName), true);
                    Profiles.ProfileList.Remove(this);
                }
                catch (Exception e)
                {
                    Host.ShowEasyDialog("删除配置档" + ProfileName + "时出现错误\n" + e.Message);
                }
            }));
        }

        public static Profile LoadProfile(string path)
        {
            if (File.Exists(Path.Combine(path)))
            {
                var profile = JObject.Parse(File.ReadAllText(Path.Combine(path))).ToObject<Profile>();
                profile.textBlock_ProfileName.Text = profile.DisplayName;
                if (profile.ProfileName == "global")
                {
                    profile.dockPanel_Controls.IsEnabled = false;
                    Host.Home.wrapPanel_Items.Children.Insert(0, profile);
                }
                else
                {
                    Host.Home.wrapPanel_Items.Children.Add(profile);
                }
                return profile;
            }
            return null;
        }

        private void Button_Rename_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new TextboxDialog("请输入新名字: ", (str) =>
            {
                Rename(str);
            });
            Host.Home.dialogHost_Root.ShowDialog(dialog);
        }

        private void Button_Remove_Click(object sender, RoutedEventArgs e)
        {
            Remove();
        }

        private void Button_Start_Click(object sender, RoutedEventArgs e)
        {
            if (ClientGuard.GetOsuRunningState())
            {
                Host.ShowEasyDialog("已经有一个osu!正在运行");
            }
            else
            {
                GameClient.BuildClient(this);
            }
        }
    }
}
