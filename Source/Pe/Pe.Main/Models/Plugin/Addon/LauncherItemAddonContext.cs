using System;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    public abstract class PluginIdentifiersLauncherItemAddonContextBase: PluginIdentifiersContextBase, ILauncherItemId
    {
        protected PluginIdentifiersLauncherItemAddonContextBase(IPluginIdentifiers pluginIdentifiers, LauncherItemId launcherItemId)
            : base(pluginIdentifiers)
        {
            LauncherItemId = launcherItemId;
        }

        #region ILauncherItemId

        public LauncherItemId LauncherItemId { get; }

        #endregion
    }

    public class LauncherItemAddonContext: PluginIdentifiersLauncherItemAddonContextBase, ILauncherItemAddonContext
    {
        #region variable

        private readonly LauncherItemAddonStorage _storage;

        #endregion

        public LauncherItemAddonContext(IPluginIdentifiers pluginIdentifiers, LauncherItemId launcherItemId, LauncherItemAddonStorage storage)
            : base(pluginIdentifiers, launcherItemId)
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
