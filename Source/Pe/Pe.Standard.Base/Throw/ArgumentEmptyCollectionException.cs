using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace ContentTypeTextNet.Pe.Standard.Base.Throw
{
    /// <summary>
    /// 引数コレクションに対する<see langword="null" />か空を示す例外。
    /// </summary>
    [Serializable]
    public class ArgumentEmptyCollectionException: ArgumentException
    {
        public ArgumentEmptyCollectionException(string paramName)
            : this(paramName, "empty sequence")
        { }

        public ArgumentEmptyCollectionException(string paramName, string message)
            : base(message, paramName)
        { }

        #region function

        //public static void ThrowIfEmpty<T>([NotNull] IEnumerable<T>? sequence, [CallerArgumentExpression(nameof(sequence))] string? paramName = null)
        public static void ThrowIfEmpty<T>([NotNull] IEnumerable<T>? sequence, string paramName)
        {
            if(sequence is null) {
                throw new ArgumentNullException(paramName);
            }

            if(sequence is Collection<T> collection) {
                if(collection.Count == 0) {
                    throw new ArgumentEmptyCollectionException(paramName);
                }
            } else if(sequence is IReadOnlyCollection<T> readonlyCollection) {
                if(readonlyCollection.Count == 0) {
                    throw new ArgumentEmptyCollectionException(paramName);
                }
            } else if(!sequence.Any()) {
                throw new ArgumentEmptyCollectionException(paramName);
            }
        }

        #endregion
    }
}
