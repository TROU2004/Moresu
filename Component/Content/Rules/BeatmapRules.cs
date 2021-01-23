using OsuParsers.Database.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Moresu.Component.Beatmap.Database
{
    class BeatmapRules : IEqualityComparer<DbBeatmap>
    {
        public bool Equals(DbBeatmap x, DbBeatmap y)
        {
            return x.BeatmapId == y.BeatmapId;
        }

        public int GetHashCode(DbBeatmap obj)
        {
            return obj.BeatmapId.GetHashCode();
        }
    }
}
