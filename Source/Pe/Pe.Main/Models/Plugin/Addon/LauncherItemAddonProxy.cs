using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    internal class LauncherItemAddonProxy: AddonProxyBase<ILauncherItemExtension>, ILauncherItemExtension
    {
        public LauncherItemAddonProxy(IAddon addon, PluginContextFactory pluginContextFactory, IHttpUserAgentFactory userAgentFactory, IPlatformTheme platformTheme, IImageLoader imageLoader, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(addon, pluginContextFactory, userAgentFactory, platformTheme, imageLoader, dispatcherWrapper, loggerFactory)
        { }

        #region function

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region AddonProxyBase

        protected override AddonKind AddonKind => throw new NotImplementedException();

        protected override ILauncherItemExtension BuildFunctionUnit(IAddon loadedAddon)
        {
            return loadedAddon.BuildLauncherItemExtension(CreateParameter(loadedAddon));
        }

        #endregion

        #region ILauncherItemExtension

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool CustomDisplayText => throw new NotImplementedException();

        public string DisplayText => throw new NotImplementedException();

        public bool CustomLauncherIcon => throw new NotImplementedException();

        public bool SupportedPreferences => throw new NotImplementedException();

        public ILauncherItemPreferences CreatePreferences()
        {
            throw new NotImplementedException();
        }

        public void Execute(ICommandExecuteParameter commandExecuteParameter, ILauncherItemExtensionExecuteParameter launcherItemExtensionExecuteParameter, ILauncherItemAddonContext launcherItemAddonContext)
        {
            throw new NotImplementedException();
        }

        public object GetIcon(LauncherItemIconMode iconMode, IconScale iconScale, ILauncherItemAddonContext launcherItemAddonContext)
        {
            throw new NotImplementedException();
        }

        public void Initialize(ILauncherItemId launcherItemId, ILauncherItemAddonContext launcherItemAddonContext)
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}
