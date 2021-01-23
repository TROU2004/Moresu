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

namespace Moresu.Component.Content.Scores
{
    class ScoreTool
    {
        private static int OsuVersion;

        public static List<Score> ReadScore(Profile.Profile profile)
        {
            var db = DatabaseDecoder.DecodeScores(Path.Combine(Profiles.ProfilesDir, profile.ProfileName, "scores.db"));
            OsuVersion = db.OsuVersion;
            var scores = new List<Score>();
            foreach (var item in db.Scores)
            {
                scores.AddRange(item.Item2);
            }
            return scores;
        }

        public static List<Score> UnionScore(List<Score> list1, List<Score> list2)
        {
            return list1.Union(list2, new ScoreRules()).ToList();
        }

        public static void SortScore(List<Score> scores, IComparer<Score> comparer)
        {
            scores.Sort(comparer);
        }

        public static void SaveScores(Profile.Profile profile, List<Score> scores)
        {
            var list = new List<Tuple<string, List<Score>>>();
            bool doEffect;
            foreach (var score in scores)
            {
                doEffect = true;
                foreach (var item in list)
                {
                    if (item.Item1 == score.BeatmapMD5Hash)
                    {
                        item.Item2.Add(score);
                        doEffect = false;
                    }
                }
                if (doEffect)
                {
                    var tuple = new Tuple<string, List<Score>>(score.BeatmapMD5Hash, new List<Score> { score });
                    list.Add(tuple);
                }
            }
            new ScoresDatabase
            {
                Scores = list,
                OsuVersion = OsuVersion
            }.Save(Path.Combine(Profiles.ProfilesDir, profile.ProfileName, "scores.db"));
        }
    }
}
