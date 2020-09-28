using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Theme
{
    /// <inheritdoc cref="IThemeParameter"/>
    internal class ThemeParameter: PluginParameterBase, IThemeParameter
    {
        public ThemeParameter(IPluginInformations pluginInformations, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(pluginInformations, platformTheme, imageLoader, mediaConverter, dispatcherWrapper, loggerFactory)
        { }

        #region IThemeParameter

        #endregion
    }
}
