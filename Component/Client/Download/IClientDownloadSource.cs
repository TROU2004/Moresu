using System;
using System.Collections.Generic;
using System.Text;

namespace Moresu.Component.Client.Download
{
    interface IClientDownloadSource
    {
        string GetDownloadLink();
        bool NeedUnzip();
        string GetFileName();
        string GetDisplayName();
    }
}
