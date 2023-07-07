using System;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    /// <summary>
    /// アプリケーション構成: Web アクセス系。
    /// </summary>
    public class WebConfiguration: ConfigurationBase
    {
        public WebConfiguration(IConfigurationSection section)
            : base(section)
        { }

        #region property

        /// <summary>
        /// 内蔵ブラウザの UA 書式。
        /// </summary>
        [Configuration("view_useragent_format")]
        public string ViewUserAgentFormat { get; } = default!;
        /// <summary>
        /// HTTP直接通信時の UA 書式。
        /// </summary>
        [Configuration("client_useragent_format")]
        public string ClientUserAgentFormat { get; } = default!;

        /// <summary>
        /// ウィンドウ生成(インスタンス化)時点でWEBブラウザっぽいのがあればそれに対して開発者ツールを呼び出せる拡張処理を行うか。
        /// <para>複数あったり動的に生成する場合は個別対応が必要。</para>
        /// </summary>
        [Configuration]
        public bool DeveloperTools { get; }

        #endregion
    }
}
