using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    /// <summary>
    /// アプリケーション構成: バックアップ。
    /// </summary>
    public class BackupConfiguration: ConfigurationBase
    {
        public BackupConfiguration(IConfigurationSection section)
            : base(section)
        { }

        #region property

        /// <summary>
        /// 設定ファイル保持数。
        /// </summary>
        [Configuration]
        public int SettingCount { get; }
        /// <summary>
        /// アーカイブ保持数。
        /// </summary>
        [Configuration]
        public int ArchiveCount { get; }

        #endregion
    }
}
