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
    public class ScoreData : AbstractOperation
    {
        private int OsuVersion;
        public List<Score> Scores = new List<Score>();

        public ScoreData(string name)
        {
            ProfileName = name;
        }

        public void ReadScore()
        {
            var db = DatabaseDecoder.DecodeScores(Path.Combine(Profiles.ProfilesDir, ProfileName, "scores.db"));
            OsuVersion = db.OsuVersion;
            foreach (var item in db.Scores)
            {
                Scores.AddRange(item.Item2);
            }
        }

        public void UnionScore(List<Score> list)
        {
            Scores = Scores.Union(list, new ScoreRules()).ToList();
        }

        public void SortScore(IComparer<Score> comparer)
        {
            Scores.Sort(comparer);
        }

        public void SaveScores()
        {
            var list = new List<Tuple<string, List<Score>>>();
            bool doEffect;
            foreach (var score in Scores)
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
            }.Save(Path.Combine(Profiles.ProfilesDir, ProfileName, "scores.db"));
        }

        public override string GetFileName()
        {
            return "scores.db";
        }

        public override void CreateEmpty()
        {
            if (Independent)
            {
                var scores = new ScoresDatabase
                {
                    OsuVersion = 20210108, //Lasy but should not cause a problem i think
                    Scores = new List<Tuple<string, List<OsuParsers.Database.Objects.Score>>>()
                };
                scores.Save(Path.Combine(Profiles.ProfilesDir, ProfileName, "scores.db"));
            }
        }
    }
}
