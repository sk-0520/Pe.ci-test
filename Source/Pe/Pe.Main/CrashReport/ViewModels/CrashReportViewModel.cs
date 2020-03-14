using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Main.CrashReport.Models.Element;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using ContentTypeTextNet.Pe.Main.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.CrashReport.ViewModels
{
    internal class CrashReportViewModel : ElementViewModelBase<CrashReportElement>
    {
        public CrashReportViewModel(CrashReportElement model, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        {
        }

        #region property

        #endregion

        #region command

        #endregion

        #region function

        #endregion
    }
}
