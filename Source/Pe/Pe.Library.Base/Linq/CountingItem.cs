using System.Collections.Generic;

namespace ContentTypeTextNet.Pe.Library.Base.Linq
{
    /// <summary>
    /// シーケンスの列挙中インデックス・データを保持する。
    /// </summary>
    /// <typeparam name="TNumber"></typeparam>
    /// <typeparam name="TElement"></typeparam>
    public readonly struct CountingItem<TNumber, TElement>
        where TNumber : struct
    {
        internal CountingItem(TNumber number, TElement element)
        {
            Number = number;
            Value = element;
        }

        #region property

        /// <summary>
        /// シーケンス値。
        /// </summary>
        /// <remarks>
        /// <para><see cref="IEnumerableExtensions.Counting{TElement}(IEnumerable{TElement}, int)"/>の基点からの加算値。</para>
        /// </remarks>
        public readonly TNumber Number { get; }
        /// <summary>
        /// 現在の値。
        /// </summary>
        public readonly TElement Value { get; }

        #endregion
    }
}
