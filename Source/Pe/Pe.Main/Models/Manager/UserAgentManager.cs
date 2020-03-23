using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Manager
{
    public interface IUserAgentManager : IUserAgentFactory, IApplicationUserAgentFactory
    {
        #region function

        #endregion
    }

    public class UserAgentManager : ManagerBase, IUserAgentManager, IUserAgentFactory, IApplicationUserAgentFactory
    {
        public UserAgentManager(IDiContainer diContainer, ILoggerFactory loggerFactory)
            : base(diContainer, loggerFactory)
        {
            UserAgentFactory = DiContainer.Build<UserAgentFactory>();
        }

        #region property

        UserAgentFactory UserAgentFactory { get; }

        string AppName { get; } = BuildStatus.Name;

        #endregion

        #region function

        #endregion

        #region ManagerBase

        #endregion

        #region IApplicationUserAgentFactory

        internal UserAgent CreateAppUserAgent() => UserAgentFactory.CreateUserAgent(AppName);
        IUserAgent IApplicationUserAgentFactory.CreateAppUserAgent() => CreateAppUserAgent();

        #endregion

        #region IUserAgentManager

        #region IUserAgentFactory

        public IUserAgentName UserAgentName => UserAgentFactory.UserAgentName;

        internal UserAgent CreateUserAgent() => UserAgentFactory.CreateUserAgent();
        IUserAgent IUserAgentFactory.CreateUserAgent() => CreateUserAgent();

        internal UserAgent CreateUserAgent(string name)
        {
            if(name == AppName) {
                throw new ArgumentException(nameof(name));
            }

            return UserAgentFactory.CreateUserAgent(name);
        }
        IUserAgent IUserAgentFactory.CreateUserAgent(string name) => CreateUserAgent(name);

        #endregion

        #endregion
    }
}
