using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ContentTypeTextNet.Pe.Main.CrashReport.Models.Data
{
    /// <summary>
    /// クラッシュレポート送信結果。
    /// </summary>
    [Serializable, DataContract]
    public class CrashReportResponse
    {
        #region property

        /// <summary>
        /// 成功状態。
        /// </summary>
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        /// <summary>
        /// メッセージ。
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        #endregion
    }
}
