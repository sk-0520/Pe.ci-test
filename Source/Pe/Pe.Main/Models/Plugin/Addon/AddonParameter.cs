using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    /// <inheritdoc cref="IAddonParameter"/>
    public class AddonParameter: PluginParameterBase, IAddonParameter
    {
        public AddonParameter(IUserAgentFactory userAgentFactory, IPlatformTheme platformTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(platformTheme, dispatcherWrapper, loggerFactory)
        {
            UserAgentFactory = userAgentFactory;
        }

        #region IAddonParameter

        public IUserAgentFactory UserAgentFactory { get; }

        #endregion
    }
}
