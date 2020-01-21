using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Manager
{
    public interface IUserAgentManager
    {
        #region function

        #endregion
    }

    public class UserAgentManager : ManagerBase, IUserAgentManager, IUserAgentFactory
    {
        public UserAgentManager(IDiContainer diContainer, ILoggerFactory loggerFactory)
            : base(diContainer, loggerFactory)
        {
            UserAgentFactory = DiContainer.Build<UserAgentFactory>();
        }

        #region property

        UserAgentFactory UserAgentFactory { get; }

        #endregion

        #region function

        #endregion

        #region ManagerBase

        #endregion

        #region IUserAgentFactory

        public IUserAgent CreateUserAgent() => UserAgentFactory.CreateUserAgent();

        public IUserAgent CreateUserAgent(string name) => UserAgentFactory.CreateUserAgent(name);

        #endregion

        #region IUserAgentManager

        #endregion
    }
}
