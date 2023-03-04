using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    /// <inheritdoc cref="IAddonParameter"/>
    internal class AddonParameter: PluginParameterBase, IAddonParameter
    {
        public AddonParameter(ISkeletonImplements skeletonImplements, IPluginInformations pluginInformations, IHttpUserAgentFactory userAgentFactory, IViewManager viewManager, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IPolicy policy, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(pluginInformations, viewManager, platformTheme, imageLoader, mediaConverter, policy, dispatcherWrapper, loggerFactory)
        {
            HttpUserAgentFactory = userAgentFactory;
            AddonExecutor = new AddonExecutor(PluginInformations, LoggerFactory);
            SkeletonImplements = skeletonImplements;
        }

        #region IAddonParameter

        /// <inheritdoc cref="IAddonParameter.HttpUserAgentFactory"/>
        public IHttpUserAgentFactory HttpUserAgentFactory { get; }
        /// <inheritdoc cref="IAddonParameter.AddonExecutor"/>
        public IAddonExecutor AddonExecutor { get; }
        /// <inheritdoc cref="IAddonParameter.SkeletonImplements"/>
        public ISkeletonImplements SkeletonImplements { get; }
        #endregion
    }
}
