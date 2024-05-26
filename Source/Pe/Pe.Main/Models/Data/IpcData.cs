using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models.Serialization;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    /// <summary>
    /// プロセス間通信内容。
    /// </summary>
    public enum IpcMode
    {
        /// <summary>
        /// プラグインステータス。
        /// </summary>
        GetPluginStatus,
    }

    public class IpcDataPluginStatus: PluginStateData, IPluginLoadState
    {
        #region IPluginLoadState

        [JsonConverter(typeof(JsonTextSerializer.VersionConverter))]
        public Version PluginVersion { get; set; } = new Version();

        public PluginState LoadState => PluginState.Disable;

        #endregion
    }
}
