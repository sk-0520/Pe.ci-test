using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model
{
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
            @this.AddRange(collection);
        }
    }
}
