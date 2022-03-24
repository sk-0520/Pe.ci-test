using System;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Addon;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Preferences
{
    public class LauncherItemPreferencesLoadContext: PluginIdentifiersLauncherItemAddonContextBase, ILauncherItemPreferencesLoadContext
    {
        #region variable

        private readonly LauncherItemAddonStorage _storage;

        #endregion

        public LauncherItemPreferencesLoadContext(IPluginIdentifiers pluginIdentifiers, LauncherItemId launcherItemId, LauncherItemAddonStorage storage)
            : base(pluginIdentifiers, launcherItemId)
        {
            this._storage = storage;
        }

        #region ILauncherItemPreferencesLoadContext

        /// <inheritdoc cref="ILauncherItemPreferencesLoadContext.Storage" />
        public LauncherItemAddonStorage Storage => GetValue(this._storage);
        ILauncherItemAddonStorage ILauncherItemPreferencesLoadContext.Storage => Storage;

        #endregion
    }

    public class LauncherItemPreferencesCheckContext: PluginIdentifiersLauncherItemAddonContextBase, ILauncherItemPreferencesCheckContext
    {
        #region variable

        private readonly LauncherItemAddonStorage _storage;

        #endregion

        public LauncherItemPreferencesCheckContext(IPluginIdentifiers pluginIdentifiers, LauncherItemId launcherItemId, LauncherItemAddonStorage storage)
            : base(pluginIdentifiers, launcherItemId)
        {
            this._storage = storage;
        }

        #region ILauncherItemPreferencesCheckContext

        /// <inheritdoc cref="ILauncherItemPreferencesCheckContext.Storage" />
        public LauncherItemAddonStorage Storage => GetValue(this._storage);
        ILauncherItemAddonStorage ILauncherItemPreferencesCheckContext.Storage => Storage;
        public bool HasError { get; set; }

        #endregion
    }

    public class LauncherItemPreferencesSaveContext: PluginIdentifiersLauncherItemAddonContextBase, ILauncherItemPreferencesSaveContext
    {
        #region variable

        private readonly LauncherItemAddonStorage _storage;

        #endregion

        public LauncherItemPreferencesSaveContext(IPluginIdentifiers pluginIdentifiers, LauncherItemId launcherItemId, LauncherItemAddonStorage storage)
            : base(pluginIdentifiers, launcherItemId)
        {
            this._storage = storage;
        }

        #region ILauncherItemPreferencesSaveContext

        /// <inheritdoc cref="ILauncherItemPreferencesCheckContext.Storage" />
        public LauncherItemAddonStorage Storage => GetValue(this._storage);
        ILauncherItemAddonStorage ILauncherItemPreferencesSaveContext.Storage => Storage;

        #endregion
    }

    public class LauncherItemPreferencesEndContext: PluginIdentifiersLauncherItemAddonContextBase, ILauncherItemPreferencesEndContext
    {
        #region variable

        #endregion

        public LauncherItemPreferencesEndContext(IPluginIdentifiers pluginIdentifiers, LauncherItemId launcherItemId)
            : base(pluginIdentifiers, launcherItemId)
        { }

        #region ILauncherItemPreferencesCheckContext

        public bool IsSaved { get; set; }

        #endregion
    }

}
