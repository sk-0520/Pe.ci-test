using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    internal class WidgetAddonCreateContext: PluginIdentifiersContextBase, IWidgetAddonCreateContext
    {
        #region variable

        readonly PluginStorage _storage;

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

    internal class WidgetAddonClosedContext: PluginIdentifiersContextBase, IWidgetAddonClosedContext
    {
        #region variable

        readonly PluginStorage _storage;

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
