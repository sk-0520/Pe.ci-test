using System;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    internal class LauncherItemExtensionExecuteParameter: AddonParameter, ILauncherItemExtensionExecuteParameter
    {
        public LauncherItemExtensionExecuteParameter(Guid launcherItemId, ILauncherItemAddonViewSupporter viewSupporter, ISkeletonImplements skeletonImplements, IPluginInformations pluginInformations, IHttpUserAgentFactory userAgentFactory, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(skeletonImplements, pluginInformations, userAgentFactory, platformTheme, imageLoader, mediaConverter, dispatcherWrapper, loggerFactory)
        {
            LauncherItemId = launcherItemId;
            ViewSupporter = viewSupporter;
        }
        #region ILauncherItemExtensionExecuteParameter

        public ILauncherItemAddonViewSupporter ViewSupporter { get; }

        public Guid LauncherItemId { get; }

        #endregion
    }
}
