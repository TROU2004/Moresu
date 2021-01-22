using Downloader;
using MaterialDesignThemes.Wpf;
using MaterialXAMLDialogs;
using MaterialXAMLDialogs.Enums;
using Moresu.Component.Client.ClientBuild;
using Moresu.Component.Client.Download;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Threading;

namespace Moresu.Component.Client
{
    class GameClient
    {
        public static readonly string ClientDir = Path.Combine(Directory.GetCurrentDirectory(), "client");

        public static bool CheckClient()
        {
            if (!Directory.Exists(ClientDir) || !File.Exists(Path.Combine(ClientDir, "osu!.exe")))
            {
                Directory.CreateDirectory(ClientDir);
                return false;
            }
            return true;
        }

        public async static void PrepareClient()
        {
            if (!CheckClient())
            {
                var sayoSource = new SayoSource();
                var peppySource = new PeppySource();
                var config = new SelectionDialogConfiguration
                {
                    Title = "选择客户端下载源"
                };
                var dialog = new SelectionDialog<string>(config);
                var selectedItem = await dialog.Show("Root", new string[] { sayoSource.GetDisplayName(), peppySource.GetDisplayName() }, (s) => s);
                switch (selectedItem)
                {
                    case "Sayobot镜像下载源":
                        DownloadClient(sayoSource);
                        break;
                    case "Peppy官网下载源":
                        DownloadClient(peppySource);
                        break;
                }
            }
        }

        private static void DownloadClient(IClientDownloadSource source)
        {
            var downloader = new DownloadService();
            downloader.DownloadFileAsync(source.GetDownloadLink(), Path.Combine(ClientDir, source.GetFileName()));
            new ClientDownload(downloader, source.NeedUnzip()).Show();
        }

        public static void BuildClient(BuildProperties properties)
        {
            if (CheckClient())
            {
                properties.DoAllOperates();
                Host.Home.Visibility = System.Windows.Visibility.Hidden;
                ClientGuard.RunOsuWithGuard(() =>
                {
                    properties.DoAllOperatesReverse();
                    Host.Home.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                    {
                        Host.Home.Close();
                    }));
                });
            }
        }
    }
}
