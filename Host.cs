using MaterialDesignThemes.Wpf;
using Moresu.Component.Client;
using Moresu.Component.Client.ClientBuild;
using Moresu.Component.Client.Download;
using Moresu.Component.Domain;
using Moresu.Component.Profile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Moresu
{
    class Host
    {
        public static Home Home;
        public static readonly string HostDir = Directory.GetCurrentDirectory();

        public static void StartupPrepare()
        {
            if (!Directory.Exists(Profiles.ProfilesDir))
            {
                Directory.CreateDirectory(Profiles.ProfilesDir);
            }
            GameClient.PrepareClient();
            Profiles.LoadAllProfiles();
        }

        public static void ShowEasyDialog(string message)
        {
            Home.dialogHost_Root.IsOpen = false;
            Home.dialogHost_Root.ShowDialog(new EasyDialog(message));
        }
    }
}
