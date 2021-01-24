using MaterialDesignThemes.Wpf;
using Moresu.Component.Client;
using Moresu.Component.Common;
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
        public static Configuration Config;
        public static readonly string HostDir = Directory.GetCurrentDirectory();

        public static void StartupPrepare()
        {
            if (!Directory.Exists(Profiles.ProfilesDir))
            {
                Directory.CreateDirectory(Profiles.ProfilesDir);
            }
            Config = Configuration.Load();
            GameClient.PrepareClient();
            Profiles.LoadAllProfiles();
        }

        public static void ShowEasyDialog(string message)
        {
            Home.dialogHost_Root.IsOpen = false;
            Home.dialogHost_Root.ShowDialog(new EasyDialog(message));
        }

        internal static void Close()
        {
            Config.Save();
            Home.Close();
        }
    }
}
