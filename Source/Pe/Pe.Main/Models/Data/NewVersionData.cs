using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using ContentTypeTextNet.Pe.Bridge.Models;

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
            "note_uri": "",
            "archive_uri": "",
            "archive_size": ,
            "archive_kind": ,
            "archive_hash_kind": "",
            "archive_hash_value": ""
        }
    ]
}
*/
namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    public interface IReadOnlyNewVersionItemData
    {
        #region property

        /// <summary>
        /// リリース日時（UTC）。
        /// </summary>
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime Release { get; }

        /// <summary>
        /// モジュールバージョン。
        /// </summary>
        public Version Version { get; }
        /// <summary>
        /// モジュールリビジョン。
        /// </summary>
        public string Revision { get; }
        /// <summary>
        /// モジュールの対象プラットフォーム。
        /// </summary>
        /// <remarks>
        /// <para>使用可能文字列: x64, x86</para>
        /// </remarks>
        public string Platform { get; }

        /// <summary>
        /// モジュールを適用可能な Pe の最低バージョン。
        /// </summary>
        /// <remarks>
        /// <para>このバージョン未満は使用・インストール不可。</para>
        /// </remarks>
        public Version MinimumVersion { get; }

        /// <summary>
        /// リリースノートのURI。
        /// </summary>
        Uri NoteUri { get; }

        /// <summary>
        /// モジュールのDL先URI。
        /// </summary>
        Uri ArchiveUri { get; }
        /// <summary>
        /// モジュールのDL時のサイズ。
        /// </summary>
        long ArchiveSize { get; }
        /// <summary>
        /// モジュールのDLファイル形式。
        /// </summary>
        /// <remarks>
        /// <para>使用可能文字列: 7z, zip</para>
        /// </remarks>
        string ArchiveKind { get; }

        /// <summary>
        /// モジュールのDLファイルのハッシュ形式。
        /// <seealso cref="https://docs.microsoft.com/ja-jp/dotnet/api/system.security.cryptography.hashalgorithm.create?view=netframework-4.8#System_Security_Cryptography_HashAlgorithm_Create_System_String_"/>
        /// </summary>
        string ArchiveHashKind { get; }
        /// <summary>
        /// モジュールのDLファイルのハッシュ値。
        /// </summary>
        string ArchiveHashValue { get; }

        #endregion
    }

    [Serializable, DataContract]
    public class NewVersionItemData: IReadOnlyNewVersionItemData
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

        #region IReadOnlyNewVersionItemData

        [DateTimeKind(DateTimeKind.Utc)]
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
        [JsonPropertyName("note_uri")]
        public Uri NoteUri { get; set; } = IgnoreUri;

        [DataMember]
        [JsonPropertyName("archive_uri")]
        public Uri ArchiveUri { get; set; } = IgnoreUri;
        [DataMember]
        [JsonPropertyName("archive_size")]
        public long ArchiveSize { get; set; }
        [DataMember]
        [JsonPropertyName("archive_kind")]
        public string ArchiveKind { get; set; } = string.Empty;
        [DataMember]
        [JsonPropertyName("archive_hash_kind")]
        public string ArchiveHashKind { get; set; } = string.Empty;
        [DataMember]
        [JsonPropertyName("archive_hash_value")]
        public string ArchiveHashValue { get; set; } = string.Empty;

        #endregion
    }

    [Serializable, DataContract]
    public class NewVersionData
    {
        #region property

        [DataMember]
        [JsonPropertyName("items")]
        public NewVersionItemData[] Items { get; set; } = Array.Empty<NewVersionItemData>();

        #endregion
    }

    public enum NewVersionState
    {
        /// <summary>
        /// なにもしてない。
        /// </summary>
        [EnumResource]
        None,
        /// <summary>
        /// チェック中。
        /// </summary>
        [EnumResource]
        Checking,
        /// <summary>
        /// DL中。
        /// </summary>
        [EnumResource]
        Downloading,
        /// <summary>
        /// 検査中。
        /// </summary>
        [EnumResource]
        Checksumming,
        /// <summary>
        /// 展開中。
        /// </summary>
        [EnumResource]
        Extracting,
        /// <summary>
        /// 完了。
        /// </summary>
        [EnumResource]
        Ready,
        /// <summary>
        /// 異常あり。
        /// </summary>
        [EnumResource]
        Error,
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
}
