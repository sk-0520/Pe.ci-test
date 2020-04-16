using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.NotifyLog;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.NotifyLog
{
    public class NotifyLogItemViewModel : ElementViewModelBase<NotifyLogItemElement>
    {
        public NotifyLogItemViewModel(NotifyLogItemElement model, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        {
        }

        #region property
        public NotifyLogKind Kind => Model.Kind;
        public string Header => Model.Header;
        public NotifyLogContentViewModel Content => new NotifyLogContentViewModel(Model.Content, LoggerFactory);

        #endregion

        #region command

        #endregion

        #region function

        #endregion

        #region ElementViewModelBase

        #endregion
    }
}
