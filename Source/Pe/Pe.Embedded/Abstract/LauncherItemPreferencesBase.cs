using System;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Embedded.Abstract
{
    /// <summary>
    /// ランチャーアイテム設定基底処理。
    /// </summary>
    internal abstract class LauncherItemPreferencesBase: ILauncherItemPreferences
    {
        protected LauncherItemPreferencesBase(PluginBase plugin, LauncherItemId launcherItemId, IAddonExecutor addonExecutor, IDispatcherWrapper dispatcherWrapper, ISkeletonImplements skeletonImplements, IImageLoader imageLoader, IHttpUserAgentFactory httpUserAgentFactory, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());

            Plugin = plugin;
            LauncherItemId = launcherItemId;
            AddonExecutor = addonExecutor;
            DispatcherWrapper = dispatcherWrapper;
            SkeletonImplements = skeletonImplements;
            ImageLoader = imageLoader;
            HttpUserAgentFactory = httpUserAgentFactory;
        }

        #region property

        /// <inheritdoc cref="ILoggerFactory"/>
        protected ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        protected ILogger Logger { get; }

        protected PluginBase Plugin { get; }
        protected LauncherItemId LauncherItemId { get; }
        protected IAddonExecutor AddonExecutor { get; }
        protected IDispatcherWrapper DispatcherWrapper { get; }
        protected ISkeletonImplements SkeletonImplements { get; }
        protected IImageLoader ImageLoader { get; }
        protected IHttpUserAgentFactory HttpUserAgentFactory { get; }
        #endregion

        #region ILauncherItemPreferences

        /// <inheritdoc cref="ILauncherItemPreferences.BeginPreferences(ILauncherItemPreferencesLoadContext)" />
        public abstract UserControl BeginPreferences(ILauncherItemPreferencesLoadContext preferencesLoadContext);

        /// <inheritdoc cref="ILauncherItemPreferences.CheckPreferences(ILauncherItemPreferencesCheckContext)" />
        public abstract void CheckPreferences(ILauncherItemPreferencesCheckContext preferencesCheckContext);

        /// <inheritdoc cref="ILauncherItemPreferences.SavePreferences(ILauncherItemPreferencesSaveContext)" />
        public abstract void SavePreferences(ILauncherItemPreferencesSaveContext preferencesSaveContext);

        /// <inheritdoc cref="ILauncherItemPreferences.EndPreferences(ILauncherItemPreferencesEndContext)" />
        public abstract void EndPreferences(ILauncherItemPreferencesEndContext preferencesEndContext);

        #endregion
    }
}
