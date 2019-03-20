using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model
{
    /// <summary>
    /// 特定の <see cref="Dispatcher"/> で実行する。
    /// </summary>
    public interface IDispatcherWapper
    {
        void Invoke(Action action, DispatcherPriority dispatcherPriority, CancellationToken cancellationToken, TimeSpan timeout);
        void Invoke(Action action, DispatcherPriority dispatcherPriority, CancellationToken cancellationToken);
        void Invoke(Action action, DispatcherPriority dispatcherPriority);
        void Invoke(Action action);

        T Get<T>(Func<T> func, DispatcherPriority dispatcherPriority, CancellationToken cancellationToken, TimeSpan timeout);
        T Get<T>(Func<T> func, DispatcherPriority dispatcherPriority, CancellationToken cancellationToken);
        T Get<T>(Func<T> func, DispatcherPriority dispatcherPriority);
        T Get<T>(Func<T> func);

        DispatcherOperation Begin(Action action, DispatcherPriority dispatcherPriority);
        DispatcherOperation Begin(Action action);
    }

    public class DispatcherWapper : IDispatcherWapper
    {
        public DispatcherWapper(Dispatcher current, ILogger logger)
        {
            Current = current;
            Logger = logger;
        }
        public DispatcherWapper(Dispatcher current, ILoggerFactory loggerFactory)
        {
            Current = current;
            Logger = loggerFactory.CreateTartget(GetType());
        }

        #region property

        protected Dispatcher Current { get; }
        protected ILogger Logger { get; }

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
                var result = default(T);
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
}
