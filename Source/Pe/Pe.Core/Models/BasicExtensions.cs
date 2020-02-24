using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public readonly struct Counter<TNumber, TElement>
        where TNumber : struct
    {
        public Counter(TNumber number, TElement element)
        {
            Value = element;
            Number = number;
        }

        #region property

        public readonly TElement Value { get; }
        public readonly TNumber Number { get; }

        #endregion
    }

    public static class IEnumerableExtensions
    {
        /// <summary>
        /// <paramref name="splitCount"/>数で分割する。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="splitCount"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> GroupSplit<T>(this IEnumerable<T> @this, int splitCount)
        {
            return @this
                 .Counting()
                .GroupBy(i => i.Number / splitCount)
                .Select(g => g.Select(i => i.Value))
            ;
        }

        /// <summary>
        /// 0基点のインデックスと値ペア列挙。
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static IEnumerable<Counter<int, TElement>> Counting<TElement>(this IEnumerable<TElement> @this)
        {
            return @this.Select((v, i) => new Counter<int, TElement>(i, v));
        }
        /// <summary>
        /// 独自基点のインデックスと値ペア列挙。
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="this"></param>
        /// <param name="baseNumber">基点。</param>
        /// <returns></returns>
        public static IEnumerable<Counter<int, TElement>> Counting<TElement>(this IEnumerable<TElement> @this, int baseNumber)
        {
            return @this.Select((v, i) => new Counter<int, TElement>(i + baseNumber, v));
        }
    }

    public static class CollectionExtensions
    {
        /// <summary>
        /// 全要素を削除してから指定コレクションを追加。
        /// <para><see cref="Collection.Clear"/>からの<see cref="ICollection{T}.Add"/></para>
        /// <para><see cref="ICollection{T}"/>が<see cref="List{T}"/>なら<see cref="List{T}.AddRange(IEnumerable{T})"/>する</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="collection"></param>
        public static void SetRange<T>(this ICollection<T> @this, IEnumerable<T> collection)
        {
            @this.Clear();
            if(@this is List<T> list) {
                list.AddRange(collection);
            } else {
                foreach(var item in collection) {
                    @this.Add(item);
                }
            }
        }
    }
}
