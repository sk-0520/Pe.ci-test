using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Models
{
    /// <summary>
    /// 遅延処理。
    /// </summary>
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

        /// <summary>
        /// 遅延処理名。
        /// </summary>
        public string LazyName { get; }
        protected ILogger Logger { get; }
        /// <summary>
        /// 遅延処理タイマー。
        /// </summary>
        Timer Timer { get; }
        /// <summary>
        /// 遅延処理本体。
        /// <para>遅延処理待機中のみ非null</para>
        /// </summary>
        Action? Action { get; set; }

        #endregion

        #region function

        /// <summary>
        /// 遅延処理を破棄。
        /// </summary>
        public void Clear()
        {
            lock(this._timerLocker) {
                if(Timer.Enabled) {
                    Timer.Stop();
                    Logger.LogDebug("[{0}] タイマー終了", LazyName);
                }
                Action = null;
            }
        }

        /// <summary>
        /// 遅延処理。
        /// <para>複数回呼び出した場合、最後に渡された処理が遅延処理対象となる。</para>
        /// </summary>
        /// <param name="action">実際に行われる処理。</param>
        public void DelayAction(Action action)
        {
            ThrowIfDisposed();

            lock(this._timerLocker) {
                if(Timer.Enabled) {
                    Timer.Stop();
                    Logger.LogDebug("[{0}] タイマー停止, 抑制", LazyName);
                }
                Action = action;
                Logger.LogDebug("[{0}] タイマー開始", LazyName);
                Timer.Start();
            }
        }

        void DoAction(bool disposing)
        {
            ThrowIfDisposed();

            lock(this._timerLocker) {
                if(disposing) {
                    Timer.Stop();
                }
                if(Action != null) {
                    Logger.LogDebug("[{0}] 実行", LazyName);
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
