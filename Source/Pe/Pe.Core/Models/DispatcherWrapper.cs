using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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
        {
            Dispatcher = dispatcher;
        }

        #region IDispatcherWapper

        public Dispatcher Dispatcher { get; }

        public bool CheckAccess() => Dispatcher.CheckAccess();

        public void VerifyAccess() => Dispatcher.VerifyAccess();

        public void Invoke(Action action, DispatcherPriority dispatcherPriority, CancellationToken cancellationToken, TimeSpan timeout)
        {
            if(CheckAccess()) {
                action();
            } else {
                Dispatcher.Invoke(action, dispatcherPriority, cancellationToken, timeout);
            }
        }
        public void Invoke(Action action, DispatcherPriority dispatcherPriority, CancellationToken cancellationToken)
        {
            Invoke(action, dispatcherPriority, cancellationToken, Timeout.InfiniteTimeSpan);
        }
        public void Invoke(Action action, DispatcherPriority dispatcherPriority)
        {
            Invoke(action, dispatcherPriority, CancellationToken.None, Timeout.InfiniteTimeSpan);
        }
        public void Invoke(Action action)
        {
            // https://referencesource.microsoft.com/#WindowsBase/Base/System/Windows/Threading/Dispatcher.cs,560
            Invoke(action, DispatcherPriority.Send, CancellationToken.None, Timeout.InfiniteTimeSpan);
        }

        public void Invoke<TArgument>(Action<TArgument> action, TArgument argument, DispatcherPriority dispatcherPriority, CancellationToken cancellationToken, TimeSpan timeout)
        {
            if(CheckAccess()) {
                action(argument);
            } else {
                Dispatcher.Invoke(action, dispatcherPriority, cancellationToken, timeout);
            }
        }
        public void Invoke<TArgument>(Action<TArgument> action, TArgument argument, DispatcherPriority dispatcherPriority, CancellationToken cancellationToken)
        {
            Invoke(action, argument, dispatcherPriority, cancellationToken, Timeout.InfiniteTimeSpan);
        }
        public void Invoke<TArgument>(Action<TArgument> action, TArgument argument, DispatcherPriority dispatcherPriority)
        {
            Invoke(action, argument, dispatcherPriority, CancellationToken.None, Timeout.InfiniteTimeSpan);
        }

        public void Invoke<TArgument>(Action<TArgument> action, TArgument argument)
        {
            Invoke(action, argument, DispatcherPriority.Send, CancellationToken.None, Timeout.InfiniteTimeSpan);
        }


        public T Get<T>(Func<T> func, DispatcherPriority dispatcherPriority, CancellationToken cancellationToken, TimeSpan timeout)
        {
            if(CheckAccess()) {
                return func();
            } else {
                T result = default!;
                using var resultWait = new ManualResetEventSlim();
                Dispatcher.BeginInvoke(new Action(() => {
                    result = func();
                    resultWait.Set();
                }), dispatcherPriority, cancellationToken, timeout);
                resultWait.Wait();
                return result;
            }
        }
        public T Get<T>(Func<T> func, DispatcherPriority dispatcherPriority, CancellationToken cancellationToken)
        {
            return Get(func, dispatcherPriority, cancellationToken, Timeout.InfiniteTimeSpan);
        }
        public T Get<T>(Func<T> func, DispatcherPriority dispatcherPriority)
        {
            return Get(func, dispatcherPriority, CancellationToken.None, Timeout.InfiniteTimeSpan);
        }
        public T Get<T>(Func<T> func)
        {
            return Get(func, DispatcherPriority.Send, CancellationToken.None, Timeout.InfiniteTimeSpan);
        }


        public DispatcherOperation Begin(Action action, DispatcherPriority dispatcherPriority)
        {
            return Dispatcher.BeginInvoke(action, dispatcherPriority);
        }
        public DispatcherOperation Begin(Action action)
        {
            return Dispatcher.BeginInvoke(action, DispatcherPriority.Send);
        }

        public DispatcherOperation Begin<TArgument>(Action<TArgument> action, TArgument argument, DispatcherPriority dispatcherPriority)
        {
            return Dispatcher.BeginInvoke(action, dispatcherPriority, argument);
        }
        public DispatcherOperation Begin<TArgument>(Action<TArgument> action, TArgument argument)
        {
            return Dispatcher.BeginInvoke(action, DispatcherPriority.Send, argument);
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
    }
}
