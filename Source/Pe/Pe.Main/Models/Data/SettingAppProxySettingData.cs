using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    public record class SettingAppProxySettingData
    {
        #region property

        public bool ProxyIsEnabled { get; init; }
        public string ProxyUrl { get; set; } = string.Empty;
        public bool CredentialIsEnabled { get; init; }
        public string CredentialUser { get; set; } = string.Empty;
        public string CredentialPassword { get; set; } = string.Empty;

        #endregion
    }
}
