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
    /// 別段指定しない<see cref="Enforce"/>処理で継続できない場合に投げられる例外。
    /// </summary>
    [Serializable]
    public sealed class EnforceException: Exception
    {
        public EnforceException()
        { }

        public EnforceException(string message)
            : base(message)
        { }

        public EnforceException(string message, Exception inner)
            : base(message, inner)
        { }
    }

    /// <summary>
    /// 状態を強制。
    /// </summary>
    public static class Enforce
    {
        #region function

        [DoesNotReturn]
        private static void Throw<TException>(string callerArgument)
            where TException : Exception
        {
            throw (TException)Activator.CreateInstance(typeof(TException), new object[] { callerArgument })!;
        }

        /// <summary>
        /// 指定値が<c>null</c>の場合に例外を投げる。
        /// </summary>
        /// <typeparam name="T">指定値</typeparam>
        /// <typeparam name="TException">投げられる例外。</typeparam>
        /// <param name="value"></param>
        /// <param name="callerArgument"></param>
        /// <exception cref="TException"><paramref name="value"/>が<c>null</c></exception>。
        public static void ThrowIfNull<T, TException>([AllowNull][NotNull] T value, [CallerArgumentExpression("value")] string callerArgument = "")
            where TException : Exception
        {
            if(value is null) {
                Throw<TException>(callerArgument);
            }
        }

        public static void ThrowIfNull<T>([AllowNull][NotNull] T value, [CallerArgumentExpression("value")] string callerArgument = "")
            => ThrowIfNull<T, EnforceException>(value, callerArgument);

        public static void ThrowIfNullOrEmpty<TException>(string value, [CallerArgumentExpression("value")] string callerArgument = "")
            where TException : Exception
        {
            if(string.IsNullOrEmpty(value)) {
                Throw<TException>(callerArgument);
            }
        }

        public static void ThrowIfNullOrEmpty(string value, [CallerArgumentExpression("value")] string callerArgument = "")
            =>  ThrowIfNullOrEmpty<EnforceException>(value, callerArgument);

        public static void ThrowIfNullOrWhiteSpace<TException>(string value, [CallerArgumentExpression("value")] string callerArgument = "")
            where TException : Exception
        {
            if(string.IsNullOrWhiteSpace(value)) {
                Throw<TException>(callerArgument);
            }
        }

        public static void ThrowIfNullOrWhiteSpace(string value, [CallerArgumentExpression("value")] string callerArgument = "")
            => ThrowIfNullOrWhiteSpace<EnforceException>(value, callerArgument);

        #endregion
    }
}
