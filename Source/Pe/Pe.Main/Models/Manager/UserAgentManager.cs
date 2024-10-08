using System;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Library.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Manager
{
    public interface IUserAgentManager: IHttpUserAgentFactory, IApplicationHttpUserAgentFactory
    {
        #region function

        #endregion
    }

    public class UserAgentManager: ManagerBase, IUserAgentManager
    {
        public UserAgentManager(IDiContainer diContainer, ILoggerFactory loggerFactory)
            : base(diContainer, loggerFactory)
        {
            UserAgentFactory = DiContainer.Build<HttpUserAgentFactory>();
        }

        #region property

        private HttpUserAgentFactory UserAgentFactory { get; }

        private string AppName { get; } = BuildStatus.Name;

        #endregion

        #region function

        #endregion

        #region ManagerBase

        #endregion

        #region IApplicationUserAgentFactory

        internal HttpUserAgent CreateAppUserAgent() => UserAgentFactory.CreateUserAgent(AppName);
        IHttpUserAgent IApplicationHttpUserAgentFactory.CreateAppHttpUserAgent() => CreateAppUserAgent();

        #endregion

        #region IUserAgentManager

        #region IUserAgentFactory

        public IHttpUserAgentName UserAgentName => UserAgentFactory.UserAgentName;

        internal HttpUserAgent CreateUserAgent() => UserAgentFactory.CreateUserAgent();
        IHttpUserAgent IHttpUserAgentFactory.CreateUserAgent() => CreateUserAgent();

        internal HttpUserAgent CreateUserAgent(string name)
        {
            if(name == AppName) {
                throw new ArgumentException(null, nameof(name));
            }

            return UserAgentFactory.CreateUserAgent(name);
        }
        IHttpUserAgent IHttpUserAgentFactory.CreateUserAgent(string name) => CreateUserAgent(name);

        #endregion

        #endregion
    }
}
