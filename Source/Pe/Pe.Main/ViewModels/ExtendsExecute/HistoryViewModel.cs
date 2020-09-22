using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.ExtendsExecute
{
    public class HistoryViewModel : ViewModelBase
    {
        public HistoryViewModel(LauncherHistoryData data, CultureInfo cultureInfo, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            var timstampConverter = new TimestampConverter(cultureInfo);

            Value = data.Value;
            Timestamp = timstampConverter.ToViewFullString(data.LastExecuteTimestamp);
        }

        public HistoryViewModel(string value, ILoggerFactory loggerFactory)
        : base(loggerFactory)
        {
            Value = value;
            Timestamp = string.Empty;
        }

        #region property

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
