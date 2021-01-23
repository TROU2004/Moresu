using OsuParsers.Database.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Moresu.Component.Beatmap.Database
{
    public class ScoreRules : IEqualityComparer<Score>
    {
        public bool Equals(Score x, Score y)
        {
            return x.ReplayMD5Hash == y.ReplayMD5Hash && x.ReplayScore == y.ReplayScore && x.ScoreTimestamp == y.ScoreTimestamp;
        }

        public int GetHashCode(Score obj)
        {
            return obj.BeatmapMD5Hash.GetHashCode();
        }
    }
}
