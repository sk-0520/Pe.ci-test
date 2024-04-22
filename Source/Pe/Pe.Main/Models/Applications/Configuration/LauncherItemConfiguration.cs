using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    /// <summary>
    /// アプリケーション構成: ランチャーアイテム。
    /// </summary>
    public class LauncherItemConfiguration: ConfigurationBase
    {
        public LauncherItemConfiguration(IConfigurationSection section)
            : base(section)
        { }

        #region property

        /// <summary>
        /// アイコン更新対象と判定する前回更新からの差分時間。
        /// </summary>
        [Configuration]
        public TimeSpan IconRefreshTime { get; }
        /// <summary>
        /// 自動登録対象外ファイルパターン。
        /// </summary>
        /// <remarks>
        /// <para>正規表現・大文字小文字を区別しない。</para>
        /// </remarks>
        [Configuration]
        public IReadOnlyList<string> AutoImportExcludePatterns { get; } = default!;

        #endregion
    }
}
