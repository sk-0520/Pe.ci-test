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
    /// 別段指定しない<see cref="Throws"/>処理で継続できない場合に投げられる例外。
    /// </summary>
    [Serializable]
    public sealed class LogicException: Exception
    {
        public LogicException()
        { }

        public LogicException(string message)
            : base(message)
        { }

        public LogicException(string message, Exception inner)
            : base(message, inner)
        { }
    }

    /// <summary>
    /// 状態を強制。
    /// </summary>
    public static class Throws
    {
        #region function

        [DoesNotReturn]
        private static void ThrowCore<TException>(string callerArgument)
            where TException : Exception
        {
            throw (TException)Activator.CreateInstance(typeof(TException), [callerArgument])!;
        }

        /// <summary>
        /// 指定値が偽の場合、例外を投げる。
        /// </summary>
        /// <typeparam name="TException">投げられる例外。</typeparam>
        /// <param name="value">真偽値。</param>
        /// <param name="callerArgument"></param>
        public static void ThrowIf<TException>(bool value, [CallerArgumentExpression(nameof(value))] string callerArgument = "")
            where TException : Exception
        {
            if(!value) {
                ThrowCore<TException>(callerArgument);
            }
        }

        /// <inheritdoc cref="ThrowIf{LogicException}"/>
        /// <exception cref="LogicException"></exception>
        public static void ThrowIf(bool value, [CallerArgumentExpression(nameof(value))] string callerArgument = "")
            => ThrowIf<LogicException>(value, callerArgument);

        /// <summary>
        /// 指定値が<see langword="null" />の場合に例外を投げる。
        /// </summary>
        /// <typeparam name="T">指定値</typeparam>
        /// <typeparam name="TException">投げられる例外。</typeparam>
        /// <param name="value"></param>
        /// <param name="callerArgument"></param>
        /// <exception cref="TException"><paramref name="value"/>が<see langword="null" /></exception>。
        public static void ThrowIfNull<T, TException>([AllowNull][NotNull] T value, [CallerArgumentExpression(nameof(value))] string callerArgument = "")
            where TException : Exception
        {
            if(value is null) {
                ThrowCore<TException>(callerArgument);
            }
        }

        /// <inheritdoc cref="ThrowIfNull{T, LogicException}"/>
        /// <exception cref="LogicException"></exception>
        public static void ThrowIfNull<T>([AllowNull][NotNull] T value, [CallerArgumentExpression(nameof(value))] string callerArgument = "")
            => ThrowIfNull<T, LogicException>(value, callerArgument);

        /// <summary>
        /// 指定値が<see cref="string.IsNullOrEmpty"/>に該当する場合、例外を投げる。
        /// </summary>
        /// <typeparam name="TException">投げられる例外。</typeparam>
        /// <param name="value"></param>
        /// <param name="callerArgument"></param>
        public static void ThrowIfNullOrEmpty<TException>([NotNull] string? value, [CallerArgumentExpression(nameof(value))] string callerArgument = "")
            where TException : Exception
        {
            if(string.IsNullOrEmpty(value)) {
                ThrowCore<TException>(callerArgument);
            }
        }

        /// <inheritdoc cref="ThrowIfNullOrEmpty{LogicException}"/>
        /// <exception cref="LogicException"></exception>
        public static void ThrowIfNullOrEmpty([NotNull] string? value, [CallerArgumentExpression(nameof(value))] string callerArgument = "")
            => ThrowIfNullOrEmpty<LogicException>(value, callerArgument);

        /// <summary>
        /// 指定値が<see cref="string.IsNullOrWhiteSpace"/>に該当する場合、例外を投げる。
        /// </summary>
        /// <typeparam name="TException">投げられる例外。</typeparam>
        /// <param name="value"></param>
        /// <param name="callerArgument"></param>
        public static void ThrowIfNullOrWhiteSpace<TException>([NotNull] string? value, [CallerArgumentExpression(nameof(value))] string callerArgument = "")
            where TException : Exception
        {
            if(string.IsNullOrWhiteSpace(value)) {
                ThrowCore<TException>(callerArgument);
            }
        }

        /// <inheritdoc cref="ThrowIfNullOrWhiteSpace{LogicException}"/>
        /// <exception cref="LogicException"></exception>
        public static void ThrowIfNullOrWhiteSpace([NotNull] string? value, [CallerArgumentExpression(nameof(value))] string callerArgument = "")
            => ThrowIfNullOrWhiteSpace<LogicException>(value, callerArgument);

        #endregion
    }
}
