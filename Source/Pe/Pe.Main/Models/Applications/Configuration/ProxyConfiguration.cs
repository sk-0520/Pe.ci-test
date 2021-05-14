using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    public class ProxyConfiguration: ConfigurationBase
    {
        public ProxyConfiguration(IConfiguration conf)
            : base(conf)
        { }

        #region property

        /// <summary>
        /// プロキシを使用するか。
        /// </summary>
        [Configuration]
        public bool IsEnabled { get; }
        /// <summary>
        /// プロキシ設定情報。
        /// </summary>
        [Configuration]
        public Uri Uri { get; } = default!;
        [Configuration]
        public bool CredentialIsEnabled { get; }
        [Configuration]
        public string CredentialUser { get; } = default!;
        [Configuration]
        public string CredentialPassword { get; } = default!;

        #endregion
    }
}
