using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
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
        [JsonPropertyName("caller_class_name")]
        public string CallerClassName { get; set; } = string.Empty;
        [DataMember]
        [JsonPropertyName("caller_member_name")]
        public string CallerMemberName { get; set; } = string.Empty;
        [DataMember]
        [JsonPropertyName("caller_file_path")]
        public string CallerFilePath { get; set; } = string.Empty;
        [DataMember]
        [JsonPropertyName("caller_line_number")]
        public int CallerLineNumber { get; set; }
        [DataMember]
        [JsonPropertyName("exception_string")]
        public string ExceptionString { get; set; } = string.Empty;
        [DataMember]
        [JsonPropertyName("logger_name")]
        public string LoggerName { get; set; } = string.Empty;
        [DataMember]
        [JsonPropertyName("formatted_message")]
        public string FormattedMessage { get; set; } = string.Empty;
        [DataMember]
        [JsonPropertyName("has_properties")]
        public bool HasProperties { get; set; }
        [DataMember]
        [JsonPropertyName("properties")]
        public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
        [DataMember]
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
        [DataMember]
        [JsonPropertyName("user_stack_frameNumber")]
        public int UserStackFrameNumber { get; set; }
        [DataMember]
        [JsonPropertyName("lebel")]
        public string Level { get; set; } = string.Empty;
        [DataMember]
        [JsonPropertyName("has_stack_trace")]
        public bool HasStackTrace { get; set; }
        [DataMember]
        [JsonPropertyName("sequence_id")]
        public int SequenceID { get; set; }
        [DataMember]
        [Timestamp(DateTimeKind.Utc)]
        [JsonPropertyName("timestamp")]
        public DateTime TimeStamp { get; set; }

        private static IDictionary<NLog.LogLevel, string>? LogLevelMap { get; set; }

        #endregion

        #region function

        private static string Convert(NLog.LogLevel level)
        {
            LogLevelMap ??= new Dictionary<NLog.LogLevel, string>() {
                [NLog.LogLevel.Trace] = LogLevel.Trace.ToString().ToLowerInvariant(),
                [NLog.LogLevel.Debug] = LogLevel.Debug.ToString().ToLowerInvariant(),
                [NLog.LogLevel.Info] = LogLevel.Information.ToString().ToLowerInvariant(),
                [NLog.LogLevel.Warn] = LogLevel.Warning.ToString().ToLowerInvariant(),
                [NLog.LogLevel.Error] = LogLevel.Error.ToString().ToLowerInvariant(),
                [NLog.LogLevel.Fatal] = LogLevel.Critical.ToString().ToLowerInvariant(),
            };
            return LogLevelMap.TryGetValue(level, out var result) ? result : LogLevel.None.ToString().ToLowerInvariant();
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
