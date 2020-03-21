using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
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
}
