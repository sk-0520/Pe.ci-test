using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.CrashReport.Models.Data
{
    [Serializable, DataContract]
    internal class LogItem: DataBase
    {
        #region property

        [DataMember]
        public string CallerClassName { get; set; } = string.Empty;
        [DataMember]
        public string CallerMemberName { get; set; } = string.Empty;
        [DataMember]
        public string CallerFilePath { get; set; } = string.Empty;
        [DataMember]
        public int CallerLineNumber { get; set; }
        [DataMember]
        public string ExceptionString { get; set; } = string.Empty;
        [DataMember]
        public string LoggerName { get; set; } = string.Empty;
        [DataMember]
        public string FormattedMessage { get; set; } = string.Empty;
        [DataMember]
        public bool HasProperties { get; set; }
        [DataMember]
        public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
        [DataMember]
        public string Message { get; set; } = string.Empty;
        [DataMember]
        public int UserStackFrameNumber { get; set; }
        [DataMember]
        public LogLevel Level { get; set; }
        [DataMember]
        public bool HasStackTrace { get; set; }
        [DataMember]
        public int SequenceID { get; set; }
        [DataMember]
        [Timestamp(DateTimeKind.Utc)]
        public DateTime TimeStamp { get; set; }

        private static IDictionary<NLog.LogLevel, LogLevel>? LogLevelMap { get; set; }

        #endregion

        #region function

        private static LogLevel Convert(NLog.LogLevel level)
        {
            LogLevelMap ??= new Dictionary<NLog.LogLevel, LogLevel>() {
                [NLog.LogLevel.Trace] = LogLevel.Trace,
                [NLog.LogLevel.Debug] = LogLevel.Debug,
                [NLog.LogLevel.Info] = LogLevel.Information,
                [NLog.LogLevel.Warn] = LogLevel.Warning,
                [NLog.LogLevel.Error] = LogLevel.Error,
                [NLog.LogLevel.Fatal] = LogLevel.Critical,
            };
            return LogLevelMap.TryGetValue(level, out var result) ? result : LogLevel.None;
        }

        public static LogItem Create(NLog.LogEventInfo logEventInfo)
        {
            var item = new LogItem() {
                CallerClassName = logEventInfo.CallerClassName,
                CallerMemberName = logEventInfo.CallerMemberName,
                CallerFilePath = logEventInfo.CallerFilePath,
                CallerLineNumber = logEventInfo.CallerLineNumber,
                ExceptionString = logEventInfo.Exception?.ToString() ?? string.Empty,
                LoggerName = logEventInfo.LoggerName,
                FormattedMessage = logEventInfo.FormattedMessage,
                HasProperties = logEventInfo.HasProperties,
                Message = logEventInfo.Message,
                UserStackFrameNumber = logEventInfo.UserStackFrameNumber,
                Level = Convert(logEventInfo.Level),
                HasStackTrace = logEventInfo.HasStackTrace,
                SequenceID = logEventInfo.SequenceID,
                TimeStamp = logEventInfo.TimeStamp.ToUniversalTime(),
            };
            foreach(var prop in logEventInfo.Properties) {
                var key = System.Convert.ToString(prop.Key);
                if(key != null) {
                    item.Properties.Add(key, prop.Value);
                }
            }

            return item;
        }

        #endregion
    }
}
