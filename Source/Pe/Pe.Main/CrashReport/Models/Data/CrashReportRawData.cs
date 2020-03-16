using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Data;

namespace ContentTypeTextNet.Pe.Main.CrashReport.Models.Data
{
    [Serializable, DataContract]
    internal class CrashReportRawData: DataBase
    {
        #region property

        [DataMember]
        public string UserId { get; set; } = string.Empty;
        [DataMember]
        public Version Version { get; set; } = new Version();
        [DataMember]
        public string Revision { get; set; } = string.Empty;

        [DataMember]
        [Timestamp(DateTimeKind.Utc)]
        public DateTime Timestamp { get; set; }
        [DataMember]
        public string Exception { get; set; } = string.Empty;
        [DataMember]
        public Dictionary<string, Dictionary<string, object?>> Informations { get; set; } = new Dictionary<string, Dictionary<string, object?>>();
        [DataMember]
        public List<LogItem> LogItems { get; set; } = new List<LogItem>();


        #endregion
    }
}
