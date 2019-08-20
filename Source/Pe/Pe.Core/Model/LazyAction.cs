using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using ContentTypeTextNet.Pe.Common.Model;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Model
{
    public class LazyAction : DisposerBase, IFlushable
    {
        #region variable

        readonly object _timerLocker = new object();

        #endregion

        public LazyAction(string lazyName, TimeSpan lazyTime, ILoggerFactory loggerFactory)
        {
            LazyName = lazyName;
            Logger = loggerFactory.CreateLogger(GetType());
            Timer = new Timer() {
                Interval = lazyTime.TotalMilliseconds,
                AutoReset = false,
            };
            Timer.Elapsed += Timer_Elapsed;
        }


        #region property
        public string LazyName { get; }
        protected ILogger Logger { get; }
        Timer Timer { get; }
        Action? Action { get; set; }

        #endregion

        #region function

        public void DelayAction(Action action)
        {
            lock(this._timerLocker) {
                if(Timer.Enabled) {
                    Timer.Stop();
                    Logger.LogDebug($"[{LazyName}] タイマー停止, 抑制");
                }
                Action = action;
                Logger.LogDebug($"[{LazyName}] タイマー開始");
                Timer.Start();
            }
        }

        void DoAction(bool disposing)
        {
            lock(this._timerLocker) {
                if(disposing) {
                    Timer.Stop();
                }
                if(Action != null) {
                    Logger.LogDebug($"[{LazyName}] 実行");
                    Action();
                }
                Action = null;
            }
        }

        #endregion

        #region IFlush

        void Flush(bool disposing)
        {
            DoAction(disposing);
        }

        public void Flush()
        {
            DoAction(true);
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Flush(disposing);
                Timer.Dispose();
            }
            Action = null;

            base.Dispose(disposing);
        }

        #endregion

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            DoAction(true);
        }


    }
}
