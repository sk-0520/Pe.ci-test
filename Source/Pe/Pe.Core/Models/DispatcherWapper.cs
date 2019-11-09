using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public class DispatcherWapper : IDispatcherWapper
    {
        public DispatcherWapper(Dispatcher current)
        {
            Dispatcher = current;
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

    public sealed class CurrentDispatcherWapper : DispatcherWapper
    {
        public CurrentDispatcherWapper()
            : base(Dispatcher.CurrentDispatcher)
        { }
    }
}
