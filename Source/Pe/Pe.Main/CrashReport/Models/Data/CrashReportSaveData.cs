using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Data;

namespace ContentTypeTextNet.Pe.Main.CrashReport.Models.Data
{
    [Serializable, DataContract]
    internal class CrashReportSaveData : DataBase
    {
        #region property

        public string UserId { get; set; } = string.Empty;
        public Version Version { get; set; } = new Version();
        public string Revision { get; set; } = string.Empty;

        public string ContactMailAddress { get; set; } = string.Empty;

        public string Comment { get; set; } = string.Empty;

        [Timestamp(DateTimeKind.Utc)]
        public DateTime Timestamp { get; set; }
        public string Exception { get; set; } = string.Empty;
        public Dictionary<string, Dictionary<string, object?>> Informations { get; set; } = new Dictionary<string, Dictionary<string, object?>>();
        public List<LogItem> LogItems { get; set; } = new List<LogItem>();

        #endregion
    }
}
