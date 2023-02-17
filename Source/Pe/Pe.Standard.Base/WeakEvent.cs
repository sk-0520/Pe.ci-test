using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ContentTypeTextNet.Pe.Standard.Base
{
    /// <summary>
    /// 弱いイベントのなんか受け側のそれっぽいの。
    /// <para><see cref="WeakEvent{TEventArgs}"/> 使ってればよろし。</para>
    /// <para>TODO: 静的リスナー未対応。</para>
    /// </summary>
    /// <typeparam name="TEventListener">リスナークラス。</typeparam>
    /// <typeparam name="TEventArgs">イベント。</typeparam>
    public class WeakEvent<TEventListener, TEventArgs>
        where TEventListener : class
        where TEventArgs : EventArgs
    {
        #region define

        /// <summary>
        /// イベントハンドラを弱く保持。
        /// </summary>
        private readonly record struct WeakHandler
        {
            /// <summary>
            /// 生成。
            /// </summary>
            /// <param name="handler"></param>
            /// <exception cref="ArgumentException"></exception>
            public WeakHandler(EventHandler<TEventArgs> handler)
            {
                var target = handler.Target;
                if(target is TEventListener listener) {
                    Listener = new WeakReference<TEventListener>(listener);
                } else {
                    throw new ArgumentException($"{nameof(handler)}.{nameof(handler.Target)}", nameof(handler));
                }

                MethodInfo = handler.GetMethodInfo();
            }

            #region property

            /// <summary>
            /// 弱参照リスナー。
            /// </summary>
            public WeakReference<TEventListener> Listener { get; }
            /// <summary>
            /// 呼び出しメソッド。
            /// </summary>
            public MethodInfo MethodInfo { get; }

            #endregion
        }

        #endregion

        #region variable

        /// <summary>
        /// <see cref="Handlers"/> の処理ロックオブジェクト。
        /// </summary>
        private readonly object _locker = new();

        #endregion

        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="eventName">イベント名。なくていいなぁ。</param>
        public WeakEvent(string eventName)
        {
            EventName = eventName;
        }

        #region property

        /// <summary>
        /// イベント名。
        /// <para>いらん気がするのですよね。</para>
        /// </summary>
        public string EventName { get; }

        /// <summary>
        /// イベントハンドラ保管箱。
        /// <para>操作する際は <see cref="_locker"/> の <c>lock</c> を行うこと。</para>
        /// </summary>
        private IList<WeakHandler> Handlers { get; } = new List<WeakHandler>();

        #endregion

        #region function

        /// <summary>
        /// イベント呼び出し。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        public void Raise(object sender, TEventArgs eventArgs)
        {
            IReadOnlyList<WeakHandler> weakHandlers;
            lock(this._locker) {
                if(Handlers.Count == 0) {
                    return;
                }

                weakHandlers = Handlers.ToArray();
            }

            var parameters = new object[2] { sender, eventArgs };
            foreach(var handler in weakHandlers) {
                if(handler.Listener.TryGetTarget(out var listener)) {
                    handler.MethodInfo.Invoke(listener, parameters);
                }
            }

            Refresh();
        }

        /// <summary>
        /// <c>event.add</c> で使用する想定。
        /// </summary>
        /// <param name="eventHandler"></param>
        /// <returns></returns>
        public bool Add(EventHandler<TEventArgs>? eventHandler)
        {
            try {
                if(eventHandler is null) {
                    return false;
                }

                var weakHandler = new WeakHandler(eventHandler);

                lock(this._locker) {
                    Handlers.Add(weakHandler);
                }
                return true;
            } finally {
                Refresh();
            }
        }

        /// <summary>
        /// <c>event.remove</c> で使用する想定。
        /// </summary>
        /// <param name="eventHandler"></param>
        /// <returns>削除成功状態: 真 成功</returns>
        public bool Remove(EventHandler<TEventArgs>? eventHandler)
        {
            if(eventHandler is null) {
                return false;
            }

            lock(this._locker) {
                for(var i = 0; i < Handlers.Count; i++) {
                    var handler = Handlers[i];

                    if(handler.Listener.TryGetTarget(out var listener)) {
                        if(eventHandler.Target == listener && handler.MethodInfo == eventHandler.Method) {
                            Handlers.RemoveAt(i);
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// すでに参照されていないやつの整理。
        /// </summary>
        private void Refresh()
        {
            lock(this._locker) {
                for(var i = Handlers.Count - 1; 0 <= i; i--) {
                    var handler = Handlers[i];

                    if(!handler.Listener.TryGetTarget(out _)) {
                        Handlers.RemoveAt(i);
                    }
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// <see cref="WeakEvent{TEventListener, TEventArgs}"/> の リスナーが <see cref="object"/> 版。
    /// <para>基本的にこっち使ってればいい。</para>
    /// </summary>
    /// <typeparam name="TEventArgs"></typeparam>
    public class WeakEvent<TEventArgs>: WeakEvent<object, TEventArgs>
        where TEventArgs : EventArgs
    {
        public WeakEvent(string eventName)
            : base(eventName)
        { }
    }

}
