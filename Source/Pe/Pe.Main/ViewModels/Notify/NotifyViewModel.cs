using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Main.Models.Element.Notify;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Notify
{
    public class NotifyViewModel : ElementViewModelBase<NotifyElement>
    {
        public NotifyViewModel(NotifyElement model, INotifyTheme notifyTheme, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        {
            NotifyTheme = notifyTheme;
        }

        #region property

        INotifyTheme NotifyTheme { get; }

        #endregion

        #region command

        #endregion

        #region function

        #endregion
    }
}
