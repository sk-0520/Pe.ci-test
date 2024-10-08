using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Manager.Setting;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Preferences;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using ContentTypeTextNet.Pe.Plugins.DefaultTheme;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class PluginsSettingEditorElement: SettingEditorElementBase
    {
        internal PluginsSettingEditorElement(PluginContainer pluginContainer, NewVersionChecker newVersionChecker, NewVersionDownloader newVersionDownloader, IPluginConstructorContext pluginConstructorContext, PauseReceiveLogDelegate pauseReceiveLog, PreferencesContextFactory preferencesContextFactory, IWindowManager windowManager, IUserTracker userTracker, ISettingNotifyManager settingNotifyManager, IClipboardManager clipboardManager, IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, ITemporaryDatabaseBarrier temporaryDatabaseBarrier, IDatabaseStatementLoader statementLoader, IIdFactory idFactory, EnvironmentParameters environmentParameters, GeneralConfiguration generalConfiguration, ApiConfiguration apiConfiguration, IHttpUserAgentFactory userAgentFactory, IViewManager viewManager, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IPolicy policy, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(settingNotifyManager, clipboardManager, mainDatabaseBarrier, largeDatabaseBarrier, temporaryDatabaseBarrier, statementLoader, idFactory, imageLoader, mediaConverter, policy, dispatcherWrapper, loggerFactory)
        {
            PluginContainer = pluginContainer;
            NewVersionChecker = newVersionChecker;
            NewVersionDownloader = newVersionDownloader;
            PluginConstructorContext = pluginConstructorContext;
            PreferencesContextFactory = preferencesContextFactory;
            PauseReceiveLog = pauseReceiveLog;

            UserTracker = userTracker;
            WindowManager = windowManager;

            UserAgentFactory = userAgentFactory;
            ViewManager = viewManager;
            PlatformTheme = platformTheme;
            EnvironmentParameters = environmentParameters;
            GeneralConfiguration = generalConfiguration;
            ApiConfiguration = apiConfiguration;

            PluginItems = new ReadOnlyObservableCollection<PluginSettingEditorElement>(PluginItemsImpl);
            InstallPluginItems = new ReadOnlyObservableCollection<PluginInstallItemElement>(InstallPluginItemsImpl);

            PluginInstaller = new PluginInstaller(PluginContainer, PluginConstructorContext, PauseReceiveLog, EnvironmentParameters, DatabaseStatementLoader, LoggerFactory);
        }

        #region property

        private PreferencesContextFactory PreferencesContextFactory { get; }
        private NewVersionChecker NewVersionChecker { get; }
        private NewVersionDownloader NewVersionDownloader { get; }
        private IHttpUserAgentFactory UserAgentFactory { get; }

        private IViewManager ViewManager { get; }
        private IPlatformTheme PlatformTheme { get; }
        private EnvironmentParameters EnvironmentParameters { get; }
        private GeneralConfiguration GeneralConfiguration { get; }
        private ApiConfiguration ApiConfiguration { get; }

        private PluginContainer PluginContainer { get; }
        private IPluginConstructorContext PluginConstructorContext { get; }
        private PauseReceiveLogDelegate PauseReceiveLog { get; }
        private IUserTracker UserTracker { get; }
        private IWindowManager WindowManager { get; }

        private ObservableCollection<PluginSettingEditorElement> PluginItemsImpl { get; } = new ObservableCollection<PluginSettingEditorElement>();
        public ReadOnlyObservableCollection<PluginSettingEditorElement> PluginItems { get; }

        private ObservableCollection<PluginInstallItemElement> InstallPluginItemsImpl { get; } = new ObservableCollection<PluginInstallItemElement>();
        public ReadOnlyObservableCollection<PluginInstallItemElement> InstallPluginItems { get; }

        private PluginInstaller PluginInstaller { get; }

        #endregion

        #region function

        public void CancelInstall(PluginId pluginId)
        {
            var cancelled = PluginInstaller.CancelInstall(pluginId, InstallPluginItemsImpl.Select(i => i.Data), TemporaryDatabaseBarrier);
            if(cancelled) {
                var removeTarget = InstallPluginItemsImpl.First(i => i.Data.PluginId == pluginId);
                InstallPluginItemsImpl.Remove(removeTarget);
            }
        }

        private void MergeInstallPlugin(PluginInstallItemElement element)
        {
            var oldElement = InstallPluginItemsImpl.FirstOrDefault(i => i.Data.PluginId == element.Data.PluginId);
            if(oldElement != null) {
                InstallPluginItemsImpl.Remove(oldElement);
            }
            InstallPluginItemsImpl.Add(element);

        }

        internal async Task InstallPluginArchiveAsync(FileInfo archiveFile, CancellationToken cancellationToken)
        {
            var ext = PluginInstaller.GetArchiveExtension(archiveFile);
            var pluginFileName = PluginInstaller.GetPluginName(archiveFile);

            var pluginInstallData = await PluginInstaller.InstallPluginArchiveAsync(archiveFile, ext, true, InstallPluginItemsImpl.Select(i => i.Data), PluginInstallAssemblyMode.Process, TemporaryDatabaseBarrier);
            var element = new PluginInstallItemElement(pluginInstallData, LoggerFactory);
            await element.InitializeAsync(cancellationToken);
            MergeInstallPlugin(element);
        }

        internal async Task<PluginWebInstallRequestParameter> CreatePluginWebInstallRequestParameterAsync(CancellationToken cancellationToken)
        {
            var element = new Plugin.PluginWebInstallElement(PluginContainer, EnvironmentParameters, ApiConfiguration, NewVersionChecker, NewVersionDownloader, UserAgentFactory, LoggerFactory);
            await element.InitializeAsync(cancellationToken);

            return new PluginWebInstallRequestParameter() {
                Element = element,
                WindowManager = WindowManager,
                UserTracker = UserTracker,
                DispatcherWrapper = DispatcherWrapper,
                LoggerFactory = LoggerFactory,
            };
        }

        #endregion

        #region SettingEditorElementBase

        protected override async Task LoadCoreAsync(CancellationToken cancellationToken)
        {
            IList<PluginStateData> pluginStates;
            using(var context = MainDatabaseBarrier.WaitRead()) {
                var pluginsEntityDao = new PluginsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                pluginStates = pluginsEntityDao.SelectPluginStateData().ToList();
            }

            IList<PluginInstallData> installDataItems;
            using(var context = TemporaryDatabaseBarrier.WaitRead()) {
                var installPluginsEntityDao = new InstallPluginsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                installDataItems = installPluginsEntityDao.SelectInstallPlugins().ToList();
            }

            foreach(var installDataItem in installDataItems) {
                var element = new PluginInstallItemElement(installDataItem, LoggerFactory);
                await element.InitializeAsync(cancellationToken);
                InstallPluginItemsImpl.Add(element);
            }

            // 標準テーマがなければ追加
            if(!pluginStates.Any(i => i.PluginId == DefaultTheme.Information.PluginIdentifiers.PluginId)) {
                pluginStates.Insert(0, new Data.PluginStateData() {
                    PluginName = DefaultTheme.Information.PluginIdentifiers.PluginName,
                    PluginId = DefaultTheme.Information.PluginIdentifiers.PluginId,
                    State = Data.PluginState.Enable,
                });
            }

            foreach(var pluginState in pluginStates) {
                var plugin = PluginContainer.Plugins.FirstOrDefault(i => pluginState.PluginId == i.PluginInformation.PluginIdentifiers.PluginId);
                var element = new PluginSettingEditorElement(pluginState, plugin, PreferencesContextFactory, MainDatabaseBarrier, DatabaseStatementLoader, UserAgentFactory, ViewManager, PlatformTheme, ImageLoader, MediaConverter, Policy, DispatcherWrapper, LoggerFactory);
                await element.InitializeAsync(cancellationToken);
                PluginItemsImpl.Add(element);
            }
        }

        protected override void SaveImpl(IDatabaseContextsPack contextsPack)
        {
            foreach(var element in PluginItems) {
                if(element.SupportedPreferences && element.StartedPreferences) {
                    element.SavePreferences(contextsPack);
                }

                element.Save(contextsPack);
            }
        }

        #endregion
    }
}
