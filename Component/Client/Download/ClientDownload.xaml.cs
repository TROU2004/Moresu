using Downloader;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
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

namespace Moresu.Component.Client.Download
{
    /// <summary>
    /// ClientDownload.xaml 的交互逻辑
    /// </summary>
    public partial class ClientDownload : UserControl
    {
        public bool NeedUnzip { get; set; }
        public DownloadService Downloader { get; set; }

        public ClientDownload(DownloadService downloader,bool needUnzip)
        {
            InitializeComponent();
            NeedUnzip = needUnzip;
            Downloader = downloader;
            textBlock_Link.Text = "从" + downloader.Package.Address.AbsoluteUri + "获取文件中";
            downloader.DownloadProgressChanged += Downloader_DownloadProgressChanged;
            downloader.DownloadFileCompleted += Downloader_DownloadFileCompleted;
        }

        private void Downloader_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (NeedUnzip)
            {
                new Thread(new ThreadStart(() =>
                {
                    ClientDL.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                    {
                        textBlock_Link.Text = "从" + Downloader.Package.FileName + "处解压中";
                        progressBar.IsIndeterminate = true;
                        textBlock_State.Text = "解压中...";
                    }));
                    Stream stream = File.Open(Downloader.Package.FileName, FileMode.Open);
                    new ZipArchive(stream).ExtractToDirectory(GameClient.ClientDir);
                    stream.Close();
                    File.Delete(Downloader.Package.FileName);
                    Host.Home.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        Host.Home.dialogHost_Root.IsOpen = false;
                    }));
                })).Start();
            }
        }

        private void Downloader_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            ClientDL.Dispatcher.BeginInvoke(new Action(() =>
            {
                progressBar.Value = e.ProgressPercentage;
                textBlock_State.Text = Math.Round((double)e.BytesPerSecondSpeed / 1024, 2).ToString() + "KB/s";
            }));
        }
    }
}
