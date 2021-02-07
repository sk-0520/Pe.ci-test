using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using ContentTypeTextNet.Pe.Core.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    public enum FeedbackKind
    {
        /// <summary>
        /// 不具合報告。
        /// </summary>
        [EnumResource]
        Bug,
        /// <summary>
        /// 提案。
        /// </summary>
        [EnumResource]
        Proposal,
        /// <summary>
        /// その他。
        /// </summary>
        [EnumResource]
        Others,
    }

    [Serializable, DataContract]
    public class FeedbackInputData: DataBase
    {
        #region property

        [DataMember]
        [JsonPropertyName("kind")]
        public FeedbackKind Kind { get; set; }

        [DataMember]
        [JsonPropertyName("subject")]
        public string Subject { get; set; } = string.Empty;

        [DataMember]
        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

        #endregion
    }

    [Serializable, DataContract]
    public class FeedbackSendData: DataBase
    {
        #region property


        #region input

        [DataMember]
        [JsonPropertyName("kind")]
        public string Kind { get; set; } = string.Empty;

        [DataMember]
        [JsonPropertyName("subject")]
        public string Subject { get; set; } = string.Empty;

        [DataMember]
        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

        #endregion

        #region auto

        [DataMember]
        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; } = string.Empty;

        [DataMember]
        [JsonPropertyName("version")]
        public string Version { get; set; } = string.Empty;

        [DataMember]
        [JsonPropertyName("revision")]
        public string Revision { get; set; } = string.Empty;

        [DataMember]
        [JsonPropertyName("build")]
        public string Build { get; set; } = string.Empty;

        #endregion

        #region setting

        [DataMember]
        [JsonPropertyName("user_id")]
        public string UserId { get; set; } = string.Empty;

        [DataMember]
        [JsonPropertyName("first_execute_timestamp")]
        public string FirstExecuteTimestamp { get; set; } = string.Empty;

        [DataMember]
        [JsonPropertyName("first_execute_version")]
        public string FirstExecuteVersion { get; set; } = string.Empty;

        #endregion

        #region platform

        [DataMember]
        [JsonPropertyName("process")]
        public string Process { get; set; } = string.Empty;

        [DataMember]
        [JsonPropertyName("platform")]
        public string Platform { get; set; } = string.Empty;

        [DataMember]
        [JsonPropertyName("os")]
        public string Os { get; set; } = string.Empty;

        [DataMember]
        [JsonPropertyName("clr")]
        public string Clr { get; set; } = string.Empty;

        #endregion

        #endregion
    }

    [Serializable, DataContract]
    public class FeedbackResponse: DataBase
    {
        #region property

        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        #endregion
    }
}
