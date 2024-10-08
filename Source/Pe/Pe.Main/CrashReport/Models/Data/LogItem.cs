using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.CrashReport.Models.Data
{
    [Serializable, DataContract]
    public class LogItem
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
        [JsonPropertyName("parameters")]
        public List<string?> Parameters { get; set; } = new List<string?>();
        [DataMember]
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
        [DataMember]
        [JsonPropertyName("level")]
        public string Level { get; set; } = string.Empty;
        [DataMember]
        [JsonPropertyName("stack_trace")]
        public string[] StackTrace { get; set; } = Array.Empty<string>();
        [DataMember]
        [JsonPropertyName("sequence_id")]
        public int SequenceID { get; set; }
        [DataMember]
        [DateTimeKind(DateTimeKind.Utc)]
        [JsonPropertyName("timestamp")]
        public DateTime TimeStamp { get; set; }

        private static IDictionary<NLog.LogLevel, string>? LogLevelMap { get; set; }

        #endregion

        #region function

        private static string Convert(NLog.LogLevel level)
        {
            LogLevelMap ??= new Dictionary<NLog.LogLevel, string>() {
                [NLog.LogLevel.Trace] = EnumUtility.ToString(LogLevel.Trace),
                [NLog.LogLevel.Debug] = EnumUtility.ToString(LogLevel.Debug),
                [NLog.LogLevel.Info] = EnumUtility.ToString(LogLevel.Information),
                [NLog.LogLevel.Warn] = EnumUtility.ToString(LogLevel.Warning),
                [NLog.LogLevel.Error] = EnumUtility.ToString(LogLevel.Error),
                [NLog.LogLevel.Fatal] = EnumUtility.ToString(LogLevel.Critical),
            };
            return LogLevelMap.TryGetValue(level, out var result) ? result : EnumUtility.ToString(LogLevel.None);
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
                Message = logEventInfo.Message,
                StackTrace = TextUtility.ReadLines(logEventInfo.StackTrace.ToString()).Skip(logEventInfo.UserStackFrameNumber).ToArray(),
                Level = Convert(logEventInfo.Level),
                SequenceID = logEventInfo.SequenceID,
                TimeStamp = logEventInfo.TimeStamp.ToUniversalTime(),
            };
            foreach(var parameter in logEventInfo.Parameters ?? Array.Empty<object>()) {
                item.Parameters.Add(System.Convert.ToString(parameter, CultureInfo.InvariantCulture));
            }

            return item;
        }

        #endregion
    }
}
