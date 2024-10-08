using System;
using System.Timers;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Platform
{
    /// <summary>
    /// システムに対してユーザーが生きている旨を通知。
    /// </summary>
    /// <remarks>
    /// <para>アイドル状態になって画面ロックやらスリープになるのを抑制。</para>
    /// </remarks>
    public class HeartBeatSender: DisposerBase
    {
        public HeartBeatSender(TimeSpan sendSpan, TimeSpan checkSpan, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());

            SendSpan = sendSpan;

            Timer = new Timer() {
                Interval = checkSpan.TotalMilliseconds,
            };
            Timer.Elapsed += Timer_Elapsed;
        }

        #region property

        private ILogger Logger { get; }

        private Timer Timer { get; }

        [DateTimeKind(DateTimeKind.Utc)]
        private DateTime LastSendTime { get; set; } = DateTime.MinValue;

        public TimeSpan SendSpan { get; }

        bool CanSend => SendSpan < DateTime.UtcNow - LastSendTime;

        #endregion

        #region function

        public void Start()
        {
            Timer.Stop();
            Timer.Start();
        }

        public void Stop()
        {
            Timer.Stop();
        }

        private void Send()
        {
            Logger.LogDebug("ロック抑制");

            var vistaFlag
                = ES.ES_DISPLAY_REQUIRED
                | ES.ES_SYSTEM_REQUIRED
                | ES.ES_AWAYMODE_REQUIRED
            ;

            if((int)NativeMethods.SetThreadExecutionState(vistaFlag) == 0) {
                // OS バージョン的には到達しない。でも失敗の場合は到達する
                var toutastu_shinai
                    = ES.ES_DISPLAY_REQUIRED
                    | ES.ES_SYSTEM_REQUIRED
                ;
                NativeMethods.SetThreadExecutionState(toutastu_shinai);
            }

            LastSendTime = DateTime.UtcNow;
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Timer.Elapsed -= Timer_Elapsed;
                Stop();
                if(disposing) {
                    Timer.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            Stop();
            if(CanSend) {
                Send();
            }
            Start();
        }
    }
}
