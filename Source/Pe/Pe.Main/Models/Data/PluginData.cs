using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    /// <summary>
    /// プラグイン読み込み状態
    /// </summary>
    public enum PluginState
    {
        /// <summary>
        /// 読み込み対象。
        /// </summary>
        Enable,
        /// <summary>
        /// 読み込み非対象。
        /// </summary>
        Disable,
        /// <summary>
        /// バージョン不一致。
        /// </summary>
        IllegalVersion,
    }
}
