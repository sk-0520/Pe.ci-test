using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Embedded.Abstract
{
    internal abstract class LauncherItemExtensionBase: ILauncherItemExtension, ILauncherItemId
    {
        protected LauncherItemExtensionBase(ILauncherItemExtensionCreateParameter parameter, IPluginInformations pluginInformations)
        {
            LoggerFactory = parameter.LoggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            LauncherItemId = parameter.LauncherItemId;
            AddonExecutor = parameter.AddonExecutor;
            PlatformTheme = parameter.PlatformTheme;
            DispatcherWrapper = parameter.DispatcherWrapper;
            SkeletonImplements = parameter.SkeletonImplements;
            ImageLoader = parameter.ImageLoader;
            MediaConverter = parameter.MediaConverter;
            HttpUserAgentFactory = parameter.HttpUserAgentFactory;
            ContextWorker = parameter.ContextWorker;
            PluginInformations = pluginInformations;
        }

        #region property

        protected ILoggerFactory LoggerFactory { get; }
        protected ILogger Logger { get; }
        protected IPlatformTheme PlatformTheme { get; }
        protected IAddonExecutor AddonExecutor { get; }
        protected IDispatcherWrapper DispatcherWrapper { get; }
        protected ISkeletonImplements SkeletonImplements { get; }
        protected IImageLoader ImageLoader { get; }
        protected IMediaConverter MediaConverter { get; }
        protected IHttpUserAgentFactory HttpUserAgentFactory { get; }
        protected ILauncherItemAddonContextWorker ContextWorker { get; }
        protected IPluginInformations PluginInformations { get; }

        protected ISet<object> CallerObjects { get; } = new HashSet<object>();

        #endregion

        #region function

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            OnPropertyChanged(propertyName);
        }

        #endregion

        #region ILauncherItemId

        public Guid LauncherItemId { get; }

        #endregion

        #region ILauncherItemExtension

        public event PropertyChangedEventHandler? PropertyChanged;

        public abstract bool CustomDisplayText { get; }
        public abstract string DisplayText { get; }
        public abstract bool CustomLauncherIcon { get; }
        public abstract bool SupportedPreferences { get; }

        public abstract void ChangeDisplay(LauncherItemIconMode iconMode, bool isVisible, object callerObject);
        public abstract object GetIcon(LauncherItemIconMode iconMode, in IconScale iconScale);
        public abstract void Execute(ICommandExecuteParameter commandExecuteParameter, ILauncherItemExtensionExecuteParameter launcherItemExtensionExecuteParameter, ILauncherItemAddonContext launcherItemAddonContext);
        public abstract ILauncherItemPreferences CreatePreferences(ILauncherItemAddonContext launcherItemAddonContext);

        #endregion
    }
}
