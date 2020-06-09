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
        public LauncherItemAddonContext(IPluginIdentifiers pluginIdentifiers, LauncherItemAddonStorage storage)
            :base(pluginIdentifiers)
        {
            Storage = storage;
        }

        #region ILauncherItemAddonContext

        /// <inheritdoc cref="ILauncherItemAddonContext.Storage"/>
        public LauncherItemAddonStorage Storage { get; }
        ILauncherItemAddonStorage ILauncherItemAddonContext.Storage => Storage;

        #endregion
    }
}
