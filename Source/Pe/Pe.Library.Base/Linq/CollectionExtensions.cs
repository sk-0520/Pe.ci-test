using System.Collections.Generic;

namespace ContentTypeTextNet.Pe.Library.Base.Linq
{
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
}
