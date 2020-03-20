using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element
{
    public abstract class WebViewElementBase : ElementBase
    {
        public WebViewElementBase(IUserAgentManager userAgentManager, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            UserAgentManager = userAgentManager;
        }

        #region property

        protected IUserAgentManager UserAgentManager { get; }

        #endregion

        #region function

        #endregion
    }
}
