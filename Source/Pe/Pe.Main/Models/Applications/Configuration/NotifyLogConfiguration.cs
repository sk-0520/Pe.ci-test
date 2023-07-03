using System;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    /// <summary>
    /// アプリケーション構成: 通知。
    /// </summary>
    public class NotifyLogConfiguration: ConfigurationBase
    {
        public NotifyLogConfiguration(IConfigurationSection section)
            : base(section)
        { }

        #region property

        /// <summary>
        /// 通常ログの表示時間。
        /// </summary>
        [Configuration]
        public TimeSpan NormalLogDisplayTime { get; }
        /// <summary>
        /// 元に戻せるログの表示時間。
        /// </summary>
        [Configuration]
        public TimeSpan UndoLogDisplayTime { get; }
        /// <summary>
        /// コマンドログの表示時間。
        /// </summary>
        [Configuration]
        public TimeSpan CommandLogDisplayTime { get; }

        /// <summary>
        /// フェードアウト時間。
        /// </summary>
        [Configuration]
        public TimeSpan FadeoutTime { get; }

        #endregion
    }
}
