using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    /// <inheritdoc cref="IPluginConstructorContext"/>
    public class PluginConstructorContext: IPluginConstructorContext
    {
        public PluginConstructorContext(ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
        }

        #region IPluginConstructorContext

        /// <inheritdoc cref="IPluginConstructorContext.LoggerFactory"/>
        public ILoggerFactory LoggerFactory { get; }

        #endregion
    }

    public abstract class PluginIdentifiersContextBase
    {
        protected PluginIdentifiersContextBase(IPluginIdentifiers pluginIdentifiers)
        {
            PluginIdentifiers = pluginIdentifiers;
        }

        #region public

        public IPluginIdentifiers PluginIdentifiers { get; }

        #endregion
    }

    /// <inheritdoc cref="IPluginInitializeContext"/>
    public class PluginInitializeContext: PluginIdentifiersContextBase, IPluginInitializeContext
    {
        public PluginInitializeContext(IPluginIdentifiers pluginIdentifiers, PluginStorage storage)
            : base(pluginIdentifiers)
        {
            Storage = storage;
        }

        #region IPluginInitializeContext

        /// <inheritdoc cref="IPluginInitializeContext.Storage"/>
        public PluginStorage Storage { get; }
        IPluginStorage IPluginInitializeContext.Storage => Storage;

        #endregion
    }

    /// <inheritdoc cref="IPluginUninitializeContext"/>
    public class PluginUninitializeContext: PluginIdentifiersContextBase, IPluginUninitializeContext
    {
        public PluginUninitializeContext(IPluginIdentifiers pluginIdentifiers, PluginStorage storage)
            : base(pluginIdentifiers)
        {
            Storage = storage;
        }

        #region IPluginUninitializeContext

        /// <inheritdoc cref="IPluginUninitializeContext.Storage"/>
        public PluginStorage Storage { get; }
        IPluginStorage IPluginUninitializeContext.Storage => Storage;

        #endregion
    }

    /// <inheritdoc cref="IPluginContext"/>
    public class PluginContext: PluginIdentifiersContextBase, IPluginContext
    {
        public PluginContext(IPluginIdentifiers pluginIdentifiers, PluginStorage storage, IUserAgentFactory userAgentFactory)
            : base(pluginIdentifiers)
        {
            Storage = storage;
            UserAgentFactory = userAgentFactory;
        }

        #region IPluginContext

        /// <inheritdoc cref="IPluginContext.Storage"/>
        public PluginStorage Storage { get; }
        IPluginStorage IPluginContext.Storage => Storage;

        /// <inheritdoc cref="IPluginContext.UserAgentFactory"/>
        public IUserAgentFactory UserAgentFactory { get; }

        #endregion
    }
}
