using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    /// <summary>
    /// 処理経過をユーザー通知する。
    /// <para>0-1の範囲に制限して使用。</para>
    /// </summary>
    public class UserNotifyProgress
    {
        public UserNotifyProgress(IProgress<double> progress, IProgress<string> logging)
        {
            Progress = progress;
            Logging = logging;
        }

        #region property

        IProgress<double> Progress { get; }
        IProgress<string> Logging { get; }

        #endregion

        #region function

        public void Start()
        {
            Progress.Report(0);
            Logging.Report(string.Empty);
        }

        public void End()
        {
            Progress.Report(1);
            Logging.Report(string.Empty);
        }

        public void Report(double value, string log)
        {
            Progress.Report(value);
            Logging.Report(log);
        }

        #endregion
    }
}
