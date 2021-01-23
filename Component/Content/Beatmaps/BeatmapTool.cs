using Moresu.Component.Beatmap.Database;
using Moresu.Component.Profile;
using OsuParsers.Database;
using OsuParsers.Database.Objects;
using OsuParsers.Decoders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Moresu.Component.Content.Beatmaps
{
    class BeatmapTool
    {
        private static OsuDatabase OldDatabase; //Should not appear private static, it's unsafe, but we don't care here

        public static List<DbBeatmap> ReadBeatmap(Profile.Profile profile)
        {
            OldDatabase = DatabaseDecoder.DecodeOsu(Path.Combine(Profiles.ProfilesDir, profile.ProfileName, "osu!.db"));
            return OldDatabase.Beatmaps;
        }

        public static List<DbBeatmap> UnionBeatmap(List<DbBeatmap> list1, List<DbBeatmap> list2)
        {
            return list1.Union(list2, new BeatmapRules()).ToList();
        }

        public static void SaveBeatmaps(Profile.Profile profile, List<DbBeatmap> beatmaps)
        {
            OldDatabase.Beatmaps = beatmaps;
            OldDatabase.BeatmapCount = beatmaps.Count;
            var ids = new List<string>();
            foreach (var item in beatmaps)
            {
                ids.Add(item.FolderName);
            }
            OldDatabase.FolderCount = ids.Distinct().Count();
            OldDatabase.Save(Path.Combine(Profiles.ProfilesDir, profile.ProfileName, "osu!.db"));
        }
    }
}
