using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;

namespace ContentTypeTextNet.Pe.Core.Models
{
    /// <summary>
    /// <see cref="Dispatcher"/>の使用をラップ。
    /// <para><see cref="Dispatcher"/>自体は大公開しているがなんかそれっぽく楽に使いたい。</para>
    /// </summary>
    public class DispatcherWrapper : IDispatcherWrapper
    {
        /// <summary>
        /// <paramref name="dispatcher"/>をラップする。
        /// </summary>
        /// <param name="dispatcher">ラップする対象。</param>
        public DispatcherWrapper(Dispatcher dispatcher)
            :this(dispatcher, TimeSpan.FromMinutes(1))
        { }

        public DispatcherWrapper(Dispatcher dispatcher, TimeSpan waitTime)
        {
            Dispatcher = dispatcher;
            WaitTime = waitTime;
        }

        #region property

        protected TimeSpan WaitTime { get; set; }

        #endregion

        #region IDispatcherWapper

        public Dispatcher Dispatcher { get; }

        public bool CheckAccess() => Dispatcher.CheckAccess();

        public void VerifyAccess() => Dispatcher.VerifyAccess();

        public Task InvokeAsync(Action action, DispatcherPriority dispatcherPriority, CancellationToken cancellationToken)
        {
            if(CheckAccess()) {
                action();
                return Task.CompletedTask;
            }

            return Dispatcher.InvokeAsync(action, dispatcherPriority, cancellationToken).Task;
        }
        public Task InvokeAsync(Action action, DispatcherPriority dispatcherPriority)
        {
            return InvokeAsync(action, dispatcherPriority, CancellationToken.None);
        }
        public Task InvokeAsync(Action action)
        {
            return InvokeAsync(action, DispatcherPriority.Send, CancellationToken.None);
        }
        public Task<TResult> InvokeAsync<TResult>(Func<TResult> func, DispatcherPriority dispatcherPriority, CancellationToken cancellationToken)
        {
            if(CheckAccess()) {
                return Task.FromResult(func());
            }

            return Dispatcher.InvokeAsync(func, dispatcherPriority, cancellationToken).Task;
        }
        public Task<TResult> InvokeAsync<TResult>(Func<TResult> func, DispatcherPriority dispatcherPriority)
        {
            return InvokeAsync(func, dispatcherPriority, CancellationToken.None);
        }
        public Task<TResult> InvokeAsync<TResult>(Func<TResult> func)
        {
            return InvokeAsync(func, DispatcherPriority.Send, CancellationToken.None);
        }

        public T Get<T>(Func<T> func, DispatcherPriority dispatcherPriority, CancellationToken cancellationToken)
        {
            if(CheckAccess()) {
                return func();
            } else {
                T result = default!;
                using var resultWait = new ManualResetEventSlim();
                Dispatcher.BeginInvoke(new Action(() => {
                    cancellationToken.ThrowIfCancellationRequested();
                    result = func();
                    resultWait.Set();
                }), dispatcherPriority);
                if(resultWait.Wait(WaitTime, cancellationToken)) {
                    return result;
                }
                throw new TimeoutException(WaitTime.ToString());
            }
        }
        public T Get<T>(Func<T> func, DispatcherPriority dispatcherPriority)
        {
            return Get(func, dispatcherPriority, CancellationToken.None);
        }
        public T Get<T>(Func<T> func)
        {
            return Get(func, DispatcherPriority.Send, CancellationToken.None);
        }

        public DispatcherOperation Begin<TArgument>(Action<TArgument> action, TArgument argument, DispatcherPriority dispatcherPriority)
        {
            return Dispatcher.BeginInvoke(action, dispatcherPriority, argument);
        }
        public DispatcherOperation Begin<TArgument>(Action<TArgument> action, TArgument argument)
        {
            return Dispatcher.BeginInvoke(action, DispatcherPriority.Send, argument);
        }

        public DispatcherOperation Begin(Action action, DispatcherPriority dispatcherPriority)
        {
            return Dispatcher.BeginInvoke(action, dispatcherPriority);
        }
        public DispatcherOperation Begin(Action action)
        {
            return Dispatcher.BeginInvoke(action, DispatcherPriority.Send);
        }


        #endregion
    }

    /// <summary>
    /// 生成元の<see cref="Dispatcher"/>を用いて<see cref="DispatcherWrapper"/>を生成する。
    /// </summary>
    public sealed class CurrentDispatcherWrapper : DispatcherWrapper
    {
        public CurrentDispatcherWrapper()
            : base(Dispatcher.CurrentDispatcher)
        { }

        public CurrentDispatcherWrapper(TimeSpan waitTime)
            : base(Dispatcher.CurrentDispatcher, waitTime)
        { }
    }
}
