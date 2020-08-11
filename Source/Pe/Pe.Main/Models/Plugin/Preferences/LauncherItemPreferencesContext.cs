using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Addon;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Preferences
{
    public class LauncherItemPreferencesLoadContext: PluginIdentifiersLauncherItemAddonContextBase, ILauncherItemPreferencesLoadContext
    {
        #region variable

        readonly LauncherItemAddonStorage _storage;

        #endregion

        public LauncherItemPreferencesLoadContext(IPluginIdentifiers pluginIdentifiers, Guid launcherItemId, LauncherItemAddonStorage storage)
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

        readonly LauncherItemAddonStorage _storage;

        #endregion

        public LauncherItemPreferencesCheckContext(IPluginIdentifiers pluginIdentifiers, Guid launcherItemId, LauncherItemAddonStorage storage)
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

        readonly LauncherItemAddonStorage _storage;

        #endregion

        public LauncherItemPreferencesSaveContext(IPluginIdentifiers pluginIdentifiers, Guid launcherItemId, LauncherItemAddonStorage storage)
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

        readonly LauncherItemAddonStorage _storage;

        #endregion

        public LauncherItemPreferencesEndContext(IPluginIdentifiers pluginIdentifiers, Guid launcherItemId, LauncherItemAddonStorage storage)
            : base(pluginIdentifiers, launcherItemId)
        {
            this._storage = storage;
        }


        #region ILauncherItemPreferencesCheckContext

        /// <inheritdoc cref="ILauncherItemPreferencesCheckContext.Storage" />
        public LauncherItemAddonStorage Storage => GetValue(this._storage);
        ILauncherItemAddonStorage ILauncherItemPreferencesEndContext.Storage => Storage;
        public bool IsSaved { get; set; }

        #endregion
    }

}
