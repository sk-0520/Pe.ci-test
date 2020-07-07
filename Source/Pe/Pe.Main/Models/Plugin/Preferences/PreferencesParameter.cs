using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Preferences
{
    public class PreferencesParameter: PluginParameterBase, IPreferencesParameter
    {
        public PreferencesParameter(ISkeletonImplements skeletonImplements, IPluginInformations pluginInformations, IUserAgentFactory userAgentFactory, IPlatformTheme platformTheme, IImageLoader imageLoader, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(pluginInformations, platformTheme, imageLoader, dispatcherWrapper, loggerFactory)
        {
            SkeletonImplements = skeletonImplements;
            UserAgentFactory = userAgentFactory;
        }

        #region IPreferencesParameter

        public IUserAgentFactory UserAgentFactory { get; }
        public ISkeletonImplements SkeletonImplements { get; }
        #endregion
    }
}
