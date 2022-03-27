using System.Collections.Generic;
using System.Linq;

namespace ContentTypeTextNet.Pe.Core.Models
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
        /// <para><see cref="IEnumerableExtensions.Counting{TElement}(IEnumerable{TElement}, int)"/>の基点からの加算値。</para>
        /// </summary>
        public readonly TNumber Number { get; }
        /// <summary>
        /// 現在の値。
        /// </summary>
        public readonly TElement Value { get; }

        #endregion
    }

    /// <summary>
    /// ふわふわ LINQ。
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// <paramref name="splitCount"/>数で分割する。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="splitCount"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> GroupSplit<T>(this IEnumerable<T> source, int splitCount)
        {
            return source
                .Counting()
                .GroupBy(i => i.Number / splitCount)
                .Select(g => g.Select(i => i.Value))
            ;
        }

        /// <summary>
        /// 0基点のインデックスと値ペア列挙。
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<CountingItem<int, TElement>> Counting<TElement>(this IEnumerable<TElement> source)
        {
            return source.Select((v, i) => new CountingItem<int, TElement>(i, v));
        }
        /// <summary>
        /// 独自基点のインデックスと値ペア列挙。
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source"></param>
        /// <param name="baseNumber">基点。</param>
        /// <returns></returns>
        public static IEnumerable<CountingItem<int, TElement>> Counting<TElement>(this IEnumerable<TElement> source, int baseNumber)
        {
            return source.Select((v, i) => new CountingItem<int, TElement>(i + baseNumber, v));
        }

        /// <summary>
        /// 入力シーケンスを結合した文字列を返す。
        /// <para><see cref="string.Join"/>してるだけだけど、 linq でふわっと使いたい。</para>
        /// </summary>
        /// <inheritdoc cref="string.Join"/>
        public static string JoinString<T>(this IEnumerable<T> source, string? separator) => string.Join(separator, source);
    }

    /// <summary>
    /// コレクション LINQ。
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// 全要素を削除してから指定コレクションを追加。
        /// <para><see cref="ICollection{T}.Clear"/>からの<see cref="ICollection{T}.Add"/></para>
        /// <para><see cref="ICollection{T}"/>が<see cref="List{T}"/>なら<see cref="List{T}.AddRange(IEnumerable{T})"/>する</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="collection"></param>
        public static void SetRange<T>(this ICollection<T> source, IEnumerable<T> collection)
        {
            source.Clear();
            if(source is List<T> list) {
                list.AddRange(collection);
            } else {
                foreach(var item in collection) {
                    source.Add(item);
                }
            }
        }

        /// <inheritdoc cref="List{T}.AddRange(IEnumerable{T})"/>
        public static void AddRange<T>(this ICollection<T> source, IEnumerable<T> collection)
        {
            if(source is List<T> list) {
                list.AddRange(collection);
            } else {
                foreach(var item in collection) {
                    source.Add(item);
                }
            }
        }
    }
}
