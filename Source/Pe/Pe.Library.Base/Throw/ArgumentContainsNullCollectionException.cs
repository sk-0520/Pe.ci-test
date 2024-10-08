using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.Base.Throw
{
    public class ArgumentContainsNullCollectionException: ArgumentException
    {
        public ArgumentContainsNullCollectionException(string paramName)
            : this(paramName, "sequence contains null")
        { }

        public ArgumentContainsNullCollectionException(string paramName, string message)
            : base(message, paramName)
        { }

        #region function

        public static void ThrowIfContainsNull<T>(IEnumerable<T> sequence, string paramName)
        {
            if(sequence.Any(a => a is null)) {
                throw new ArgumentContainsNullCollectionException(paramName);
            }
        }

        #endregion
    }
}
