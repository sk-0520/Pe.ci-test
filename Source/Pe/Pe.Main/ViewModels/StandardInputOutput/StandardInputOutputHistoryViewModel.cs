using System;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.StandardInputOutput
{
    public class StandardInputOutputHistoryViewModel: ViewModelBase
    {
        public StandardInputOutputHistoryViewModel(string value, [DateTimeKind(DateTimeKind.Utc)] DateTime timestamp, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Value = value;
            Timestamp = timestamp;
        }

        #region property

        public string Value { get; }
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime Timestamp { get; }

        #endregion

        #region ViewModelBase

        public override string ToString() => Value;

        #endregion
    }
}
