using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Feedback
{
    public class FeedbackElement : WebViewElementBase
    {
        public FeedbackElement(IOrderManager orderManager, IUserAgentManager userAgentManager, ILoggerFactory loggerFactory)
            : base(userAgentManager, loggerFactory)
        {
            OrderManager = orderManager;
        }

        #region property

        IOrderManager OrderManager { get; }

        #endregion

        #region function

        #endregion

        #region WebViewElementBase

        protected override void InitializeImpl()
        {
        }


        #endregion
    }
}
