using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public class WeakEvent<TEventListener, TEventArgs>
        where TEventListener : class
        where TEventArgs : EventArgs
    {
        #region define

        private readonly record struct WeakHandler
        {
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

            public WeakReference<TEventListener> Listener { get; }
            public MethodInfo MethodInfo { get; }

            #endregion
        }

        #endregion

        #region variable

        private readonly object _locker = new();

        #endregion

        public WeakEvent(string eventName, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(eventName + ": " + GetType());
        }

        public WeakEvent(string eventName)
            : this(eventName, NullLoggerFactory.Instance)
        {
        }

        #region property

        ILogger Logger { get; }

        private IList<WeakHandler> Handlers { get; } = new List<WeakHandler>();

        #endregion

        #region function

        public void Raise(object sender, TEventArgs eventArgs)
        {
            IReadOnlyList<WeakHandler> weakHandlers;

            lock(this._locker) {
                weakHandlers = Handlers.ToList();
            }

            var parameters = new object[] { sender, eventArgs };

            for(var i = weakHandlers.Count - 1; 0 <= i; i--) {
                var handler = weakHandlers[i];

                if(handler.Listener.TryGetTarget(out var listener)) {
                    handler.MethodInfo.Invoke(listener, parameters);
                }
            }

            Refresh();
        }

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

}
