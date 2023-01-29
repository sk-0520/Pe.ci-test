using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ContentTypeTextNet.Pe.Main.CrashReport.Models.Data
{
    [Serializable, DataContract]
    public class CrashReportResponse
    {
        #region property

        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        #endregion
    }
}
