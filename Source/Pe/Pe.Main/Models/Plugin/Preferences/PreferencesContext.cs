using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using ContentTypeTextNet.Pe.Bridge.ViewModels;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Preferences
{
    /// <inheritdoc cref="IPreferencesLoadContext"/>
    public class PreferencesLoadContext: PluginIdentifiersContextBase, IPreferencesLoadContext
    {
        #region variable

        readonly PluginStorage _storage;

        #endregion

        public PreferencesLoadContext(IPluginIdentifiers pluginIdentifiers, PluginStorage storage)
            : base(pluginIdentifiers)
        {
            this._storage = storage;
        }

        #region IPreferencesLoadContext

        /// <inheritdoc cref="IPreferencesLoadContext.Storage"/>
        public PluginStorage Storage => GetValue(this._storage);
        IPluginStorage IPreferencesLoadContext.Storage => Storage;

        #endregion
    }

    /// <inheritdoc cref="IPreferencesCheckContext"/>
    public class PreferencesCheckContext: PluginIdentifiersContextBase, IPreferencesCheckContext
    {
        #region variable

        readonly PluginStorage _storage;

        #endregion

        public PreferencesCheckContext(IPluginIdentifiers pluginIdentifiers, PluginStorage storage)
            : base(pluginIdentifiers)
        {
            this._storage = storage;
        }

        #region IPreferencesCheckContext

        /// <inheritdoc cref="IPreferencesCheckContext.Storage"/>
        public PluginStorage Storage => GetValue(this._storage);
        IPluginStorage IPreferencesCheckContext.Storage => Storage;

        /// <inheritdoc cref="IPreferencesCheckContext.HasError"/>
        public bool HasError { get; set; }

        #endregion
    }

    /// <inheritdoc cref="IPreferencesSaveContext"/>
    public class PreferencesSaveContext: PluginIdentifiersContextBase, IPreferencesSaveContext
    {
        #region variable

        readonly PluginStorage _storage;

        #endregion

        public PreferencesSaveContext(IPluginIdentifiers pluginIdentifiers, PluginStorage storage)
            : base(pluginIdentifiers)
        {
            this._storage = storage;
        }

        #region IPreferencesSaveContext

        /// <inheritdoc cref="IPreferencesSaveContext.Storage"/>
        public PluginStorage Storage => GetValue(this._storage);
        IPluginStorage IPreferencesSaveContext.Storage => Storage;

        #endregion
    }

    /// <inheritdoc cref="IPreferencesEndContext"/>
    public class PreferencesEndContext: PluginIdentifiersContextBase, IPreferencesEndContext
    {
        #region variable

        readonly PluginStorage _storage;

        #endregion

        public PreferencesEndContext(IPluginIdentifiers pluginIdentifiers, PluginStorage storage)
            : base(pluginIdentifiers)
        {
            this._storage = storage;
        }

        #region IPreferencesEndContext

        /// <inheritdoc cref="IPreferencesEndContext.Storage"/>
        public PluginStorage Storage => GetValue(this._storage);
        IPluginStorage IPreferencesEndContext.Storage => Storage;

        /// <inheritdoc cref="IPreferencesEndContext.IsSaved"/>
        public bool IsSaved { get; }

        #endregion
    }

}
