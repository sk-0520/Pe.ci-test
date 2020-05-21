using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Theme
{
    /// <inheritdoc cref="IThemeParameter"/>
    public class ThemeParameter : PluginParameterBase, IThemeParameter
    {
        public ThemeParameter(IPlatformTheme platformTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(platformTheme, dispatcherWrapper, loggerFactory)
        { }

        #region IThemeParameter

        #endregion
    }
}
