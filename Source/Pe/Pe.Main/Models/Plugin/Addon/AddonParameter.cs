using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    /// <inheritdoc cref="IAddonParameter"/>
    public class AddonParameter: PluginParameterBase, IAddonParameter
    {
        public AddonParameter(IPluginInformations pluginInformations, IUserAgentFactory userAgentFactory, IPlatformTheme platformTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(pluginInformations, platformTheme, dispatcherWrapper, loggerFactory)
        {
            UserAgentFactory = userAgentFactory;
            AddonExecutor = new AddonExecutor(PluginInformations, LoggerFactory);
        }

        #region IAddonParameter

        public IUserAgentFactory UserAgentFactory { get; }
        public IAddonExecutor AddonExecutor { get; }
        #endregion
    }
}
