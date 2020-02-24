using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    public abstract class PluginParameterBase: IPluginParameter
    {
        protected PluginParameterBase(IPlatformTheme platformTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        {
            PlatformTheme = platformTheme;
            DispatcherWrapper = dispatcherWrapper;
            LoggerFactory = loggerFactory;
        }

        #region PluginParameter

        public IPlatformTheme PlatformTheme { get; }
        public IDispatcherWrapper DispatcherWrapper { get; }
        public ILoggerFactory LoggerFactory { get; }

        #endregion
    }
}
