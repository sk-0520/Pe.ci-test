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
    /// <para>Pe から提供される。</para>
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
        /// <inheritdoc cref="Dispatcher.CheckAccess"/>
        bool CheckAccess();
        /// <inheritdoc cref="Dispatcher.VerifyAccess"/>
        void VerifyAccess();

        /// <inheritdoc cref="Dispatcher.InvokeAsync(Action, DispatcherPriority, CancellationToken)"/>
        Task InvokeAsync(Action action, DispatcherPriority dispatcherPriority, CancellationToken cancellationToken);
        /// <inheritdoc cref="InvokeAsync(Action, DispatcherPriority, CancellationToken)"/>
        Task InvokeAsync(Action action, DispatcherPriority dispatcherPriority);
        /// <inheritdoc cref="InvokeAsync(Action, DispatcherPriority, CancellationToken)"/>
        Task InvokeAsync(Action action);
        /// <inheritdoc cref="Dispatcher.InvokeAsync{TResult}(Func{TResult}, DispatcherPriority, CancellationToken)"/>
        Task<TResult> InvokeAsync<TResult>(Func<TResult> func, DispatcherPriority dispatcherPriority, CancellationToken cancellationToken);
        /// <inheritdoc cref="InvokeAsync{TResult}(Func{TResult}, DispatcherPriority, CancellationToken)"/>
        Task<TResult> InvokeAsync<TResult>(Func<TResult> func, DispatcherPriority dispatcherPriority);
        /// <inheritdoc cref="InvokeAsync{TResult}(Func{TResult}, DispatcherPriority, CancellationToken)"/>
        Task<TResult> InvokeAsync<TResult>(Func<TResult> func);


        /// <summary>
        /// 対象 <see cref="Dispatcher"/> でなんかした結果を取得する。
        /// <para>内部的には停止状態。</para>
        /// <para>癌。</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="dispatcherPriority"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        T Get<T>(Func<T> func, DispatcherPriority dispatcherPriority, CancellationToken cancellationToken);
        /// <inheritdoc cref="Get{T}(Func{T}, DispatcherPriority, CancellationToken)"/>
        T Get<T>(Func<T> func, DispatcherPriority dispatcherPriority);
        /// <inheritdoc cref="Get{T}(Func{T}, DispatcherPriority, CancellationToken)"/>
        T Get<T>(Func<T> func);

        /// <summary>
        /// <inheritdoc cref="Dispatcher.BeginInvoke(Delegate, DispatcherPriority, object[])"/>
        /// </summary>
        /// <param name="action">実施する処理。</param>
        /// <param name="dispatcherPriority"></param>
        /// <param name="argument">パラメータ。</param>
        DispatcherOperation Begin<TArgument>(Action<TArgument> action, TArgument argument, DispatcherPriority dispatcherPriority);
        /// <inheritdoc cref="Begin{TArgument}(Action{TArgument}, TArgument, DispatcherPriority)"/>
        DispatcherOperation Begin<TArgument>(Action<TArgument> action, TArgument argument);
        /// <inheritdoc cref="Begin{TArgument}(Action{TArgument}, TArgument, DispatcherPriority)"/>
        DispatcherOperation Begin(Action action, DispatcherPriority dispatcherPriority);
        /// <inheritdoc cref="Begin{TArgument}(Action{TArgument}, TArgument, DispatcherPriority)"/>
        DispatcherOperation Begin(Action action);

        #endregion
    }
}
