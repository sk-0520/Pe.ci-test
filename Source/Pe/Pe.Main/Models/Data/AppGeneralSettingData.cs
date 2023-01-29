using System;
using System.Runtime.Serialization;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    /// <summary>
    /// 基本設定。
    /// </summary>
    [Serializable, DataContract]
    public class SettingAppGeneralSettingData
    {
        public SettingAppGeneralSettingData()
        { }

        #region property

        /// <summary>
        /// 使用言語。
        /// </summary>
        public string Language { get; set; } = string.Empty;

        /// <summary>
        /// ユーザー設定ディレクトリパス。
        /// </summary>

        public string UserBackupDirectoryPath { get; set; } = string.Empty;

        /// <summary>
        /// 使用テーマのプラグインID。
        /// </summary>
        public PluginId ThemePluginId { get; set; }

        #endregion
    }

    /// <summary>
    /// 初回実行データ。
    /// </summary>
    public class AppGeneralFirstData
    {
        #region property

        /// <summary>
        /// 初回実行バージョン。
        /// </summary>
        public Version FirstExecuteVersion { get; set; } = new Version();

        /// <summary>
        /// 初回実行日時。
        /// </summary>
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime FirstExecuteTimestamp { get; set; }


        #endregion
    }
}
