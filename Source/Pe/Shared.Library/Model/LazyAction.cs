using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using ContentTypeTextNet.Pe.Library.Shared.Embedded.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model
{
    public class LazyAction: DisposerBase, IFlush
    {
        public LazyAction(string lazyName, TimeSpan lazyTime, ILoggerFactory loggerFactory)
        {
            LazyName = lazyName;
            Logger = loggerFactory.CreateTartget(GetType());
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
        Action Action { get; set; }

        #endregion

        #region function

        public void DelayAction(Action action)
        {
            if(Timer.Enabled) {
                Timer.Stop();
                Logger.Debug($"[{LazyName}] タイマー停止, 抑制");
            }
            Action = action;
            Logger.Debug($"[{LazyName}] タイマー開始");
            Timer.Start();
        }

        void DoAction(bool disposing)
        {
            if(disposing) {
                Timer.Stop();
            }
            Logger.Debug($"[{LazyName}] 実行");
            Action();
            Action = null;
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
