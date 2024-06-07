using System;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    /// <summary>
    /// <see cref="IAddon.CreateLauncherItemExtension(ILauncherItemExtensionCreateParameter)"/> で渡されるデータ。
    /// </summary>
    public class LauncherItemExtensionCreateParameter: AddonParameter, ILauncherItemExtensionCreateParameter
    {
        public LauncherItemExtensionCreateParameter(LauncherItemId launcherItemId, ILauncherItemAddonContextWorker contextWorker, ISkeletonImplements skeletonImplements, IPluginInformation pluginInformation, IHttpUserAgentFactory userAgentFactory, IViewManager viewManager, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IPolicy policy, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(skeletonImplements, pluginInformation, userAgentFactory, viewManager, platformTheme, imageLoader, mediaConverter, policy, dispatcherWrapper, loggerFactory)
        {
            LauncherItemId = launcherItemId;
            ContextWorker = contextWorker;
        }

        #region ILauncherItemExtensionBuildParameter

        public LauncherItemId LauncherItemId { get; }

        public ILauncherItemAddonContextWorker ContextWorker { get; }

        #endregion
    }
}
