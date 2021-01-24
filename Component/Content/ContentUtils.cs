using Moresu.Component.Profile;
using OsuParsers.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Moresu.Component.Content
{
    class ContentUtils
    {
        public static bool HasGlobalContents()
        {
            return File.Exists(Path.Combine(Profiles.ProfilesDir, "osu!.db"));
        }
    }
}
