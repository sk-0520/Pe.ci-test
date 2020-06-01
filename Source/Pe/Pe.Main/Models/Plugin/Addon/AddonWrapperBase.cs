using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    internal abstract class AddonWrapperBase: DisposerBase
    {
        protected AddonWrapperBase(IReadOnlyList<IAddon> addons, EnvironmentParameters environmentParameters, IUserAgentManager userAgentManager, IPlatformTheme platformTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            EnvironmentParameters = environmentParameters;
            UserAgentManager = userAgentManager;
            PlatformTheme = platformTheme;
            DispatcherWrapper = dispatcherWrapper;
            Addons = addons;
        }

        #region property

        protected ILoggerFactory LoggerFactory { get; }
        protected ILogger Logger { get; }
        protected EnvironmentParameters EnvironmentParameters { get; }
        protected IUserAgentManager UserAgentManager { get; }
        protected IPlatformTheme PlatformTheme{get;}
        protected IDispatcherWrapper DispatcherWrapper { get; }


        protected IReadOnlyList<IAddon> Addons { get; }


        #endregion

        #region function

        protected AddonParameter CreateParameter() => new AddonParameter(PlatformTheme, DispatcherWrapper, LoggerFactory);


        #endregion
    }
}
