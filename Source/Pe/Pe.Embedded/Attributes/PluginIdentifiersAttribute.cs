using System;

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
        /// <param name="pluginId">プラグインID。<see cref="Guid"/>へ変換可能な値を指定すること。</param>
        public PluginIdentifiersAttribute(string pluginName, string pluginId)
        {
            if(string.IsNullOrWhiteSpace(pluginName)) {
                throw new ArgumentException(nameof(pluginName));
            }
            PluginName = pluginName.Trim();
            if(PluginName.Length == 0) {
                throw new ArgumentException(nameof(pluginName));
            }

            if(Guid.TryParse(pluginId, out var guid)) {
                PluginId = guid;
            } else {
                throw new ArgumentException(nameof(pluginId));
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
        public Guid PluginId { get; }

        #endregion
    }
}
