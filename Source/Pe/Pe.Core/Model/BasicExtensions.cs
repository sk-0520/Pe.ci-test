using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Model
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> GroupSplit<T>(this IEnumerable<T> @this, int splitCount)
        {
            return @this
                 .Select((v, i) => (value: v, index: i))
                .GroupBy(i => i.index / splitCount)
                .Select(g => g.Select(i => i.value))
            ;
        }
    }

    public static class CollectionExtensions
    {
        /// <summary>
        /// 全要素を削除してから指定コレクションを追加。
        /// <para><see cref="Collection.Clear"/>からの<see cref="System.Collections.ObjectModel.CollectionExtensions.AddRange{T}(Collection{T}, IEnumerable{T})"/></para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="collection"></param>
        public static void SetRange<T>(this Collection<T> @this, IEnumerable<T> collection)
        {
            @this.Clear();
            if(collection is List<T> list) {
                list.AddRange(collection);
            } else {
                foreach(var item in collection) {
                    @this.Add(item);
                }
            }
        }
    }
}
