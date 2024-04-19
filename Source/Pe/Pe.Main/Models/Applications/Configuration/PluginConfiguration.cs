using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    /// <summary>
    /// アプリケーション構成: プラグイン。
    /// </summary>
    public class PluginConfiguration: ConfigurationBase
    {
        public PluginConfiguration(IConfigurationSection section)
            : base(section)
        { }

        #region property

        /// <summary>
        /// プラグインとなり得る拡張子。
        /// </summary>
        /// <remarks>
        /// <para>先に一致したものを優先する。</para>
        /// </remarks>
        [Configuration]
        public IReadOnlyList<string> Extensions { get; } = default!;
        
        /// <summary>
        /// プラグインとしてそもそも無視するファイル名(拡張子抜き)。
        /// </summary>
        [Configuration]
        public IReadOnlyList<string> IgnoreBaseFileNames { get; } = default!;

        #endregion
    }
}
