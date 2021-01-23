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
    class CollectionTool
    {
        private static int OsuVersion;

        public static List<Collection> ReadCollection(Profile.Profile profile)
        {
            var db = DatabaseDecoder.DecodeCollection(Path.Combine(Profiles.ProfilesDir, profile.ProfileName, "collection.db"));
            OsuVersion = db.OsuVersion;
            return db.Collections;
        }

        public static List<Collection> UnionCollection(List<Collection> list1, List<Collection> list2)
        {
            var list = list1.Union(list2, new CollectionRules()).ToList();
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
            return newList;
        }

        public static void SaveCollections(Profile.Profile profile, List<Collection> collections)
        {

            new CollectionDatabase
            {
                CollectionCount = collections.Count,
                OsuVersion = OsuVersion, 
                Collections = collections
            }.Save(Path.Combine(Profiles.ProfilesDir, profile.ProfileName, "collections.db"));
        }
    }
}
