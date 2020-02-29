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
        DispatcherOperation Begin<TArgument>(Action<TArgument> action, TArgument argument, DispatcherPriority dispatcherPriority);
        DispatcherOperation Begin<TArgument>(Action<TArgument> action, TArgument argument);

        #endregion
    }
}
