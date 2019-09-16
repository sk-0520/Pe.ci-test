using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.ViewModels
{
    public class RequestSender : ViewModelBase
    {
        public RequestSender(ILogger logger)
            : base(logger)
        { }

        public RequestSender(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        { }

        #region function

        public void Send(Action response)
        {

        }

        #endregion
    }
}
