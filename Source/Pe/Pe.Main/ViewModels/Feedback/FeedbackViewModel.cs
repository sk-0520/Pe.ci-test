using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.Feedback;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Feedback
{
    public class FeedbackViewModel : ElementViewModelBase<FeedbackElement>
    {
        public FeedbackViewModel(FeedbackElement model, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        { }

        #region property

        #endregion

        #region command

        #endregion

        #region function

        #endregion
    }
}
