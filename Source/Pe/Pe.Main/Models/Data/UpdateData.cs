using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
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
        [DataMember(Name = "release")]
        public DateTime Release { get; set; }

        [DataMember(Name = "version")]
        public Version Version { get; set; } = new Version();

        [DataMember(Name = "minimum_version")]
        public Version MinimumVersion { get; set; } = new Version();

        [DataMember(Name = "note_uri")]
        public Uri NoteUri { get; set; } = IgnoreUri;

        [DataMember(Name = "archive_uri")]
        public Uri ArchiveUri { get; set; } = IgnoreUri;

        #endregion
    }

    [Serializable, DataContract]
    public class UpdateData : DataBase
    {
        #region property

        [DataMember(Name = "items")]
        public UpdateItemData[] Items { get; set; } = new UpdateItemData[0];

        #endregion
    }
}
