using System;
using System.Collections.Generic;
using System.Text;

namespace Moresu.Component.Client.Download
{
    class SayoSource : IClientDownloadSource
    {
        public string GetDisplayName()
        {
            return "Sayobot镜像下载源";
        }

        public string GetDownloadLink()
        {
            return "https://dl.sayobot.cn/osu.zip";
        }

        public string GetFileName()
        {
            return "osu!.zip";
        }

        public bool NeedUnzip()
        {
            return true;
        }
    }
}
