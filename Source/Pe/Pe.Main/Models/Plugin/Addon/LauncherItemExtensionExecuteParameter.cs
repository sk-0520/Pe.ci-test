using System;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    internal class LauncherItemExtensionExecuteParameter: AddonParameter, ILauncherItemExtensionExecuteParameter
    {
        public LauncherItemExtensionExecuteParameter(LauncherItemId launcherItemId, ILauncherItemAddonViewSupporter viewSupporter, ISkeletonImplements skeletonImplements, IPluginInformations pluginInformations, IHttpUserAgentFactory userAgentFactory, IViewManager viewManager, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IPolicy policy , IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(skeletonImplements, pluginInformations, userAgentFactory, viewManager, platformTheme, imageLoader, mediaConverter, policy, dispatcherWrapper, loggerFactory)
        {
            LauncherItemId = launcherItemId;
            ViewSupporter = viewSupporter;
        }

        #region ILauncherItemExtensionExecuteParameter

        public ILauncherItemAddonViewSupporter ViewSupporter { get; }

        public LauncherItemId LauncherItemId { get; }

        #endregion
    }
}
