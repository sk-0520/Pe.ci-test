using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    public class LauncherItemConfiguration: ConfigurationBase
    {
        public LauncherItemConfiguration(IConfigurationSection section)
            : base(section)
        {
            //IconRefreshTime = section.GetValue<TimeSpan>("icon_refresh_time");
            AutoImportUntargetPatterns = GetList<string>(section, "auto_import_untarget_patterns");
        }

        #region property

        [Configuration]
        public TimeSpan IconRefreshTime { get; }
        /// <summary>
        /// 自動登録対象外ファイルパターン。
        /// <para>正規表現・大文字小文字を区別しない。</para>
        /// </summary>
        public IReadOnlyList<string> AutoImportUntargetPatterns { get; }

        #endregion
    }
}
