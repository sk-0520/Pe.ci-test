using System;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    public static class PluginUtility
    {
        #region property

        public static int CheckVersionStep { get; } = 10;

        #endregion

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
