using System;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    /// <summary>
    /// アプリケーション構成: API。
    /// </summary>
    public class ApiConfiguration: ConfigurationBase
    {
        public ApiConfiguration(IConfigurationSection section)
            : base(section)
        { }

        #region property

        /// <summary>
        /// クラッシュレポート送信先エンドポイント。
        /// </summary>
        [Configuration]
        public Uri CrashReportUri { get; } = default!;
        /// <summary>
        /// クラッシュレポート ソースコード URI。
        /// </summary>
        [Configuration]
        public Uri CrashReportSourceUri { get; } = default!;

        /// <summary>
        /// フィードバック送信先エンドポイント。
        /// </summary>
        [Configuration]
        public Uri FeedbackUri { get; } = default!;
        /// <summary>
        /// フィードバック ソースコード URI。
        /// </summary>
        [Configuration]
        public Uri FeedbackSourceUri { get; } = default!;

        /// <summary>
        /// プラグイン情報取得APIエンドポイント。
        /// </summary>
        [Configuration]
        public Uri ServerPluginInformation { get; } = default!;

        #endregion
    }
}
