using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        PluginStatus,
    }

    public record IpcDataPluginStatus
    {
    }
}
