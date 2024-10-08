using System;
using System.Timers;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Library.Base
{
    /// <summary>
    /// 遅延処理。
    /// </summary>
    public class DelayAction: DisposerBase, IFlushable
    {
        #region variable

        private readonly object _sync = new object();

        #endregion

        /// <summary>
        ///
        /// </summary>
        /// <param name="delayName">遅延処理名。</param>
        /// <param name="delayTime">遅延時間。</param>
        /// <param name="loggerFactory"></param>
        public DelayAction(string delayName, TimeSpan delayTime, ILoggerFactory loggerFactory)
        {
            DelayName = delayName;
            Logger = loggerFactory.CreateLogger(GetType());
            Timer = new Timer() {
                Interval = delayTime.TotalMilliseconds,
                AutoReset = false,
            };
            Timer.Elapsed += Timer_Elapsed;
        }

        #region property

        /// <summary>
        /// 遅延処理名。
        /// </summary>
        public string DelayName { get; }
        protected ILogger Logger { get; }
        /// <summary>
        /// 遅延処理タイマー。
        /// </summary>
        private Timer Timer { get; }
        /// <summary>
        /// 遅延処理本体。
        /// </summary>
        /// <remarks>
        /// <para>遅延処理待機中のみ非<see langword="null" />。</para>
        /// </remarks>
        private Action? Action { get; set; }

        #endregion

        #region function

        /// <summary>
        /// 遅延処理を破棄。
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "HAA0101:Array allocation for params parameter")]
        public void Clear()
        {
            lock(this._sync) {
                if(Timer.Enabled) {
                    Timer.Stop();
                    Logger.LogTrace("[{0}] タイマー終了", DelayName);
                }
                Action = null;
            }
        }

        /// <summary>
        /// 遅延処理。
        /// </summary>
        /// <remarks>
        /// <para>複数回呼び出した場合、最後に渡された処理が遅延処理対象となる。</para>
        /// </remarks>
        /// <param name="action">実際に行われる処理。</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "HAA0101:Array allocation for params parameter")]
        public void Callback(Action action)
        {
            ThrowIfDisposed();

            lock(this._sync) {
                if(Timer.Enabled) {
                    Timer.Stop();
                    Logger.LogTrace("[{0}] タイマー停止, 抑制", DelayName);
                }
                Action = action;
                Logger.LogTrace("[{0}] タイマー開始", DelayName);
                Timer.Start();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "HAA0101:Array allocation for params parameter")]
        private void FlushCore(bool disposing)
        {
            ThrowIfDisposed();

            lock(this._sync) {
                if(disposing) {
                    Timer.Stop();
                }
                if(Action != null) {
                    Logger.LogTrace("[{0}] 実行", DelayName);
                    Action();
                }
                Action = null;
            }
        }

        #endregion

        #region IFlushable

        void Flush(bool disposing)
        {
            FlushCore(disposing);
        }

        public void Flush()
        {
            FlushCore(true);
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Flush(disposing);
                Timer.Elapsed -= Timer_Elapsed;
                if(disposing) {
                    Timer.Dispose();
                }
            }
            Action = null;

            base.Dispose(disposing);
        }

        #endregion

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            FlushCore(true);
        }
    }
}
