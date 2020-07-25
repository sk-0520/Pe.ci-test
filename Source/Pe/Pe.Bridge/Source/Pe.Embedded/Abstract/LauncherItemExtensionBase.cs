using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Embedded.Abstract
{
    internal abstract class LauncherItemExtensionBase: ILauncherItemExtension
    {
        protected LauncherItemExtensionBase(IAddonParameter parameter, IPluginInformations pluginInformations)
        {
            LoggerFactory = parameter.LoggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            AddonExecutor = parameter.AddonExecutor;
            DispatcherWrapper = parameter.DispatcherWrapper;
            SkeletonImplements = parameter.SkeletonImplements;
            PluginInformations = pluginInformations;

        }

        #region property

        protected ILoggerFactory LoggerFactory { get; }
        protected ILogger Logger { get; }
        protected IAddonExecutor AddonExecutor { get; }
        protected IDispatcherWrapper DispatcherWrapper { get; }
        protected ISkeletonImplements SkeletonImplements { get; }
        protected IPluginInformations PluginInformations { get; }

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

        #region ILauncherItemExtension

        public event PropertyChangedEventHandler? PropertyChanged;

        public abstract bool CustomDisplayText { get; }
        public abstract string DisplayText { get; }
        public abstract bool CustomLauncherIcon { get; }

        public abstract void Initialize(ILauncherItemId launcherItemId, ILauncherItemAddonContext launcherItemAddonContext);
        public abstract object GetIcon(LauncherItemIconMode iconMode, IconScale iconScale, ILauncherItemAddonContext launcherItemAddonContext);
        public abstract void Execute(ICommandExecuteParameter commandExecuteParameter, ILauncherItemExtensionExecuteParameter launcherItemExtensionExecuteParameter, ILauncherItemAddonContext launcherItemAddonContext);

        #endregion
    }
}
