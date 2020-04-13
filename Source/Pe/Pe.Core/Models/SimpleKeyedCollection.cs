using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public class SimpleKeyedCollection<TKey, TValue> : KeyedCollection<TKey, TValue>
    {
        public SimpleKeyedCollection(Func<TValue, TKey> toKey)
        {
            ToKey = toKey;
        }

        public SimpleKeyedCollection(IEqualityComparer<TKey> comparer, Func<TValue, TKey> toKey)
            : base(comparer)
        {
            ToKey = toKey;
        }

        public SimpleKeyedCollection(IEqualityComparer<TKey> comparer, int dictionaryCreationThreshold, Func<TValue, TKey> toKey)
            : base(comparer, dictionaryCreationThreshold)
        {
            ToKey = toKey;
        }

        #region property

        Func<TValue, TKey> ToKey { get; }

        #endregion

        #region KeyedCollection

        protected override TKey GetKeyForItem(TValue item) => ToKey(item);

        #endregion
    }

}
