using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Standard.Base;

namespace ContentTypeTextNet.Pe.Standard.Base
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

    /// <summary>
    /// 順序。
    /// </summary>
    public enum Order
    {
        /// <summary>
        /// 昇順。
        /// </summary>
        Ascending,
        /// <summary>
        /// 降順。
        /// </summary>
        Descending,
    }

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

    /// <summary>
    /// コレクション LINQ。
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// 全要素を削除してから指定コレクションを追加。
        /// </summary>
        /// <remarks>
        /// <para><see cref="ICollection{T}.Clear"/>からの<see cref="ICollection{T}.Add"/></para>
        /// <para><see cref="ICollection{T}"/>が<see cref="List{T}"/>なら<see cref="List{T}.AddRange(IEnumerable{T})"/>する</para>
        /// </remarks>
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
            foreach(var item in collection) {
                source.Add(item);
            }
        }
    }

    public static class IReadOnlyCollectionExtensions
    {
        #region function

        /// <summary>
        /// 指定したオブジェクトを検索し、そのオブジェクトが最初に見つかった位置のインデックス番号を返します。
        /// </summary>
        /// <typeparam name="T"><paramref name="source"/> の型。</typeparam>
        /// <param name="source">返される要素が含まれる <see cref="IEnumerable{T}"/></param>
        /// <param name="item"><paramref name="source"/> 内で検索するオブジェクト。</param>
        /// <returns><paramref name="source"/> で <paramref name="item"/> が見つかった場合は、最初に見つかった位置のインデックス。それ以外の場合は、配列の下限 - 1。</returns>
        public static int IndexOf<T>(this IReadOnlyCollection<T> source, T item)
        {
            if(source is IList<T> list) {
                return list.IndexOf(item);
            }

            var index = 0;
            foreach(var element in source) {
                if(EqualityComparer<T>.Default.Equals(item, element)) {
                    return index;
                }
                index += 1;
            }

            return -1;
        }

        #endregion
    }

    public static class IEnumerableNonGenericsExtensions
    {
        #region function

        private static bool NonGenericsAnyCore(IEnumerable source, Predicate<object?>? predicate)
        {
            if(predicate is null) {
                predicate = o => true;
            }

            var enumerator = source.GetEnumerator();

            while(enumerator.MoveNext()) {
                if(predicate(enumerator.Current)) {
                    return true;
                }
            }

            return false;
        }

        public static bool NonGenericsAny(this IEnumerable source)
        {
            return NonGenericsAnyCore(source, null);
        }

        public static bool NonGenericsAny(this IEnumerable source, Predicate<object?> predicate)
        {
            Debug.Assert(predicate is not null);
            return NonGenericsAnyCore(source, predicate);
        }

        #endregion
    }
}
