using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Preferences;
using ContentTypeTextNet.Pe.Plugins.DefaultTheme;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class PluginSettingEditorElement: ElementBase, IPluginId
    {
        internal PluginSettingEditorElement(PluginStateData pluginState, IPlugin? plugin, PreferencesContextFactory preferencesContextFactory, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, IHttpUserAgentFactory userAgentFactory, IViewManager viewManager, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IPolicy policy, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            PluginState = pluginState;
            Plugin = plugin;
            PreferencesContextFactory = preferencesContextFactory;
            MainDatabaseBarrier = mainDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
            UserAgentFactory = userAgentFactory;

            ViewManager = viewManager;
            PlatformTheme = platformTheme;
            ImageLoader = imageLoader;
            MediaConverter = mediaConverter;
            Policy = policy;
            DispatcherWrapper = dispatcherWrapper;

            if(Plugin is IPreferences preferences) {
                SupportedPreferences = true;
                Preferences = preferences;
            } else {
                SupportedPreferences = false;
            }

            var appPluginIds = new[] {
                DefaultTheme.Information.PluginIdentifiers.PluginId
            };
            CanUninstall = !appPluginIds.Any(i => i == Plugin?.PluginInformation.PluginIdentifiers.PluginId);

            if(CanUninstall) {
                using(var context = MainDatabaseBarrier.WaitRead()) {
                    var pluginsEntityDao = new PluginsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                    var data = pluginsEntityDao.SelectPluginStateDataByPluginId(PluginId);
                    if(data != null) {
                        MarkedUninstall = data.State == Data.PluginState.Uninstall;
                    }
                }
            }
        }

        #region property

        public PluginStateData PluginState { get; }
        public IPlugin? Plugin { get; }
        private PreferencesContextFactory PreferencesContextFactory { get; }
        private IMainDatabaseBarrier MainDatabaseBarrier { get; }
        private IDatabaseStatementLoader DatabaseStatementLoader { get; }
        private IHttpUserAgentFactory UserAgentFactory { get; }

        private IViewManager ViewManager { get; }
        private IPlatformTheme PlatformTheme { get; }
        private IImageLoader ImageLoader { get; }
        private IMediaConverter MediaConverter { get; }
        private IPolicy Policy { get; }
        private IDispatcherWrapper DispatcherWrapper { get; }

        public bool SupportedPreferences { get; }
        private IPreferences? Preferences { get; }

        public bool StartedPreferences { get; private set; }

        public Version PluginVersion { get; private set; } = new Version();

        /// <summary>
        /// アンイントール対象とするか。
        /// </summary>
        public bool MarkedUninstall { get; private set; }

        /// <summary>
        /// そもそもアンインストール可能か。
        /// </summary>
        /// <remarks>
        /// <para>Pe 提供プラグインが偽になる。</para>
        /// </remarks>
        public bool CanUninstall { get; private set; }

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
                using var context = PreferencesContextFactory.CreatePreferencesLoadContext(Plugin.PluginInformation, reader);
                var skeleton = new SkeletonImplements();
                var parameter = new PreferencesParameter(skeleton, Plugin.PluginInformation, UserAgentFactory, ViewManager, PlatformTheme, ImageLoader, MediaConverter, Policy, DispatcherWrapper, LoggerFactory);
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
                using var context = PreferencesContextFactory.CreateCheckContext(Plugin.PluginInformation, reader);
                Preferences.CheckPreferences(context);
                hasError = context.HasError;
            }
            return hasError;
        }

        /// <summary>
        /// プラグイン側の保存処理。
        /// </summary>
        /// <param name="databaseContextsPack"></param>
        public void SavePreferences(IDatabaseContextsPack databaseContextsPack)
        {
            if(!SupportedPreferences) {
                throw new InvalidOperationException(nameof(SupportedPreferences));
            }
            Debug.Assert(Preferences != null);
            Debug.Assert(StartedPreferences);
            Debug.Assert(Plugin != null);

            using var context = PreferencesContextFactory.CreateSaveContext(Plugin.PluginInformation, databaseContextsPack);
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
                using var context = PreferencesContextFactory.CreateEndContext(Plugin.PluginInformation, reader);
                Preferences.EndPreferences(context);
            }
        }

        public void ToggleUninstallMark()
        {
            MarkedUninstall = !MarkedUninstall;
        }

        /// <summary>
        /// プラグイン設定ではなくプラグイン状態に対する保存処理。
        /// </summary>
        /// <remarks>
        /// <para>アンインストールとかね。将来的には非活性もここでやる。</para>
        /// </remarks>
        /// <param name="contextsPack"></param>
        public void Save(IDatabaseContextsPack contextsPack)
        {
            var pluginsEntityDao = new PluginsEntityDao(contextsPack.Main.Context, DatabaseStatementLoader, contextsPack.Main.Implementation, LoggerFactory);

            if(CanUninstall && MarkedUninstall) {
                var pluginState = new PluginStateData() {
                    PluginId = PluginState.PluginId,
                    PluginName = PluginState.PluginName,
                    State = ContentTypeTextNet.Pe.Main.Models.Data.PluginState.Uninstall,
                };
                pluginsEntityDao.UpdatePluginStateData(pluginState, contextsPack.CommonStatus);
            } else if(!MarkedUninstall) {
                var pluginState = new PluginStateData() {
                    PluginId = PluginState.PluginId,
                    PluginName = PluginState.PluginName,
                    State = ContentTypeTextNet.Pe.Main.Models.Data.PluginState.Enable, // TODO: 無効化処理を入れた際には変更が必要
                };
                pluginsEntityDao.UpdatePluginStateData(pluginState, contextsPack.CommonStatus);
            }
        }

        #endregion

        #region ElementBase

        protected override Task InitializeCoreAsync(CancellationToken cancellationToken)
        {
            if(Plugin != null) {
                PluginVersion = Plugin.PluginInformation.PluginVersions.PluginVersion;
            } else {
                var pluginVersion = MainDatabaseBarrier.ReadData(c => {
                    var pluginsEntityDao = new PluginsEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                    return pluginsEntityDao.SelectLastUsePluginVersion(PluginId);
                });
                if(pluginVersion != null) {
                    PluginVersion = pluginVersion;
                }
            }

            return Task.CompletedTask;
        }

        #endregion

        #region IPLuginId

        public PluginId PluginId => PluginState.PluginId;

        #endregion
    }
}
