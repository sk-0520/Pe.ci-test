using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public readonly struct CountingItem<TNumber, TElement>
        where TNumber : struct
    {
        public CountingItem(TNumber number, TElement element)
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
        public static IEnumerable<CountingItem<int, TElement>> Counting<TElement>(this IEnumerable<TElement> @this)
        {
            return @this.Select((v, i) => new CountingItem<int, TElement>(i, v));
        }
        /// <summary>
        /// 独自基点のインデックスと値ペア列挙。
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="this"></param>
        /// <param name="baseNumber">基点。</param>
        /// <returns></returns>
        public static IEnumerable<CountingItem<int, TElement>> Counting<TElement>(this IEnumerable<TElement> @this, int baseNumber)
        {
            return @this.Select((v, i) => new CountingItem<int, TElement>(i + baseNumber, v));
        }

        /// <summary>
        /// 入力シーケンスを結合した文字列を返す。
        /// <para><see cref="string.Join"/>してるだけだけど、 linq でふわっと使いたい。</para>
        /// </summary>
        /// <inheritdoc cref="string.Join"/>
        public static string JoinString<T>(this IEnumerable<T> values, string? separator) => string.Join(separator, values);
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
