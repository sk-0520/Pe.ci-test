using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Threading;

namespace ContentTypeTextNet.Pe.Bridge.Models
{
    /// <summary>
    /// 特定の <see cref="System.Windows.Threading.Dispatcher"/> で実行する。
    /// </summary>
    public interface IDispatcherWapper
    {
        #region property

        /// <summary>
        /// ラップ中の<see cref="System.Windows.Threading.Dispatcher"/>
        /// </summary>
        Dispatcher Dispatcher { get; }


        #endregion

        #region function
        bool CheckAccess();
        void VerifyAccess();

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

        #endregion
    }
}
