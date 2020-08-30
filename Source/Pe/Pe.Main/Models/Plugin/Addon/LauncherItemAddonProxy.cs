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
        public LauncherItemAddonProxy(Guid launcherItemId, IAddon addon, PluginContextFactory pluginContextFactory, LauncherItemAddonContextFactory launcherItemAddonContextFactory, IHttpUserAgentFactory userAgentFactory, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(addon, pluginContextFactory, userAgentFactory, platformTheme, imageLoader, mediaConverter, dispatcherWrapper, loggerFactory)
        {
            LauncherItemId = launcherItemId;
            LauncherItemAddonContextFactory = launcherItemAddonContextFactory;
        }

        #region proeprty

        LauncherItemAddonContextFactory LauncherItemAddonContextFactory { get; }

        #endregion

        #region function

        //void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        #endregion

        #region AddonProxyBase

        protected override AddonKind AddonKind => AddonKind.LauncherItem;

        protected override AddonParameter CreateParameter(IPlugin plugin)
        {
            var worker = LauncherItemAddonContextFactory.CreateWorker(plugin.PluginInformations, LauncherItemId);
            return new LauncherItemExtensionCreateParameter(LauncherItemId, worker, new SkeletonImplements(), plugin.PluginInformations, UserAgentFactory, PlatformTheme, ImageLoader, MediaConverter, DispatcherWrapper, LoggerFactory);
        }

        protected override ILauncherItemExtension BuildFunctionUnit(IAddon loadedAddon)
        {
            var parameter = (ILauncherItemExtensionCreateParameter)CreateParameter(loadedAddon);
            return loadedAddon.CreateLauncherItemExtension(parameter);
        }

        #endregion

        #region ILauncherItemExtension

        public event PropertyChangedEventHandler? PropertyChanged
        {
            add => FunctionUnit.PropertyChanged += value;
            remove => FunctionUnit.PropertyChanged -= value;
        }

        public bool CustomDisplayText => FunctionUnit.CustomDisplayText;

        public string DisplayText => FunctionUnit.DisplayText;

        public bool CustomLauncherIcon => FunctionUnit.CustomLauncherIcon;

        public bool SupportedPreferences => FunctionUnit.SupportedPreferences;

        public void ChangeDisplay(LauncherItemIconMode iconMode, bool isVisible, object callerObject)
        {
            FunctionUnit.ChangeDisplay(iconMode, isVisible, callerObject);
        }

        public ILauncherItemPreferences CreatePreferences(ILauncherItemAddonContext launcherItemAddonContext)
        {
            return FunctionUnit.CreatePreferences(launcherItemAddonContext);
        }

        public void Execute(ICommandExecuteParameter commandExecuteParameter, ILauncherItemExtensionExecuteParameter launcherItemExtensionExecuteParameter, ILauncherItemAddonContext launcherItemAddonContext)
        {
            FunctionUnit.Execute(commandExecuteParameter, launcherItemExtensionExecuteParameter, launcherItemAddonContext);
        }

        public object GetIcon(LauncherItemIconMode iconMode, in IconScale iconScale)
        {
            return FunctionUnit.GetIcon(iconMode, iconScale);
        }

        #endregion

        #region ILauncherItemId

        public Guid LauncherItemId { get; }

        #endregion

        //private void FunctionUnit_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    OnPropertyChanged(e.PropertyName);
        //}
    }
}
