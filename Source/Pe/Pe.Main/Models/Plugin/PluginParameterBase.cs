using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    /// <inheritdoc cref="IPluginParameter"/>
    public abstract class PluginParameterBase: IPluginParameter
    {
        protected PluginParameterBase(IPlatformTheme platformTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        {
            PlatformTheme = platformTheme;
            DispatcherWrapper = dispatcherWrapper;
            LoggerFactory = loggerFactory;
        }

        #region PluginParameter

        /// <inheritdoc cref="IPluginParameter.PlatformTheme"/>
        public IPlatformTheme PlatformTheme { get; }
        /// <inheritdoc cref="IPluginParameter.DispatcherWrapper"/>
        public IDispatcherWrapper DispatcherWrapper { get; }
        /// <inheritdoc cref="IPluginParameter.LoggerFactory"/>
        public ILoggerFactory LoggerFactory { get; }

        #endregion
    }
}
