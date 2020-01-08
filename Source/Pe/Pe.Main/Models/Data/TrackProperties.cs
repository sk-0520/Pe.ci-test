using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    internal class TrackProperties : Dictionary<string, string>
    {
        public TrackProperties()
        {
        }

        public TrackProperties(IDictionary<string, string> dictionary) : base(dictionary)
        {
        }

        public TrackProperties(IEnumerable<KeyValuePair<string, string>> collection) : base(collection)
        {
        }

        public TrackProperties(IEqualityComparer<string>? comparer) : base(comparer)
        {
        }

        #region function
        #endregion
    }
}
