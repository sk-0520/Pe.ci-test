using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.Base
{
    /// <summary>
    /// <see cref="KeyedCollection{TKey, TItem}"/>をラムダで対応する暫定クラス。
    /// </summary>
    /// <remarks>
    /// <para>クラス内で完結する場合のみに使用する前提、複数個所で使用する場合はちゃんと<see cref="KeyedCollection{TKey, TItem}"/>の実装を作成すべき。</para>
    /// </remarks>
    /// <typeparam name="TKey">コレクション内のキーの型。</typeparam>
    /// <typeparam name="TValue">コレクション内の項目の型。</typeparam>
    public sealed class SimpleKeyedCollection<TKey, TValue>: KeyedCollection<TKey, TValue>
        where TKey : notnull
    {
        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="toKey"><inheritdoc cref="ToKey"/></param>
        public SimpleKeyedCollection(Func<TValue, TKey> toKey)
        {
            ToKey = toKey;
        }

        /// <summary>
        /// 比較処理付きで生成。
        /// </summary>
        /// <param name="comparer">比較処理。</param>
        /// <param name="toKey"><inheritdoc cref="ToKey"/></param>
        public SimpleKeyedCollection(IEqualityComparer<TKey> comparer, Func<TValue, TKey> toKey)
            : base(comparer)
        {
            ToKey = toKey;
        }

        /// <summary>
        /// <inheritdoc cref="SimpleKeyedCollection{TKey, TValue}(IEqualityComparer{TKey}, Func{TValue, TKey})"/>
        /// </summary>
        /// <remarks>
        /// <para>閾値付き。</para>
        /// </remarks>
        /// <param name="comparer">比較処理。</param>
        /// <param name="dictionaryCreationThreshold"></param>
        /// <param name="toKey"><inheritdoc cref="ToKey"/></param>
        public SimpleKeyedCollection(IEqualityComparer<TKey> comparer, int dictionaryCreationThreshold, Func<TValue, TKey> toKey)
            : base(comparer, dictionaryCreationThreshold)
        {
            ToKey = toKey;
        }

        #region property

        /// <summary>
        /// <typeparamref name="TKey"/>から<typeparamref name="TValue"/>への変換処理。
        /// </summary>
        private Func<TValue, TKey> ToKey { get; }

        #endregion

        #region KeyedCollection

        protected override TKey GetKeyForItem(TValue item) => ToKey(item);

        #endregion
    }
}
