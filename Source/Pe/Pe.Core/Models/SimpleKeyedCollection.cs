using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ContentTypeTextNet.Pe.Core.Models
{
    /// <summary>
    /// <see cref="KeyedCollection{TKey, TItem}"/>をラムダで対応する暫定クラス。
    /// <para>クラス内で完結する場合のみに使用する前提、複数個所で使用する場合はちゃんと<see cref="KeyedCollection{TKey, TItem}"/>の実装を作成すべき。</para>
    /// </summary>
    /// <typeparam name="TKey">コレクション内のキーの型。</typeparam>
    /// <typeparam name="TValue">コレクション内の項目の型。</typeparam>
    public sealed class SimpleKeyedCollection<TKey, TValue>: KeyedCollection<TKey, TValue>
        where TKey : notnull
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

        private Func<TValue, TKey> ToKey { get; }

        #endregion

        #region KeyedCollection

        protected override TKey GetKeyForItem(TValue item) => ToKey(item);

        #endregion
    }

}
