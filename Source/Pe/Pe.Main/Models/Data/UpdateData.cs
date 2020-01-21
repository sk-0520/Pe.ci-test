using System;
using System.Collections.Generic;
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
            "platform": "x64",
            "minimum_version": "y.y.y"
            "note_uri": ""
            "archive_uri": ""
        }
    ]
}
*/
namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    [Serializable, DataContract]
    public class UpdateItemData : DataBase
    {
        #region property

        public static Uri IgnoreUri { get; } = new Uri("https://example.com/");

        [Timestamp(DateTimeKind.Utc)]
        [DataMember]
        [JsonPropertyName("release")]
        public DateTime Release { get; set; }

        [DataMember]
        [JsonPropertyName("version")]
        public string _Version { get; set; } = string.Empty;
        [IgnoreDataMember, JsonIgnore]
        public Version Version
        {
            get => Version.Parse(_Version);
            set => _Version = value.ToString();
        }

        [DataMember]
        [JsonPropertyName("minimum_version")]
        public string _MinimumVersion { get; set; } = string.Empty;
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
}
