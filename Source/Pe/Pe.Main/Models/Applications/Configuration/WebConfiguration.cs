using System;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    public class WebConfiguration: ConfigurationBase
    {
        public WebConfiguration(IConfigurationSection section)
            : base(section)
        { }

        #region property

        [Configuration("view_useragent_format")]
        public string ViewUserAgentFormat { get; } = default!;
        [Configuration("client_useragent_format")]
        public string ClientUserAgentFormat { get; } = default!;

        /// <summary>
        /// ウィンドウ生成(インスタンス化)時点でWEBブラウザっぽいのがあればそれに対して開発者ツールを呼び出せる拡張処理を行うか。
        /// <para>複数あったり動的に生成する場合は個別対応が必要。</para>
        /// </summary>
        [Configuration]
        public bool DeveloperTools { get; }

        /// <summary>
        /// プロキシを使用するか。
        /// </summary>
        [Configuration]
        public bool ProxyIsEnabled { get; }
        /// <summary>
        /// プロキシ設定情報。
        /// </summary>
        [Configuration]
        public Uri ProxyUri { get; } = default!;
        [Configuration]
        public bool ProxyCredentialIsEnabled { get; }
        [Configuration]
        public string ProxyCredentialUser { get; } = default!;
        [Configuration]
        public string ProxyCredentialPassword { get; } = default!;

        #endregion
    }
}
