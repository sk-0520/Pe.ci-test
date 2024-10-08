using System;
using System.Collections.Generic;
using System.Linq;
using ContentTypeTextNet.Pe.Library.Base.Linq;

namespace ContentTypeTextNet.Pe.Library.Base.Linq
{
    /// <summary>
    /// ふわふわ LINQ。
    /// </summary>
    public static class IEnumerableExtensions
    {
        #region function

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
        /// </summary>
        /// <remarks>
        /// <para><see cref="string.Join"/>してるだけだけど、 linq でふわっと使いたい。</para>
        /// </remarks>
        /// <inheritdoc cref="string.Join"/>
        public static string JoinString<T>(this IEnumerable<T> source, string? separator) => string.Join(separator, source);

        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Order order, Func<TSource, TKey> keySelector)
        {
            return order switch {
                Order.Ascending => source.OrderBy(keySelector),
                Order.Descending => source.OrderByDescending(keySelector),
                _ => throw new NotImplementedException(),
            };
        }

        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Order order, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer)
        {
            return order switch {
                Order.Ascending => source.OrderBy(keySelector, comparer),
                Order.Descending => source.OrderByDescending(keySelector, comparer),
                _ => throw new NotImplementedException(),
            };
        }

        public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Order order, Func<TSource, TKey> keySelector)
        {
            return order switch {
                Order.Ascending => source.ThenBy(keySelector),
                Order.Descending => source.ThenByDescending(keySelector),
                _ => throw new NotImplementedException(),
            };
        }

        public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Order order, Func<TSource, TKey> keySelector, IComparer<TKey>? comparer)
        {
            return order switch {
                Order.Ascending => source.ThenBy(keySelector, comparer),
                Order.Descending => source.ThenByDescending(keySelector, comparer),
                _ => throw new NotImplementedException(),
            };
        }

        /// <summary>
        /// シーケンスの要素が全て同じか。
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns>同じか。</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool AllEquals<TSource>(this IEnumerable<TSource> source)
        {
            if(source is null) {
                throw new ArgumentNullException(nameof(source));
            }

            var enumerator = source.GetEnumerator();
            if(enumerator.MoveNext()) {
                var baseElement = enumerator.Current;
                if(baseElement is null) {
                    while(enumerator.MoveNext()) {
                        var currentElement = enumerator.Current;
                        if(currentElement is not null) {
                            return false;
                        }
                    }
                } else {
                    while(enumerator.MoveNext()) {
                        var currentElement = enumerator.Current;
                        if(!baseElement.Equals(currentElement)) {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        #endregion
    }
}
