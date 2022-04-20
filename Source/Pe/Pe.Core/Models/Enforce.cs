using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Core.Models
{
    /// <summary>
    /// <see cref="Enforce"/>処理で継続できない場合に投げられる例外。
    /// </summary>
    [Serializable]
    public class EnforceException: Exception
    {
        public EnforceException()
        { }
        public EnforceException(string message)
            : base(message)
        { }
        public EnforceException(string message, Exception inner)
            : base(message, inner)
        { }
        protected EnforceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }

    /// <summary>
    /// 状態を強制。
    /// </summary>
    public static class Enforce
    {
        #region function

        /// <summary>
        /// 指定値が<c>null</c>の場合に例外を投げる。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="callerArgument"></param>
        /// <exception cref="EnforceException"><paramref name="value"/>が<c>null</c></exception>。
        public static void ThrowIfNull<T>([AllowNull][NotNull] T value, [CallerArgumentExpression("value")] string callerArgument = "")
        {
            if(value is null) {
                throw new EnforceException(callerArgument);
            }
        }

        public static void ThrowIfNullOrEmpty(string value, [CallerArgumentExpression("value")] string callerArgument = "")
        {
            if(string.IsNullOrEmpty(value)) {
                throw new EnforceException(callerArgument);
            }
        }

        public static void ThrowIfNullOrWhiteSpace(string value, [CallerArgumentExpression("value")] string callerArgument = "")
        {
            if(string.IsNullOrWhiteSpace(value)) {
                throw new EnforceException(callerArgument);
            }
        }

        #endregion
    }
}
