using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    internal class LauncherItemAddonContext: PluginIdentifiersContextBase, ILauncherItemAddonContext
    {
        public LauncherItemAddonContext(IPluginIdentifiers pluginIdentifiers, LauncherItemAddonStorage storage, IUserAgentFactory userAgentFactory)
            :base(pluginIdentifiers)
        {
            Storage = storage;
            UserAgentFactory = userAgentFactory;
        }

        #region ILauncherItemAddonContext

        /// <inheritdoc cref="ILauncherItemAddonContext.Storage"/>
        public LauncherItemAddonStorage Storage { get; }
        ILauncherItemAddonStorage ILauncherItemAddonContext.Storage => Storage;

        /// <inheritdoc cref="ILauncherItemAddonContext.UserAgentFactory"/>
        public IUserAgentFactory UserAgentFactory { get; }

        #endregion
    }
}
