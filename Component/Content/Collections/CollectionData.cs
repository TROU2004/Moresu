using Moresu.Component.Content.Rules;
using Moresu.Component.Profile;
using OsuParsers.Database;
using OsuParsers.Database.Objects;
using OsuParsers.Decoders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Moresu.Component.Content.Collections
{
    public class CollectionData : AbstractOperation
    {
        private int OsuVersion;
        public List<Collection> Collections = new List<Collection>();

        public CollectionData(string name)
        {
            ProfileName = name;
        }

        public void ReadCollection()
        {
            var db = DatabaseDecoder.DecodeCollection(Path.Combine(Profiles.ProfilesDir, ProfileName, "collection.db"));
            OsuVersion = db.OsuVersion;
            Collections = db.Collections;
        }

        public void UnionCollection(List<Collection> yourlist)
        {
            var list = Collections.Union(yourlist, new CollectionRules()).ToList();
            var newList = new List<Collection>();
            bool doEffect;
            foreach (var item in list)
            {
                doEffect = true;
                foreach (var newItem in newList)
                {
                    if (item.Name == newItem.Name)
                    {
                        newItem.MD5Hashes.Union(item.MD5Hashes);
                        doEffect = false;
                    }
                }
                if (doEffect)
                {
                    newList.Add(item);
                }
            }
            foreach (var item in newList)
            {
                item.Count = item.MD5Hashes.Count;
            }
            Collections = newList;
        }

        public void SaveCollections()
        {
            new CollectionDatabase
            {
                CollectionCount = Collections.Count,
                OsuVersion = OsuVersion, 
                Collections = Collections
            }.Save(Path.Combine(Profiles.ProfilesDir, ProfileName, "collections.db"));
        }

        public override string GetFileName()
        {
            return "collection.db";
        }

        public override void CreateEmpty()
        {
            if (Independent)
            {
                new CollectionDatabase().Save(Path.Combine(Profiles.ProfilesDir, ProfileName, "collection.db"));
            }
        }
    }
}
