using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Manager;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    public class PluginInitializeContext: IPluginInitializeContext
    {
        public PluginInitializeContext(IPluginIdentifiers pluginIdentifiers, PluginStorage storage)
        {
            PluginIdentifiers = pluginIdentifiers;
            Storage = storage;
        }

        #region property

        public IPluginIdentifiers PluginIdentifiers { get; }

        #endregion

        #region IPluginInitializeContext

        public PluginStorage Storage { get; }
        IPluginStorage IPluginInitializeContext.Storage => Storage;

        #endregion
    }

    public class PluginContext : IPluginContext
    {
        public PluginContext(IPluginIdentifiers pluginIdentifiers, PluginStorage storage, IUserAgentFactory userAgentFactory)
        {
            PluginIdentifiers = pluginIdentifiers;
            Storage = storage;
            UserAgentFactory = userAgentFactory;
        }

        #region property

        public IPluginIdentifiers PluginIdentifiers { get; }

        #endregion

        #region IPluginContext

        public PluginStorage Storage { get; }
        IPluginStorage IPluginContext.Storage => Storage;

        public IUserAgentFactory UserAgentFactory { get; }

        #endregion
    }
}
