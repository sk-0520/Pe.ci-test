using System;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    /// <summary>
    /// アプリケーション構成: ディスプレイ。
    /// </summary>
    public class DisplayConfiguration: ConfigurationBase
    {
        public DisplayConfiguration(IConfigurationSection section)
            : base(section)
        { }

        #region property

        /// <summary>
        /// ディスプレイ数変更検知再実施回数。
        /// </summary>
        [Configuration]
        public int ChangedRetryCount { get; }
        /// <summary>
        /// ディスプレイ数変更検知待機時間。
        /// </summary>
        [Configuration]
        public TimeSpan ChangedRetryWaitTime { get; }

        #endregion
    }
}
