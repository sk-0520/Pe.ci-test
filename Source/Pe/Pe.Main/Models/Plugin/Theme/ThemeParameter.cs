using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Theme
{
    /// <inheritdoc cref="IThemeParameter"/>
    internal class ThemeParameter: PluginParameterBase, IThemeParameter
    {
        public ThemeParameter(IPluginInformation pluginInformations, IViewManager viewManager, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IPolicy policy, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(pluginInformations, viewManager, platformTheme, imageLoader, mediaConverter, policy, dispatcherWrapper, loggerFactory)
        { }

        #region IThemeParameter

        #endregion
    }
}
