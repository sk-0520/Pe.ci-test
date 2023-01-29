using System;
using System.Runtime.Serialization;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    /// <summary>
    /// アプリケーション実行設定。
    /// </summary>
    [Serializable, DataContract]
    public class SettingAppExecuteSettingData
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
