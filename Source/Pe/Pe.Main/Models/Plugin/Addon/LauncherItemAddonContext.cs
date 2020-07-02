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
        #region variable

        readonly LauncherItemAddonStorage _storage;

        #endregion

        public LauncherItemAddonContext(IPluginIdentifiers pluginIdentifiers, LauncherItemAddonStorage storage)
            :base(pluginIdentifiers)
        {
            this._storage = storage;
        }

        #region ILauncherItemAddonContext

        /// <inheritdoc cref="ILauncherItemAddonContext.Storage"/>
        public LauncherItemAddonStorage Storage => GetValue(this._storage);
        ILauncherItemAddonStorage ILauncherItemAddonContext.Storage => Storage;

        #endregion
    }
}
