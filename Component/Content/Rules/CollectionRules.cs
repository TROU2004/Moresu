using OsuParsers.Database.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Moresu.Component.Content.Rules
{
    class CollectionRules : IEqualityComparer<Collection>
    {
        public bool Equals(Collection x, Collection y)
        {
            return x.Name == y.Name && x.MD5Hashes.Equals(y);
        }

        public int GetHashCode(Collection obj)
        {
            return obj.MD5Hashes.GetHashCode() + obj.Name.Length;
        }
    }
}
