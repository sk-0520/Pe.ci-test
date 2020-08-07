using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    public class LauncherItemAddonContext: PluginIdentifiersContextBase, ILauncherItemAddonContext
    {
        #region variable

        readonly LauncherItemAddonStorage _storage;

        #endregion

        public LauncherItemAddonContext(IPluginIdentifiers pluginIdentifiers, Guid launcherItemId, LauncherItemAddonStorage storage)
            : base(pluginIdentifiers)
        {
            LauncherItemId = launcherItemId;
            this._storage = storage;
        }

        #region ILauncherItemAddonContext

        /// <inheritdoc cref="ILauncherItemAddonContext.LauncherItemId"/>
        public Guid LauncherItemId { get; }

        /// <inheritdoc cref="ILauncherItemAddonContext.Storage"/>
        public LauncherItemAddonStorage Storage => GetValue(this._storage);
        ILauncherItemAddonStorage ILauncherItemAddonContext.Storage => Storage;

        #endregion
    }
}
