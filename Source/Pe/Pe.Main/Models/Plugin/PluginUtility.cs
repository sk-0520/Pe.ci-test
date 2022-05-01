using System;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    /// <summary>
    /// プラグイン処理ユーティリティ。
    /// </summary>
    public static class PluginUtility
    {
        #region property

        public static int CheckVersionStep { get; } = 10;

        #endregion

        #region function

        /// <summary>
        /// 指定プラグイン制限バージョンは Pe バージョンに関係なく使用可能か。
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static bool IsUnlimitedVersion(Version version)
        {
            return version.Major == 0 && version.Minor == 0 && version.Build == 0;
        }

        /// <summary>
        /// プラグインIDをディレクトリ名として使用可能な文字列に変換。
        /// </summary>
        /// <param name="pluginId"></param>
        /// <returns></returns>
        public static string ConvertDirectoryName(PluginId pluginId)
        {
            return pluginId.ToString();
        }
        /// <inheritdoc cref="ConvertDirectoryName(PluginId)"/>
        public static string ConvertDirectoryName(IPluginId pluginId) => ConvertDirectoryName(pluginId.PluginId);
        /// <inheritdoc cref="ConvertDirectoryName(PluginId)"/>
        public static string ConvertDirectoryName(IPluginIdentifiers pluginIdentifiers) => ConvertDirectoryName(pluginIdentifiers.PluginId);

        #endregion
    }
}
