using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;
using Prism.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.NotifyLog
{
    public class NotifyLogContentViewModel : ViewModelBase
    {
        public NotifyLogContentViewModel(IReadOnlyNotifyLogContent content, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Content = content;
        }

        #region property

        IReadOnlyNotifyLogContent Content { get; }

        public string Message => Content.Message;
        [Timestamp(DateTimeKind.Utc)]
        public DateTime Timestamp => Content.Timestamp;

        #endregion

        #region command

        #endregion

        #region function

        #endregion

        #region ViewModelBase

        #endregion
    }
}
