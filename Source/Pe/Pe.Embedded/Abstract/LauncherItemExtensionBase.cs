using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
        protected LauncherItemExtensionBase(ILauncherItemExtensionCreateParameter parameter, IPluginInformation pluginInformations)
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

        /// <inheritdoc cref="ILoggerFactory"/>
        protected ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        protected ILogger Logger { get; }
        /// <inheritdoc cref="IPlatformTheme"/>
        protected IPlatformTheme PlatformTheme { get; }
        /// <inheritdoc cref="IAddonExecutor"/>
        protected IAddonExecutor AddonExecutor { get; }
        /// <inheritdoc cref="IDispatcherWrapper"/>
        protected IDispatcherWrapper DispatcherWrapper { get; }
        /// <inheritdoc cref="ISkeletonImplements"/>
        protected ISkeletonImplements SkeletonImplements { get; }
        /// <inheritdoc cref="ILogger"/>
        protected IImageLoader ImageLoader { get; }
        /// <inheritdoc cref="IMediaConverter"/>
        protected IMediaConverter MediaConverter { get; }
        /// <inheritdoc cref="IHttpUserAgentFactory"/>
        protected IHttpUserAgentFactory HttpUserAgentFactory { get; }
        /// <inheritdoc cref="ILauncherItemAddonContextWorker"/>
        protected ILauncherItemAddonContextWorker ContextWorker { get; }
        /// <inheritdoc cref="IPluginInformation"/>
        protected IPluginInformation PluginInformations { get; }

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

        public LauncherItemId LauncherItemId { get; }

        #endregion

        #region ILauncherItemExtension

        public event PropertyChangedEventHandler? PropertyChanged;

        public abstract bool CustomDisplayText { get; }
        public abstract string DisplayText { get; }
        public abstract bool CustomLauncherIcon { get; }
        public abstract bool SupportedPreferences { get; }

        public abstract void ChangeDisplay(LauncherItemIconMode iconMode, bool isVisible, object callerObject);
        public abstract object GetIcon(LauncherItemIconMode iconMode, in IconScale iconScale);
        public abstract void Execute(string? argument, ICommandExecuteParameter commandExecuteParameter, ILauncherItemExtensionExecuteParameter launcherItemExtensionExecuteParameter, ILauncherItemAddonContext launcherItemAddonContext);
        public abstract ILauncherItemPreferences CreatePreferences(ILauncherItemAddonContext launcherItemAddonContext);

        #endregion
    }
}
