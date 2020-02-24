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

        public T Get<T>(Func<T> func, DispatcherPriority dispatcherPriority, CancellationToken cancellationToken, TimeSpan timeout)
        {
            if(CheckAccess()) {
                return func();
            } else {
                T result = default!;
                Dispatcher.Invoke(() => {
                    result = func();
                }, dispatcherPriority, cancellationToken, timeout);
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
