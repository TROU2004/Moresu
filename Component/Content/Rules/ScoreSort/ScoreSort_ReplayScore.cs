using OsuParsers.Database.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Moresu.Component.Content.Rules.ScoreSort
{
    class ScoreSort_ReplayScore : IComparer<Score>
    {
        public int Compare(Score x, Score y)
        {
            return y.ReplayScore.CompareTo(x.ReplayScore);
        }
    }
}
