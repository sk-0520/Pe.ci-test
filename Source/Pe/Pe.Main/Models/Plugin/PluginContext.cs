using System.Runtime.CompilerServices;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    /// <inheritdoc cref="IPluginCommonContext"/>
    public abstract class PluginCommonContextBase: DisposerBase
    {
        #region function

        /// <summary>
        /// このコンテキストが使用できない際に<see cref="PluginUnavailableContextException"/>を投げる。
        /// </summary>
        /// <param name="_callerMemberName"></param>
        protected void ThrowIfUnavailable([CallerMemberName] string _callerMemberName = "")
        {
            if(!IsAvailable) {
                throw new PluginUnavailableContextException(_callerMemberName);
            }
        }

        protected TValue GetValue<TValue>(TValue value)
        {
            ThrowIfUnavailable();
            return value;
        }

        #endregion

        #region IPluginCommonContext

        /// <inheritdoc cref="IPluginCommonContext.IsAvailable"/>
        public bool IsAvailable => !IsDisposed;

        #endregion
    }

    /// <inheritdoc cref="IPluginConstructorContext"/>
    public class PluginConstructorContext: PluginCommonContextBase, IPluginConstructorContext
    {
        #region variable

        private readonly ILoggerFactory _loggerFactory;

        #endregion

        public PluginConstructorContext(ILoggerFactory loggerFactory)
        {
            this._loggerFactory = loggerFactory;
        }

        #region IPluginConstructorContext

        /// <inheritdoc cref="IPluginConstructorContext.LoggerFactory"/>
        public ILoggerFactory LoggerFactory => GetValue(this._loggerFactory);

        #endregion
    }

    public abstract class PluginIdentifiersContextBase: PluginCommonContextBase
    {
        protected PluginIdentifiersContextBase(IPluginIdentifiers pluginIdentifiers)
        {
            PluginIdentifiers = pluginIdentifiers;
        }

        #region property

        public IPluginIdentifiers PluginIdentifiers { get; }

        #endregion
    }

    /// <inheritdoc cref="IPluginInitializeContext"/>
    public class PluginInitializeContext: PluginIdentifiersContextBase, IPluginInitializeContext
    {
        #region variable

        private readonly PluginStorage _storage;

        #endregion

        public PluginInitializeContext(IPluginIdentifiers pluginIdentifiers, PluginStorage storage)
            : base(pluginIdentifiers)
        {
            this._storage = storage;
        }

        #region IPluginInitializeContext

        /// <inheritdoc cref="IPluginInitializeContext.Storage"/>
        public PluginStorage Storage => GetValue(this._storage);
        IPluginStorage IPluginInitializeContext.Storage => Storage;

        #endregion
    }

    /// <inheritdoc cref="IPluginFinalizeContext"/>
    public class PluginFinalizeContext: PluginIdentifiersContextBase, IPluginFinalizeContext
    {
        #region variable

        private readonly PluginStorage _storage;

        #endregion

        public PluginFinalizeContext(IPluginIdentifiers pluginIdentifiers, PluginStorage storage)
            : base(pluginIdentifiers)
        {
            this._storage = storage;
        }

        #region IPluginUninitializeContext

        /// <inheritdoc cref="IPluginFinalizeContext.Storage"/>
        public PluginStorage Storage => GetValue(this._storage);
        IPluginStorage IPluginFinalizeContext.Storage => Storage;

        #endregion
    }

    /// <inheritdoc cref="IPluginFinalizeContext"/>
    public class PluginLoadContext: PluginIdentifiersContextBase, IPluginLoadContext
    {
        #region variable

        private readonly PluginStorage _storage;

        #endregion

        public PluginLoadContext(IPluginIdentifiers pluginIdentifiers, PluginStorage storage)
            : base(pluginIdentifiers)
        {
            this._storage = storage;
        }

        #region IPluginLoadContext

        /// <inheritdoc cref="IPluginLoadContext.Storage"/>
        public PluginStorage Storage => GetValue(this._storage);
        IPluginStorage IPluginLoadContext.Storage => Storage;

        #endregion
    }

    /// <inheritdoc cref="IPluginUnloadContext"/>
    public class PluginUnloadContext: PluginIdentifiersContextBase, IPluginUnloadContext
    {
        #region variable

        private readonly PluginStorage _storage;

        #endregion

        public PluginUnloadContext(IPluginIdentifiers pluginIdentifiers, PluginStorage storage)
            : base(pluginIdentifiers)
        {
            this._storage = storage;
        }

        #region IPluginUnloadContext

        /// <inheritdoc cref="IPluginUnloadContext.Storage"/>
        public PluginStorage Storage => GetValue(this._storage);
        IPluginStorage IPluginUnloadContext.Storage => Storage;

        #endregion
    }

    /// <inheritdoc cref="IPluginContext"/>
    public class PluginContext: PluginIdentifiersContextBase, IPluginContext
    {
        #region variable

        private readonly PluginStorage _storage;

        #endregion

        public PluginContext(IPluginIdentifiers pluginIdentifiers, PluginStorage storage)
            : base(pluginIdentifiers)
        {
            this._storage = storage;
        }

        #region IPluginContext

        /// <inheritdoc cref="IPluginContext.Storage"/>
        public PluginStorage Storage => GetValue(this._storage);
        IPluginStorage IPluginContext.Storage => Storage;

        #endregion
    }
}
