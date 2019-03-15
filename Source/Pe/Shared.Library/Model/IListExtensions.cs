using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model
{
    public static class IListExtensions
    {
        public static void AddRange<T>(this IList<T> @this, IEnumerable<T> collection)
        {
            if(@this is List<T> list) {
                list.AddRange(collection);
                return;
            }

            foreach(var item in collection) {
                @this.Add(item);
            }
        }
    }
}
