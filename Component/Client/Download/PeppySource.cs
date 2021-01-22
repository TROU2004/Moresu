using System;
using System.Collections.Generic;
using System.Text;

namespace Moresu.Component.Client.Download
{
    class PeppySource : IClientDownloadSource
    {
        public string GetDisplayName()
        {
            return "Peppy官网下载源";
        }

        public string GetDownloadLink()
        {
            return "https://m1.ppy.sh/r/osu!.exe";
        }

        public string GetFileName()
        {
            return "osu!.exe";
        }

        public bool NeedUnzip()
        {
            return false;
        }
    }
}
