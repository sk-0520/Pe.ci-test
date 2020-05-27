using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Manager;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    /// <inheritdoc cref="IPluginInitializeContext"/>
    internal class PluginInitializeContext: IPluginInitializeContext
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

        /// <inheritdoc cref="IPluginInitializeContext.Storage"/>
        public PluginStorage Storage { get; }
        IPluginStorage IPluginInitializeContext.Storage => Storage;

        #endregion
    }

    /// <inheritdoc cref="IPluginContext"/>
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

        /// <inheritdoc cref="IPluginContext.Storage"/>
        public PluginStorage Storage { get; }
        IPluginStorage IPluginContext.Storage => Storage;

        /// <inheritdoc cref="IPluginContext.UserAgentFactory"/>
        public IUserAgentFactory UserAgentFactory { get; }

        #endregion
    }
}
