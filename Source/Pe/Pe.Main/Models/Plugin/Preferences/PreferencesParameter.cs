using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Preferences
{
    public class PreferencesParameter: PluginParameterBase, IPreferencesParameter
    {
        public PreferencesParameter(ISkeletonImplements skeletonImplements, IPluginInformations pluginInformations, IHttpUserAgentFactory userAgentFactory, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IPolicy policy, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(pluginInformations, platformTheme, imageLoader, mediaConverter, policy, dispatcherWrapper, loggerFactory)
        {
            SkeletonImplements = skeletonImplements;
            HttpUserAgentFactory = userAgentFactory;
        }

        #region IPreferencesParameter

        public IHttpUserAgentFactory HttpUserAgentFactory { get; }
        public ISkeletonImplements SkeletonImplements { get; }
        #endregion
    }
}
