using System;
using System.Globalization;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.ExtendsExecute
{
    public class HistoryViewModel: ViewModelBase
    {
        public HistoryViewModel(LauncherHistoryData data, CultureInfo cultureInfo, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            var timestampConverter = new TimestampConverter(cultureInfo);

            Value = data.Value;
            Timestamp = timestampConverter.ToViewFullString(data.LastExecuteTimestamp);
            LastExecuteTimestamp = data.LastExecuteTimestamp;
            Kind = data.Kind;
            CanRemove = true;
        }

        public HistoryViewModel(string value, ILoggerFactory loggerFactory)
        : base(loggerFactory)
        {
            Value = value;
            Timestamp = string.Empty;
            CanRemove = false;
        }

        #region property

        public LauncherHistoryKind Kind { get; }
        /// <summary>
        /// 最終使用日時。
        /// </summary>
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime LastExecuteTimestamp { get; }
        public bool CanRemove { get; }

        #endregion

        #region function

        public string Value { get; }
        public string Timestamp { get; }
        #endregion

        #region object

        public override string ToString() => Value;

        #endregion
    }
}
