using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Platform
{
    public class ExplorerHorizontalScrollSupporter: DisposerBase
    {
        public ExplorerHorizontalScrollSupporter(TimeSpan checkSpan, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());

            CheckSpan = checkSpan;

            Timer = new Timer() {
                Interval = CheckSpan.TotalMilliseconds,
            };
            Timer.Elapsed += Timer_Elapsed;
        }

        #region property
        ILogger Logger { get; }

        Timer Timer { get; }

        public TimeSpan CheckSpan { get; }

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

        public void Refresh()
        {

        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Timer.Elapsed -= Timer_Elapsed;
                if(disposing) {
                    Timer.Stop();
                    Timer.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Stop();

            Refresh();

            Start();
        }

    }
}
