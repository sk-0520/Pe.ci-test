using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    public class WidgetAddonCreateContext: PluginIdentifiersContextBase, IWidgetAddonCreateContext
    {
        #region variable

        private readonly PluginStorage _storage;

        #endregion

        public WidgetAddonCreateContext(IPluginIdentifiers pluginIdentifiers, PluginStorage storage)
            : base(pluginIdentifiers)
        {
            this._storage = storage;
        }

        #region IWidgetAddonCreateContext

        public PluginStorage Storage => GetValue(this._storage);
        IPluginStorage IWidgetAddonCreateContext.Storage => Storage;

        #endregion
    }

    public class WidgetAddonClosedContext: PluginIdentifiersContextBase, IWidgetAddonClosedContext
    {
        #region variable

        private readonly PluginStorage _storage;

        #endregion

        public WidgetAddonClosedContext(IPluginIdentifiers pluginIdentifiers, PluginStorage storage)
            : base(pluginIdentifiers)
        {
            this._storage = storage;
        }

        #region IWidgetAddonClosedContext

        public PluginStorage Storage => GetValue(this._storage);
        IPluginStorage IWidgetAddonClosedContext.Storage => Storage;

        #endregion
    }
}
