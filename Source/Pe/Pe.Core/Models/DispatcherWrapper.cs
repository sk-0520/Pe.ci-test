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
        #region define

        private class WaitParameterBase<TResult>: IDisposable
        {
            public WaitParameterBase(CancellationToken cancellationToken)
            {
                Event = new ManualResetEventSlim();
                CancellationToken = cancellationToken;
            }

            #region property

            public ManualResetEventSlim Event { get; }
            public CancellationToken CancellationToken { get; }

            [MaybeNull]
            public TResult Result { get; set; } = default!;

            #endregion

            #region IDisposable

            private bool _disposedValue;

            protected virtual void Dispose(bool disposing)
            {
                if(!this._disposedValue) {
                    if(disposing) {
                        Event.Dispose();
                    }

                    // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                    // TODO: 大きなフィールドを null に設定します
                    this._disposedValue = true;
                }
            }

            public void Dispose()
            {
                // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }

            #endregion
        }

        private class WaitParameter<TResult>: WaitParameterBase<TResult>
        {
            public WaitParameter(Func<TResult> function, CancellationToken cancellationToken)
                : base(cancellationToken)
            {
                Function = function;
            }

            #region property

            public Func<TResult> Function { get; }

            #endregion
        }

        private class WaitParameter<TArgument, TResult>: WaitParameterBase<TResult>
        {
            public WaitParameter(Func<TArgument, TResult> function, TArgument argument, CancellationToken cancellationToken)
                : base(cancellationToken)
            {
                Function = function;
                Argument = argument;
            }

            #region property

            public Func<TArgument, TResult> Function { get; }
            public TArgument Argument { get; }

            #endregion
        }

        #endregion

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
                using var waitParameter = new WaitParameter<TArgument, TResult>(func, argument, cancellationToken);
                Dispatcher.BeginInvoke(dispatcherPriority, new Action<WaitParameter<TArgument, TResult>>(static waitParameter => {
                    waitParameter.CancellationToken.ThrowIfCancellationRequested();
                    waitParameter.Result = waitParameter.Function(waitParameter.Argument);
                    waitParameter.Event.Set();
                }), waitParameter);
                if(waitParameter.Event.Wait(WaitTime, cancellationToken)) {
                    return waitParameter.Result!;
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
                using var waitParameter = new WaitParameter<T>(func, cancellationToken);
                Dispatcher.BeginInvoke(dispatcherPriority, new Action<WaitParameter<T>>(static waitParameter => {
                    waitParameter.CancellationToken.ThrowIfCancellationRequested();
                    waitParameter.Result = waitParameter.Function();
                    waitParameter.Event.Set();
                }), waitParameter);
                if(waitParameter.Event.Wait(WaitTime, cancellationToken)) {
                    return waitParameter.Result!;
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
        public Task BeginAsync<TArgument>(Action<TArgument> action, TArgument argument, DispatcherPriority dispatcherPriority)
        {
            if(CheckAccess()) {
                action(argument);
                return Task.CompletedTask;
            } else {
                return Dispatcher.BeginInvoke(dispatcherPriority, action, argument).Task;
            }
        }
        public Task BeginAsync<TArgument>(Action<TArgument> action, TArgument argument)
        {
            return BeginAsync(action, argument, DispatcherPriority.Send);
        }

        public Task BeginAsync(Action action, DispatcherPriority dispatcherPriority)
        {
            if(CheckAccess()) {
                action();
                return Task.CompletedTask;
            } else {
                return Dispatcher.BeginInvoke(dispatcherPriority, action).Task;
            }
        }
        public Task BeginAsync(Action action)
        {
            return BeginAsync(action, DispatcherPriority.Send);
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
