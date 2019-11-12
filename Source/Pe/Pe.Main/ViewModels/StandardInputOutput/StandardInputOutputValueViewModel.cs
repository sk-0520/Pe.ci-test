using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.StandardInputOutput
{
    public class StandardInputOutputValueViewModel : ViewModelBase
    {
        public StandardInputOutputValueViewModel(string value, [Timestamp(DateTimeKind.Utc)] DateTime timestamp, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Value = value;
            Timestamp = timestamp;
        }

        #region property

        public string Value { get; }
        [Timestamp(DateTimeKind.Utc)]
        public DateTime Timestamp { get; }

        #endregion

        #region ViewModelBase

        public override string ToString() => Value;

        #endregion
    }
}
