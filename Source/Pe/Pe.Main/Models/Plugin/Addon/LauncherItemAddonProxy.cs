using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    internal class LauncherItemAddonProxy: AddonProxyBase<ILauncherItemExtension>, ILauncherItemExtension, ILauncherItemId
    {
        public LauncherItemAddonProxy(Guid launcherItemId, IAddon addon, PluginContextFactory pluginContextFactory, LauncherItemAddonContextFactory launcherItemAddonContextFactory, IHttpUserAgentFactory userAgentFactory, IPlatformTheme platformTheme, IImageLoader imageLoader, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(addon, pluginContextFactory, userAgentFactory, platformTheme, imageLoader, dispatcherWrapper, loggerFactory)
        {
            LauncherItemId = launcherItemId;
            LauncherItemAddonContextFactory = launcherItemAddonContextFactory;
        }

        #region proeprty

        LauncherItemAddonContextFactory LauncherItemAddonContextFactory { get; }

        #endregion

        #region function

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region AddonProxyBase

        protected override AddonKind AddonKind => throw new NotImplementedException();

        protected override AddonParameter CreateParameter(IPlugin plugin)
        {
            var worker = LauncherItemAddonContextFactory.CreateWorker(LauncherItemId);
            return new LauncherItemExtensionCreateParameter(LauncherItemId, worker, new SkeletonImplements(), plugin.PluginInformations, UserAgentFactory, PlatformTheme, ImageLoader, DispatcherWrapper, LoggerFactory);
        }

        protected override ILauncherItemExtension BuildFunctionUnit(IAddon loadedAddon)
        {
            var parameter = (ILauncherItemExtensionCreateParameter)CreateParameter(loadedAddon);
            return loadedAddon.CreateLauncherItemExtension(parameter);
        }

        #endregion

        #region ILauncherItemExtension

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool CustomDisplayText => throw new NotImplementedException();

        public string DisplayText => throw new NotImplementedException();

        public bool CustomLauncherIcon => throw new NotImplementedException();

        public bool SupportedPreferences => throw new NotImplementedException();

        public ILauncherItemPreferences CreatePreferences(ILauncherItemAddonContext launcherItemAddonContext)
        {
            throw new NotImplementedException();
        }

        public void Execute(ICommandExecuteParameter commandExecuteParameter, ILauncherItemExtensionExecuteParameter launcherItemExtensionExecuteParameter, ILauncherItemAddonContext launcherItemAddonContext)
        {
            throw new NotImplementedException();
        }

        public object GetIcon(LauncherItemIconMode iconMode, in IconScale iconScale)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ILauncherItemId

        public Guid LauncherItemId { get; }
        #endregion
    }
}
