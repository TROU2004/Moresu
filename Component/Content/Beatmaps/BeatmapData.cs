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
    public class BeatmapData : AbstractOperation
    {
        private OsuDatabase OldDatabase;
        public List<DbBeatmap> Beatmaps = new List<DbBeatmap>();

        public BeatmapData(string name)
        {
            ProfileName = name;
        }

        public void ReadBeatmap()
        {
            OldDatabase = DatabaseDecoder.DecodeOsu(Path.Combine(Profiles.ProfilesDir, ProfileName, "osu!.db"));
            Beatmaps = OldDatabase.Beatmaps;
        }

        public void UnionBeatmap(List<DbBeatmap> list)
        {
            Beatmaps = Beatmaps.Union(list, new BeatmapRules()).ToList();
        }

        public void SaveBeatmaps()
        {
            OldDatabase.Beatmaps = Beatmaps;
            OldDatabase.BeatmapCount = Beatmaps.Count;
            var ids = new List<string>();
            foreach (var item in Beatmaps)
            {
                ids.Add(item.FolderName);
            }
            OldDatabase.FolderCount = ids.Distinct().Count();
            OldDatabase.Save(Path.Combine(Profiles.ProfilesDir, ProfileName, "osu!.db"));
        }

        public override string GetFileName()
        {
            return "osu!.db";
        }

        public override void CreateEmpty()
        {
            if (Independent)
            {
                new OsuDatabase().Save(Path.Combine(Profiles.ProfilesDir, ProfileName, "osu!.db"));
            }
        }
    }
}
