using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using ContentTypeTextNet.Pe.Bridge.Model;

namespace ContentTypeTextNet.Pe.Core.Model
{
    public class DispatcherWapper : IDispatcherWapper
    {
        public DispatcherWapper(Dispatcher current)
        {
            Current = current;
        }

        #region property

        protected Dispatcher Current { get; }

        #endregion

        #region IDispatcherWapper

        public void Invoke(Action action, DispatcherPriority dispatcherPriority, CancellationToken cancellationToken, TimeSpan timeout)
        {
            if(Dispatcher.CurrentDispatcher == Current) {
                action();
            } else {
                Current.Invoke(action, dispatcherPriority, cancellationToken, timeout);
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
            if(Dispatcher.CurrentDispatcher == Application.Current.Dispatcher) {
                return func();
            } else {
                T result = default!;
                Current.Invoke(() => {
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
            return Current.BeginInvoke(action, dispatcherPriority);
        }
        public DispatcherOperation Begin(Action action)
        {
            return Current.BeginInvoke(action, DispatcherPriority.Send);
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
