using System;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    /// <summary>
    /// <see cref="IAddon.CreateLauncherItemExtension(ILauncherItemExtensionCreateParameter)"/> で渡されるデータ。
    /// </summary>
    internal class LauncherItemExtensionCreateParameter: AddonParameter, ILauncherItemExtensionCreateParameter
    {
        public LauncherItemExtensionCreateParameter(Guid launcherItemId, ILauncherItemAddonContextWorker contextWorker, ISkeletonImplements skeletonImplements, IPluginInformations pluginInformations, IHttpUserAgentFactory userAgentFactory, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IPolicy policy, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(skeletonImplements, pluginInformations, userAgentFactory, platformTheme, imageLoader, mediaConverter, policy, dispatcherWrapper, loggerFactory)
        {
            LauncherItemId = launcherItemId;
            ContextWorker = contextWorker;
        }

        #region ILauncherItemExtensionBuildParameter

        public Guid LauncherItemId { get; }

        public ILauncherItemAddonContextWorker ContextWorker { get; }

        #endregion
    }
}
