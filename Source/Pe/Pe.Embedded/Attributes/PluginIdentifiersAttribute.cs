using System;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Embedded.Attributes
{
    /// <summary>
    /// [アセンブリ] プラグインID設定。
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class PluginIdentifiersAttribute: Attribute
    {
        /// <summary>
        /// [アセンブリ] プラグインID設定。
        /// </summary>
        /// <param name="pluginName">プラグイン名。</param>
        /// <param name="pluginId">プラグインID。<see cref="PluginId"/>へ変換可能な値を指定すること。</param>
        public PluginIdentifiersAttribute(string pluginName, string pluginId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(pluginName);
            ArgumentNullException.ThrowIfNull(pluginId);

            PluginName = pluginName.Trim();

            if(PluginId.TryParse(pluginId, out var guid)) {
                PluginId = guid;
            } else {
                throw new ArgumentException(null, nameof(pluginId));
            }
        }

        #region property
        /// <summary>
        /// プラグイン名。
        /// </summary>
        public string PluginName { get; }
        /// <summary>
        /// プラグインID。
        /// </summary>
        public PluginId PluginId { get; }

        #endregion
    }
}
