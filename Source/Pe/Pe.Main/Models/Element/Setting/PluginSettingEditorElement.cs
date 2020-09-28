using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Preferences;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class PluginSettingEditorElement: ElementBase, IPluginId
    {
        internal PluginSettingEditorElement(PluginStateData pluginState, IPlugin? plugin, PreferencesContextFactory preferencesContextFactory, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, IHttpUserAgentFactory userAgentFactory, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            PluginState = pluginState;
            Plugin = plugin;
            PreferencesContextFactory = preferencesContextFactory;
            MainDatabaseBarrier = mainDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
            UserAgentFactory = userAgentFactory;
            PlatformTheme = platformTheme;
            ImageLoader = imageLoader;
            MediaConverter = mediaConverter;
            DispatcherWrapper = dispatcherWrapper;

            if(Plugin is IPreferences preferences) {
                SupportedPreferences = true;
                Preferences = preferences;
            } else {
                SupportedPreferences = false;
            }
        }

        #region property

        public PluginStateData PluginState { get; }
        public IPlugin? Plugin { get; }
        PreferencesContextFactory PreferencesContextFactory { get; }
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IDatabaseStatementLoader DatabaseStatementLoader { get; }
        IHttpUserAgentFactory UserAgentFactory { get; }
        IPlatformTheme PlatformTheme { get; }
        IImageLoader ImageLoader { get; }
        IMediaConverter MediaConverter { get; }
        IDispatcherWrapper DispatcherWrapper { get; }

        public bool SupportedPreferences { get; }
        IPreferences? Preferences { get; }

        public bool StartedPreferences { get; private set; }

        public Version PluginVersion { get; private set; } = new Version();

        #endregion

        #region function

        public UserControl BeginPreferences()
        {
            if(!SupportedPreferences) {
                throw new InvalidOperationException(nameof(SupportedPreferences));
            }
            Debug.Assert(Preferences != null);
            Debug.Assert(!StartedPreferences);
            Debug.Assert(Plugin != null);

            UserControl result;
            using(var reader = PreferencesContextFactory.BarrierRead()) {
                using var context = PreferencesContextFactory.CreateLoadContext(Plugin.PluginInformations, reader);
                var skeleton = new SkeletonImplements();
                var parameter = new PreferencesParameter(skeleton, Plugin.PluginInformations, UserAgentFactory, PlatformTheme, ImageLoader, MediaConverter, DispatcherWrapper, LoggerFactory);
                result = Preferences.BeginPreferences(context, parameter);
            }
            StartedPreferences = true;
            return result;
        }

        public bool CheckPreferences()
        {
            if(!SupportedPreferences) {
                throw new InvalidOperationException(nameof(SupportedPreferences));
            }
            Debug.Assert(Preferences != null);
            Debug.Assert(StartedPreferences);
            Debug.Assert(Plugin != null);

            bool hasError;
            using(var reader = PreferencesContextFactory.BarrierRead()) {
                using var context = PreferencesContextFactory.CreateCheckContext(Plugin.PluginInformations, reader);
                Preferences.CheckPreferences(context);
                hasError = context.HasError;
            }
            return hasError;
        }

        public void SavePreferences(IDatabaseCommandsPack databaseCommandPack)
        {
            if(!SupportedPreferences) {
                throw new InvalidOperationException(nameof(SupportedPreferences));
            }
            Debug.Assert(Preferences != null);
            Debug.Assert(StartedPreferences);
            Debug.Assert(Plugin != null);

            using var context = PreferencesContextFactory.CreateSaveContext(Plugin.PluginInformations, databaseCommandPack);
            Preferences.SavePreferences(context);
        }

        public void EndPreferences()
        {
            if(!SupportedPreferences) {
                throw new InvalidOperationException(nameof(SupportedPreferences));
            }
            Debug.Assert(Preferences != null);
            Debug.Assert(StartedPreferences);
            Debug.Assert(Plugin != null);

            // NOTE: 多分ここじゃなくて別んところで呼び出すべき

            using(var reader = PreferencesContextFactory.BarrierRead()) {
                using var context = PreferencesContextFactory.CreateEndContext(Plugin.PluginInformations, reader);
                Preferences.EndPreferences(context);
            }
        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            if(Plugin != null) {
                PluginVersion = Plugin.PluginInformations.PluginVersions.PluginVersion;
            } else {
                var pluginVersion = MainDatabaseBarrier.ReadData(c => {
                    var pluginsEntityDao = new PluginsEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                    return pluginsEntityDao.SelectLastUsePluginVersion(PluginId);
                });
                if(pluginVersion != null) {
                    PluginVersion = pluginVersion;
                }
            }
        }

        #endregion

        #region IPLuginId

        public Guid PluginId => PluginState.PluginId;

        #endregion
    }
}
