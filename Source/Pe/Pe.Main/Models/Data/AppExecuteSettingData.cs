using ContentTypeTextNet.Pe.Core.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    /// <summary>
    /// アプリケーション実行設定。
    /// </summary>
    public class SettingAppExecuteSettingData: DataBase
    {
        public SettingAppExecuteSettingData()
        { }

        #region property

        /// <summary>
        /// ユーザーID。
        /// </summary>
        public string UserId { get; set; } = string.Empty;
        /// <summary>
        /// テレメトリーを有効にするか。
        /// </summary>
        public bool IsEnabledTelemetry { get; set; }

        #endregion
    }
}
