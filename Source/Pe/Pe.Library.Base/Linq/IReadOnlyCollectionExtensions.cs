using System.Collections.Generic;

namespace ContentTypeTextNet.Pe.Library.Base.Linq
{
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
}
