using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    public static class PluginUtility
    {
        #region function

        /// <summary>
        /// プラグインバージョンは Pe バージョンに制限されるか。
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static bool IsUnlimitedVersion(Version version)
        {
            return version.Major == 0 && version.Minor == 0 && version.Build == 0;
        }

        public static string ConvertDirectoryName(Guid pluginId)
        {
            return pluginId.ToString("D");
        }
        public static string ConvertDirectoryName(IPluginId pluginId) => ConvertDirectoryName(pluginId.PluginId);
        public static string ConvertDirectoryName(IPluginIdentifiers pluginIdentifiers) => ConvertDirectoryName(pluginIdentifiers.PluginId);

        #endregion
    }
}
