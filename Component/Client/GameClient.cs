using Downloader;
using MaterialDesignThemes.Wpf;
using Moresu.Component.Client.Download;
using Moresu.Component.Domain;
using Moresu.Component.Profile;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Threading;

namespace Moresu.Component.Client
{
    class GameClient
    {
        public static readonly string ClientDir = Path.Combine(Directory.GetCurrentDirectory(), "client");
        public static readonly List<IClientDownloadSource> Sources = new List<IClientDownloadSource> 
        {
            new SayoSource(), 
            new PeppySource()
        };

        public static bool CheckClient()
        {
            if (!Directory.Exists(ClientDir) || !File.Exists(Path.Combine(ClientDir, "osu!.exe")))
            {
                Directory.CreateDirectory(ClientDir);
                return false;
            }
            return true;
        }

        public static void PrepareClient()
        {
            if (!CheckClient())
            {
                var dialog = new SectionDialog("选择客户端下载源", "Moresu的使用需要下载osu!客户端\n请从下面几个镜像源中选择一个源\n国内推荐使用Sayobot源, Peppy源将调用osu!installer下载");
                foreach (var source in Sources)
                {
                    dialog.AddSection(source.GetDisplayName(), (obj, args) =>
                    {
                        Host.Home.dialogHost_Root.IsOpen = false;
                        DownloadClient(source);
                    });
                }
                Host.Home.dialogHost_Root.ShowDialog(dialog);
            }
        }

        public static void DownloadClient(IClientDownloadSource source)
        {
            var downloader = new DownloadService();
            downloader.DownloadFileAsync(source.GetDownloadLink(), Path.Combine(ClientDir, source.GetFileName()));
            Host.Home.dialogHost_Root.ShowDialog(new ClientDownload(downloader, source.NeedUnzip()));
        }

        public static void BuildClient(Profile.Profile profile)
        {
            if (CheckClient())
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                profile.CollectionData.Operate();
                profile.BeatmapData.Operate();
                profile.ScoreData.Operate();
                Host.Home.Visibility = System.Windows.Visibility.Hidden;
                ClientGuard.RunOsuWithGuard(() =>
                {
                    profile.CollectionData.OperateReverse();
                    profile.BeatmapData.OperateReverse();
                    profile.ScoreData.OperateReverse();
                    stopWatch.Stop();
                    profile.AddPlayTime(stopWatch.Elapsed);
                    profile.ApplyToGlobal();
                    Host.Home.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                    {
                        Host.Close();
                    }));
                });
            }
        }
    }
}
