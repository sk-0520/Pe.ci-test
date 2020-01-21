using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Manager
{
    public interface IUserAgentManager
    {
        #region function

        #endregion
    }

    public class UserAgentManager : ManagerBase, IUserAgentManager
    {
        public UserAgentManager(IDiContainer diContainer, ILoggerFactory loggerFactory)
            : base(diContainer, loggerFactory)
        { }

        #region property

        #endregion

        #region function

        #endregion

        #region ManagerBase

        #endregion

        #region IUserAgentManager

        #endregion
    }
}
