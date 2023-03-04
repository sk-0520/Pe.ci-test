using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    /// <inheritdoc cref="IPluginParameter"/>
    public abstract class PluginParameterBase: IPluginParameter
    {
        protected PluginParameterBase(IPluginInformations pluginInformations, IViewManager viewManager, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IPolicy policy, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        {
            PluginInformations = pluginInformations;
            ViewManager = viewManager;
            PlatformTheme = platformTheme;
            MediaConverter = mediaConverter;
            ImageLoader = imageLoader;
            Policy = policy;
            DispatcherWrapper = dispatcherWrapper;
            LoggerFactory = loggerFactory;
        }

        #region property

        protected IPluginInformations PluginInformations { get; }

        #endregion

        #region PluginParameter

        /// <inheritdoc cref="IPluginParameter.ViewManager"/>
        public IViewManager ViewManager { get; }
        /// <inheritdoc cref="IPluginParameter.PlatformTheme"/>
        public IPlatformTheme PlatformTheme { get; }
        /// <inheritdoc cref="IPluginParameter.ImageLoader"/>
        public IImageLoader ImageLoader { get; }
        /// <inheritdoc cref="IPluginParameter.MediaConverter"/>
        public IMediaConverter MediaConverter { get; }
        /// <inheritdoc cref="IPluginParameter.Policy"/>
        public IPolicy Policy { get; }
        /// <inheritdoc cref="IPluginParameter.DispatcherWrapper"/>
        public IDispatcherWrapper DispatcherWrapper { get; }
        /// <inheritdoc cref="IPluginParameter.LoggerFactory"/>
        public ILoggerFactory LoggerFactory { get; }

        #endregion
    }
}
