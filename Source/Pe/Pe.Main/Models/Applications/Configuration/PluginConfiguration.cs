using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    public class PluginConfiguration: ConfigurationBase
    {
        public PluginConfiguration(IConfigurationSection section)
            : base(section)
        {
            Extentions = GetList<string>(section, "extentions");
        }

        #region property

        /// <summary>
        /// プラグインとなり得る拡張子。
        /// <para>先に一致したものを優先する。</para>
        /// </summary>
        public IReadOnlyList<string> Extentions { get; }


        #endregion
    }
}
