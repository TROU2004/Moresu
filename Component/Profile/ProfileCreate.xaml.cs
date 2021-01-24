using OsuParsers.Database;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

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
            Regex regex = new Regex(@"^([a-zA-Z]:\\)?[^\/\:\*\?\""\<\>\|\,]*$");
            if (!regex.Match(textBox_profileName.Text).Success)
            {
                Host.ShowEasyDialog("文件名非法 >_< \n如果你觉得合法的话可以匹配以下正则: \n" + @"^([a-zA-Z]:\\)?[^\/\:\*\?\""\<\>\|\,]*$");
                return;
            }
            var profile = Profile.CreateProfile(textBox_profileName.Text);
            if (profile != null)
            {
                profile.CreateTime = CreateTime;
                ProcessData(profile);
                profile.Save();
            }
        }

        private void ProcessData(Profile profile)
        {
            profile.BeatmapData.Independent = (bool)checkBox_osudb.IsChecked;
            profile.ScoreData.Independent = (bool)checkBox_scoresdb.IsChecked;
            profile.CollectionData.Independent = (bool)checkBox_collectiondb.IsChecked;
            profile.BeatmapData.CreateEmpty();
            profile.ScoreData.CreateEmpty();
            profile.CollectionData.CreateEmpty();
        }
    }
}
