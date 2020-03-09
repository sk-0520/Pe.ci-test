using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ContentTypeTextNet.Pe.Bridge.Models
{
    /// <summary>
    /// 特定の <see cref="System.Windows.Threading.Dispatcher"/> で実行する。
    /// </summary>
    public interface IDispatcherWrapper
    {
        #region property

        /// <summary>
        /// ラップ中の<see cref="System.Windows.Threading.Dispatcher"/>
        /// </summary>
        Dispatcher Dispatcher { get; }


        #endregion

        #region function
        /// <summary>
        ///呼び出し元のスレッドがこの <see cref="Dispatcher"/> に関連付けられたスレッドであるかどうかを判断します。
        /// </summary>
        /// <remarks>https://docs.microsoft.com/ja-jp/dotnet/api/system.windows.threading.dispatcher.checkaccess?view=netframework-4.8</remarks>
        /// <returns></returns>
        bool CheckAccess();
        /// <summary>
        /// 呼び出し元のスレッドがこの <see cref="Dispatcher"/> にアクセスできるかどうかを確認します。
        /// </summary>
        void VerifyAccess();

        [Obsolete]
        void Invoke(Action action, DispatcherPriority dispatcherPriority, CancellationToken cancellationToken, TimeSpan timeout);
        [Obsolete]
        void Invoke(Action action, DispatcherPriority dispatcherPriority, CancellationToken cancellationToken);
        [Obsolete]
        void Invoke(Action action, DispatcherPriority dispatcherPriority);
        [Obsolete]
        void Invoke(Action action);
        [Obsolete]
        void Invoke<TArgument>(Action<TArgument> action, TArgument argument, DispatcherPriority dispatcherPriority, CancellationToken cancellationToken, TimeSpan timeout);
        [Obsolete]
        void Invoke<TArgument>(Action<TArgument> action, TArgument argument, DispatcherPriority dispatcherPriority, CancellationToken cancellationToken);
        [Obsolete]
        void Invoke<TArgument>(Action<TArgument> action, TArgument argument, DispatcherPriority dispatcherPriority);
        [Obsolete]
        void Invoke<TArgument>(Action<TArgument> action, TArgument argument);

        Task InvokeAsync(Action action, DispatcherPriority dispatcherPriority, CancellationToken cancellationToken);
        Task InvokeAsync(Action action, DispatcherPriority dispatcherPriority);
        Task InvokeAsync(Action action);
        Task<TResult> InvokeAsync<TResult>(Func<TResult> func, DispatcherPriority dispatcherPriority, CancellationToken cancellationToken);
        Task<TResult> InvokeAsync<TResult>(Func<TResult> func, DispatcherPriority dispatcherPriority);
        Task<TResult> InvokeAsync<TResult>(Func<TResult> func);


        /// <summary>
        /// 対象 <see cref="Dispatcher"/> でなんかした結果を取得する。
        /// <para>内部的には停止状態。</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="dispatcherPriority"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        T Get<T>(Func<T> func, DispatcherPriority dispatcherPriority, CancellationToken cancellationToken);
        /// <summary>
        /// <see cref="Get{T}(Func{T}, DispatcherPriority, CancellationToken)"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="dispatcherPriority"></param>
        /// <returns></returns>
        T Get<T>(Func<T> func, DispatcherPriority dispatcherPriority);
        /// <summary>
        /// <see cref="Get{T}(Func{T}, DispatcherPriority, CancellationToken)"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        T Get<T>(Func<T> func);

        DispatcherOperation Begin(Action action, DispatcherPriority dispatcherPriority);
        DispatcherOperation Begin(Action action);
        DispatcherOperation Begin<TArgument>(Action<TArgument> action, TArgument argument, DispatcherPriority dispatcherPriority);
        DispatcherOperation Begin<TArgument>(Action<TArgument> action, TArgument argument);

        #endregion
    }
}
