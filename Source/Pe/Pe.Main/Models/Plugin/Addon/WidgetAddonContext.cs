using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    internal class WidgetAddonCreateContext: PluginIdentifiersContextBase, IWidgetAddonCreateContext
    {
        public WidgetAddonCreateContext(IPluginIdentifiers pluginIdentifiers, PluginStorage storage)
            : base(pluginIdentifiers)
        {
            Storage = storage;
        }

        #region IWidgetAddonCreateContext

        public PluginStorage Storage { get; }
        IPluginStorage IWidgetAddonCreateContext.Storage => Storage;

        #endregion
    }

    internal class WidgetAddonClosedContext: PluginIdentifiersContextBase, IWidgetAddonClosedContext
    {
        public WidgetAddonClosedContext(IPluginIdentifiers pluginIdentifiers, PluginStorage storage)
            : base(pluginIdentifiers)
        {
            Storage = storage;
        }

        #region IWidgetAddonClosedContext

        public PluginStorage Storage { get; }
        IPluginStorage IWidgetAddonClosedContext.Storage => Storage;

        #endregion
    }
}
