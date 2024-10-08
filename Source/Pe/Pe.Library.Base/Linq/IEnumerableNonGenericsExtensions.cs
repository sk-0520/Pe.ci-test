using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.Base.Linq
{
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
