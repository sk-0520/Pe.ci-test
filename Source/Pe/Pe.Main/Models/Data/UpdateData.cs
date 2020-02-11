using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Data;

/*
こんな構造
{
    "items": [
        {
            "release": "utc",
            "version": "x.x.x",
            "revision": "commit",
            "platform": "x64",
            "minimum_version": "y.y.y"
            "note_kind": "",
            "note_uri": "",
            "archive_uri": "",
            "archive_size": ,
            "archive_hash_kind": "",
            "archive_hash_value": ""
        }
    ]
}
*/
namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    public interface IReadOnlyUpdateItemData
    {
        #region property

        public DateTime Release { get; }

        public Version Version { get; }
        public string Revision { get; }

        public string Platform { get; }

        public Version MinimumVersion { get; }

        string NoteMime { get; }

        Uri NoteUri { get; }

        Uri ArchiveUri { get; }
        long ArchiveSize { get; }
        string ArchiveHashKind { get; }
        string ArchiveHashValue { get; }

        #endregion
    }

    [Serializable, DataContract]
    public class UpdateItemData : DataBase, IReadOnlyUpdateItemData
    {
        #region property

        public static Uri IgnoreUri { get; } = new Uri("https://example.com/");

        [DataMember]
        [JsonPropertyName("version")]
        public string _Version { get; set; } = string.Empty;

        [DataMember]
        [JsonPropertyName("minimum_version")]
        public string _MinimumVersion { get; set; } = string.Empty;

        #endregion

        #region IReadOnlyUpdateItemData

        [Timestamp(DateTimeKind.Utc)]
        [DataMember]
        [JsonPropertyName("release")]
        public DateTime Release { get; set; }

        [IgnoreDataMember, JsonIgnore]
        public Version Version
        {
            get => Version.Parse(_Version);
            set => _Version = value.ToString();
        }

        [DataMember]
        [JsonPropertyName("revision")]
        public string Revision { get; set; } = string.Empty;

        [DataMember]
        [JsonPropertyName("platform")]
        public string Platform { get; set; } = string.Empty;

        [IgnoreDataMember, JsonIgnore]
        public Version MinimumVersion
        {
            get => Version.Parse(_MinimumVersion);
            set => _MinimumVersion = value.ToString();
        }

        [DataMember]
        [JsonPropertyName("note_mime")]
        public string NoteMime { get; set; } = string.Empty;

        [DataMember]
        [JsonPropertyName("note_uri")]
        public Uri NoteUri { get; set; } = IgnoreUri;

        [DataMember]
        [JsonPropertyName("archive_uri")]
        public Uri ArchiveUri { get; set; } = IgnoreUri;
        [DataMember]
        [JsonPropertyName("archive_size")]
        public long ArchiveSize { get; set; }
        [DataMember]
        [JsonPropertyName("archive_hash_kind")]
        public string ArchiveHashKind { get; set; } = string.Empty;
        [DataMember]
        [JsonPropertyName("archive_hash_value")]
        public string ArchiveHashValue { get; set; } = string.Empty;

        #endregion
    }

    [Serializable, DataContract]
    public class UpdateData : DataBase
    {
        #region property

        [DataMember]
        [JsonPropertyName("items")]
        public UpdateItemData[] Items { get; set; } = new UpdateItemData[0];

        #endregion
    }

    public enum UpdateState
    {
        /// <summary>
        /// なにもしてない。
        /// </summary>
        None,
        /// <summary>
        /// チェック中。
        /// </summary>
        Checking,
        /// <summary>
        /// DL中。
        /// </summary>
        Downloading,
        /// <summary>
        /// 展開中。
        /// </summary>
        Extracting,
        /// <summary>
        /// 完了。
        /// </summary>
        Ready,
    }

    /// <summary>
    /// アップデートチェック方法。
    /// </summary>
    public enum UpdateCheckKind
    {
        /// <summary>
        /// チェックのみ（更新履歴表示）
        /// </summary>
        CheckOnly,
        /// <summary>
        /// アップデート（更新履歴表示）
        /// </summary>
        Update,
        /// <summary>
        /// アップデート（更新履歴非表示）
        /// </summary>
        ForceUpdate
    }

    public static class ReleaseNoteMime
    {
        #region property

        public const string Json = "application/json";

        #endregion
    }

    public enum ReleaseNoteContentKind
    {
        [DataMember(Name = "unknown")]
        [EnumResource]
        Unknown,
        [DataMember(Name = "note")]
        [EnumResource]
        Note,
        [DataMember(Name = "features")]
        [EnumResource]
        Features,
        [DataMember(Name = "fixes")]
        [EnumResource]
        Fixes,
        [DataMember(Name = "developer")]
        [EnumResource]
        Developer,
    }

    public enum ReleaseNoteLogKind
    {
        [DataMember(Name = "none")]
        [EnumResource]
        None,
        [DataMember(Name = "package")]
        [EnumResource]
        Package,
        [DataMember(Name = "compatibility")]
        [EnumResource]
        Compatibility,
    }

    [Serializable, DataContract]
    public class ReleaseNoteLogItemData : DataBase
    {
        #region property

        [DataMember(Name = "revision")]
        [JsonPropertyName("revision")]
        public string Revision { get; set; } = string.Empty;

        [DataMember(Name = "kind")]
        [JsonPropertyName("kind")]
        public string _Kind { get; set; } = string.Empty;
        [IgnoreDataMember, JsonIgnore]
        public ReleaseNoteLogKind Kind
        {
            get => EnumUtility.TryParse(_Kind, out ReleaseNoteLogKind kind) ? kind : ReleaseNoteLogKind.None;
            set => _Kind = value.ToString();
        }

        [DataMember(Name = "subject")]
        [JsonPropertyName("subject")]
        public string Subject { get; set; } = string.Empty;

        [DataMember(Name = "comments")]
        [JsonPropertyName("comments")]
        public string[] Comments { get; set; } = new string[0];

        #endregion
    }

    [Serializable, DataContract]
    public class ReleaseNoteContentData : DataBase
    {
        #region property

        [DataMember(Name = "kind")]
        [JsonPropertyName("kind")]
        public string _Kind { get; set; } = string.Empty;
        [IgnoreDataMember, JsonIgnore]
        public ReleaseNoteContentKind Kind
        {
            get => EnumUtility.TryParse(_Kind, out ReleaseNoteContentKind kind) ? kind : ReleaseNoteContentKind.Unknown;
            set => _Kind = value.ToString();
        }

        [DataMember(Name = "logs")]
        [JsonPropertyName("logs")]
        public ReleaseNoteLogItemData[]? Logs { get; set; }

        #endregion
    }

    [Serializable, DataContract]
    public class ReleaseNoteItemData : DataBase
    {
        #region property

        [DataMember(Name = "date")]
        [JsonPropertyName("date")]
        public string _Date { get; set; } = string.Empty;
        [IgnoreDataMember, JsonIgnore]
        [Timestamp(DateTimeKind.Utc)]
        public DateTime Date
        {
            //get => DateTime.SpecifyKind(DateTime.Parse(_Date), DateTimeKind.Utc);
            get => DateTime.Parse(_Date).ToUniversalTime();
            set => _Date = value.ToString();
        }

        [DataMember(Name = "version")]
        [JsonPropertyName("version")]
        public string _Version { get; set; } = string.Empty;
        [IgnoreDataMember, JsonIgnore]
        public Version Version
        {
            get => Version.Parse(_Version);
            set => _Version = value.ToString();
        }

        [DataMember(Name = "contents", IsRequired = true)]
        [JsonPropertyName("contents")]
        public ReleaseNoteContentData[] Contents { get; set; } = new ReleaseNoteContentData[0];

        #endregion
    }


}

