using System;
using System.Collections.Generic;
using ContentTypeTextNet.Pe.Bridge.Plugin;

namespace ContentTypeTextNet.Pe.Embedded.Attributes
{
    /// <summary>
    /// [アセンブリ] プラグインサポートバージョン設定。
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class PluginSupportVersionsAttribute: Attribute
    {
        /// <summary>
        /// [アセンブリ] プラグインサポートバージョン設定。
        /// </summary>
        /// <param name="minimumVersion"><see cref="IPluginVersions.MinimumSupportVersion"/>。<see cref="Version"/>へ変換可能な値を指定すること。</param>
        /// <param name="maximumVersion"><see cref="IPluginVersions.MaximumSupportVersion"/><see cref="Version"/>へ変換可能な値を指定すること。</param>
        /// <param name="checkUrls">バージョンアップチェックURLを指定。</param>
        public PluginSupportVersionsAttribute(string minimumVersion = "0.0.0", string maximumVersion = "0.0.0", params string[] checkUrls)
        {
            MinimumVersion = Version.Parse(minimumVersion);
            MaximumVersion = Version.Parse(maximumVersion);
            CheckUrls = checkUrls;
        }

        #region property

        /// <inheritdoc cref="IPluginVersions.MinimumSupportVersion"/>
        public Version MinimumVersion { get; }
        /// <inheritdoc cref="IPluginVersions.MaximumSupportVersion"/>
        public Version MaximumVersion { get; }
        /// <inheritdoc cref="IPluginVersions.CheckUrls"/>
        public IReadOnlyList<string> CheckUrls { get; }

        #endregion
    }
}
