using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;

namespace ContentTypeTextNet.Pe.Core.Models
{
    /// <summary>
    /// <see cref="Dispatcher"/>の使用をラップ。
    /// <para><see cref="Dispatcher"/>自体は大公開しているがなんかそれっぽく楽に使いたい。</para>
    /// </summary>
    public class DispatcherWrapper: IDispatcherWrapper
    {
        /// <summary>
        /// <paramref name="dispatcher"/>をラップする。
        /// </summary>
        /// <param name="dispatcher">ラップする対象。</param>
        public DispatcherWrapper(Dispatcher dispatcher)
            : this(dispatcher, TimeSpan.FromMinutes(1))
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

        public TResult Get<TArgument, TResult>(Func<TArgument, TResult> func, TArgument argument, DispatcherPriority dispatcherPriority, CancellationToken cancellationToken)
        {
            if(CheckAccess()) {
                return func(argument);
            } else {
                using var waitBag = new Toybag<TArgument, TResult>(func, new ManualResetEventSlim(), argument, cancellationToken);
                Dispatcher.BeginInvoke(dispatcherPriority, new Action<Toybag<TArgument, TResult>>(bag => {
                    bag.CancellationToken.ThrowIfCancellationRequested();
                    bag.Result = bag.Method(bag.Parameter);
                    bag.Event.Set();
                }), waitBag);
                if(waitBag.Event.Wait(WaitTime, cancellationToken)) {
                    return waitBag.Result!;
                }
                throw new TimeoutException(WaitTime.ToString());
            }
        }
        /// <inheritdoc cref="Get{TResult}(Func{TResult}, DispatcherPriority, CancellationToken)"/>
        public TResult Get<TArgument, TResult>(Func<TArgument, TResult> func, TArgument argument, DispatcherPriority dispatcherPriority) => Get(func, argument, dispatcherPriority, CancellationToken.None);
        /// <inheritdoc cref="Get{TResult}(Func{TResult}, DispatcherPriority, CancellationToken)"/>
        public TResult Get<TArgument, TResult>(Func<TArgument, TResult> func, TArgument argument) => Get(func, argument, DispatcherPriority.Send, CancellationToken.None);

        public T Get<T>(Func<T> func, DispatcherPriority dispatcherPriority, CancellationToken cancellationToken)
        {
            if(CheckAccess()) {
                return func();
            } else {
                using var waitBag = new Toybag<T>(func, new ManualResetEventSlim(), cancellationToken);
                Dispatcher.BeginInvoke(dispatcherPriority, new Action<Toybag<T>>(bag => {
                    bag.CancellationToken.ThrowIfCancellationRequested();
                    bag.Result = bag.Method();
                    bag.Event.Set();
                }), waitBag);
                if(waitBag.Event.Wait(WaitTime, cancellationToken)) {
                    return waitBag.Result!;
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

        [SuppressMessage("Performance", "HAA0601:Value type to reference type conversion causing boxing allocation")]
        public Task Begin<TArgument>(Action<TArgument> action, TArgument argument, DispatcherPriority dispatcherPriority)
        {
            if(CheckAccess()) {
                action(argument);
                return Task.CompletedTask;
            } else {
                return Dispatcher.BeginInvoke(dispatcherPriority, action, argument).Task;
            }
        }
        public Task Begin<TArgument>(Action<TArgument> action, TArgument argument)
        {
            return Begin(action, argument, DispatcherPriority.Send);
        }

        public Task Begin(Action action, DispatcherPriority dispatcherPriority)
        {
            if(CheckAccess()) {
                action();
                return Task.CompletedTask;
            } else {
                return Dispatcher.BeginInvoke(dispatcherPriority, action).Task;
            }
        }
        public Task Begin(Action action)
        {
            return Begin(action, DispatcherPriority.Send);
        }


        #endregion
    }

    /// <summary>
    /// 生成元の<see cref="Dispatcher"/>を用いて<see cref="DispatcherWrapper"/>を生成する。
    /// </summary>
    public sealed class CurrentDispatcherWrapper: DispatcherWrapper
    {
        public CurrentDispatcherWrapper()
            : base(Dispatcher.CurrentDispatcher)
        { }

        public CurrentDispatcherWrapper(TimeSpan waitTime)
            : base(Dispatcher.CurrentDispatcher, waitTime)
        { }
    }
}
