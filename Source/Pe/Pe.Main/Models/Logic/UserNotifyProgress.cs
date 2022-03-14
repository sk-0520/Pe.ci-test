using System;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    /// <summary>
    /// 処理経過をユーザー通知する。
    /// </summary>
    public class UserNotifyProgress
    {
        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="progress">パーセント(0-1)進行状況の更新のプロバイダー。</param>
        /// <param name="logging">ログ進捗状況の更新のプロバイダー。</param>
        public UserNotifyProgress(IProgress<double> progress, IProgress<string> logging)
        {
            Progress = progress;
            Logging = logging;
        }

        #region property

        private IProgress<double> Progress { get; }
        private IProgress<string> Logging { get; }

        #endregion

        #region function

        /// <summary>
        /// 開始。
        /// </summary>
        public void Start()
        {
            Progress.Report(0);
            Logging.Report(string.Empty);
        }

        /// <summary>
        /// 終了。
        /// </summary>
        public void End()
        {
            Progress.Report(1);
            Logging.Report(string.Empty);
        }

        /// <summary>
        /// <see cref="IProgress{double}"/>, <see cref="IProgress{string}"/> の進捗状況を更新。
        /// </summary>
        /// <param name="value">0-1までの数値進捗状況。</param>
        /// <param name="log">ログ進捗状況。</param>
        public void Report(double value, string log)
        {
            Progress.Report(value);
            Logging.Report(log);
        }

        #endregion
    }

    /// <summary>
    /// 処理経過をユーザーに通知しないダミー通知。
    /// </summary>
    public sealed class NullNotifyProgress: UserNotifyProgress
    {
        public NullNotifyProgress(ILoggerFactory loggerFactory)
            : base(new NullDoubleProgress(loggerFactory), new NullStringProgress(loggerFactory))
        { }
    }
}
