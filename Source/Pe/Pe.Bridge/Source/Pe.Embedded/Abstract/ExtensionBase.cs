using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Embedded.Abstract
{
    internal class ExtensionBase: IPlugin
    {
        protected ExtensionBase(IPluginConstructorContext pluginConstructorContext, IPlugin plugin)
        {
            Logger = pluginConstructorContext.LoggerFactory.CreateLogger(GetType());
            Plugin = plugin;
        }

        #region property

        protected ILogger Logger { get; }
        protected IPlugin Plugin { get; }

        #endregion

        #region IPlugin

        public IPluginInformations PluginInformations => Plugin.PluginInformations;

        public bool IsInitialized => Plugin.IsInitialized;

        public DependencyObject GetIcon(IconBox iconBox) => throw new NotSupportedException();

        public void Initialize(IPluginInitializeContext pluginInitializeContext) => throw new NotSupportedException();
        public void Uninitialize(IPluginUninitializeContext pluginUninitializeContext) => throw new NotSupportedException();

        public bool IsLoaded(PluginKind pluginKind) => throw new NotSupportedException();

        public void Load(PluginKind pluginKind, IPluginLoadContext pluginLoadContext) => throw new NotSupportedException();
        public void Unload(PluginKind pluginKind, IPluginUnloadContext pluginUnloadContext) => throw new NotSupportedException();

        #endregion

    }
}
