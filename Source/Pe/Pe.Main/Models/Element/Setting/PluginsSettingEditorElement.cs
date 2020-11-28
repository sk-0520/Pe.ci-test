using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Manager.Setting;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Preferences;
using ContentTypeTextNet.Pe.Plugins.DefaultTheme;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public class PluginsSettingEditorElement: SettingEditorElementBase
    {
        #region define

        class StringProgress: IProgress<string>
        {
            public StringProgress(ILoggerFactory loggerFactory)
            {
                Logger = loggerFactory.CreateLogger(GetType());
            }

            #region proeprty

            ILogger Logger { get; }

            #endregion

            #region IProgress

            public void Report(string value)
            {
                Logger.LogInformation(value);
            }

            #endregion
        }

        class DoubleProgress: IProgress<double>
        {
            public DoubleProgress(ILoggerFactory loggerFactory)
            {
                Logger = loggerFactory.CreateLogger(GetType());
            }

            #region proeprty

            ILogger Logger { get; }

            #endregion

            #region IProgress

            public void Report(double value)
            {
                Logger.LogInformation("{0}", value);
            }

            #endregion
        }

        #endregion

        internal PluginsSettingEditorElement(PluginContainer pluginContainer, PluginConstructorContext pluginConstructorContext, Func<IDisposable> pauseReceiveLog, PreferencesContextFactory preferencesContextFactory, ISettingNotifyManager settingNotifyManager, IClipboardManager clipboardManager, IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, ITemporaryDatabaseBarrier temporaryDatabaseBarrier, IDatabaseStatementLoader statementLoader, IIdFactory idFactory, EnvironmentParameters environmentParameters, IHttpUserAgentFactory userAgentFactory, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(settingNotifyManager, clipboardManager, mainDatabaseBarrier, largeDatabaseBarrier, temporaryDatabaseBarrier, statementLoader, idFactory, imageLoader, mediaConverter, dispatcherWrapper, loggerFactory)
        {
            PluginContainer = pluginContainer;
            PluginConstructorContext = pluginConstructorContext;
            PreferencesContextFactory = preferencesContextFactory;
            PauseReceiveLog = pauseReceiveLog;

            UserAgentFactory = userAgentFactory;
            PlatformTheme = platformTheme;
            EnvironmentParameters = environmentParameters;

            PluginItems = new ReadOnlyObservableCollection<PluginSettingEditorElement>(PluginItemsImpl);
            InstallPluginItems = new ReadOnlyObservableCollection<PluginInstallItemElement>(InstallPluginItemsImpl);
        }

        #region property

        PreferencesContextFactory PreferencesContextFactory { get; }
        IHttpUserAgentFactory UserAgentFactory { get; }
        IPlatformTheme PlatformTheme { get; }
        EnvironmentParameters EnvironmentParameters { get; }

        PluginContainer PluginContainer { get; }
        PluginConstructorContext PluginConstructorContext { get; }
        Func<IDisposable> PauseReceiveLog { get; }

        ObservableCollection<PluginSettingEditorElement> PluginItemsImpl { get; } = new ObservableCollection<PluginSettingEditorElement>();
        public ReadOnlyObservableCollection<PluginSettingEditorElement> PluginItems { get; }

        ObservableCollection<PluginInstallItemElement> InstallPluginItemsImpl { get; } = new ObservableCollection<PluginInstallItemElement>();
        public ReadOnlyObservableCollection<PluginInstallItemElement> InstallPluginItems { get; }

        #endregion

        #region function

        private Task<DirectoryInfo> ExtractArchiveAsync(FileInfo archiveFile, string archiveKind, bool isManual)
        {
            Debug.Assert(new[] { "7z", "zip" }.Contains(archiveKind)); // enum 作っておかないからこうなる

            var dirName = Path.GetFileNameWithoutExtension(archiveFile.Name);
            var baseDir = isManual
                ? EnvironmentParameters.TemporaryPluginManualExtractDirectory
                : EnvironmentParameters.TemporaryPluginAutomaticExtractDirectory
            ;
            var dirPath = Path.Join(baseDir.FullName, dirName);
            var extractDirectory = new DirectoryInfo(dirPath);

            return Task.Run(() => {
                extractDirectory.Refresh();
                if(extractDirectory.Exists) {
                    throw new PluginDuplicateExtractDirectoryException(extractDirectory.FullName);
                }

                var archiveExtractor = new ArchiveExtractor(LoggerFactory);
                var progress = new UserNotifyProgress(new DoubleProgress(LoggerFactory), new StringProgress(LoggerFactory));
                archiveExtractor.Extract(archiveFile, extractDirectory, archiveKind, progress);

                return extractDirectory;
            });
        }

        private Task<FileInfo?> GetPluginFileAsync(DirectoryInfo pluginDirectory, string pluginName, IReadOnlyList<string> extensions)
        {
            var file = PluginContainer.GetPluginFile(pluginDirectory, pluginDirectory.Name, EnvironmentParameters.ApplicationConfiguration.Plugin.Extentions);
            if(file != null) {
                return Task.FromResult<FileInfo?>(file);
            }

            // ファイルなければディレクトリを掘り下げていく
            return Task.Run(() => {
                var dirs = pluginDirectory.EnumerateDirectories();
                foreach(var dir in dirs) {
                    var file = PluginContainer.GetPluginFile(dir, pluginDirectory.Name, EnvironmentParameters.ApplicationConfiguration.Plugin.Extentions);
                    if(file != null) {
                        return file;
                    }
                }

                return default;
            });
        }

        private async Task<PluginInstallItemElement> InstallPluginAsync(FileInfo archiveFile, string archiveKind, bool isManual)
        {
            var extractedDirectory = await ExtractArchiveAsync(archiveFile, archiveKind, isManual);

            var pluginFile = await GetPluginFileAsync(extractedDirectory, extractedDirectory.Name, EnvironmentParameters.ApplicationConfiguration.Plugin.Extentions);
            if(pluginFile == null) {
                extractedDirectory.Delete(true);
                throw new PluginNotFoundException(extractedDirectory.FullName);
            }

            var loadStateData = PluginContainer.LoadPlugin(pluginFile, Enumerable.Empty<PluginStateData>().ToList(), BuildStatus.Version, PluginConstructorContext, PauseReceiveLog);
            if(loadStateData.PluginId == Guid.Empty || loadStateData.LoadState != PluginState.Enable) {
                throw new PluginBrokenException(extractedDirectory.FullName);
            }

            Debug.Assert(loadStateData.Plugin != null);
            var info = loadStateData.Plugin.PluginInformations;
            var isUpdate = false;

            var installTargetPlugin = InstallPluginItemsImpl.FirstOrDefault(i => i.Informations.PluginIdentifiers.PluginId == info.PluginIdentifiers.PluginId);
            if(installTargetPlugin != null) {
                if(info.PluginVersions.PluginVersion <= installTargetPlugin.Informations.PluginVersions.PluginVersion) {
                    // すでに同一・新規バージョンがインストール対象になっている
                    throw new PluginInstallException($"{info.PluginVersions.PluginVersion}  <= {installTargetPlugin.Informations.PluginVersions.PluginVersion}");
                }
            } else {
                var installedPlugin = PluginContainer.Plugins.FirstOrDefault(i => i.PluginInformations.PluginIdentifiers.PluginId == info.PluginIdentifiers.PluginId);
                if(installedPlugin != null) {
                    if(info.PluginVersions.PluginVersion <= installedPlugin.PluginInformations.PluginVersions.PluginVersion) {
                        // すでに同一・新規バージョンがインストールされている
                        throw new PluginInstallException($"{info.PluginVersions.PluginVersion}  <= {installedPlugin.PluginInformations.PluginVersions.PluginVersion}");
                    }
                    isUpdate = true;
                }
            }

            if(!PluginUtility.IsUnlimitedVersion(info.PluginVersions.MinimumSupportVersion)) {
                if(BuildStatus.Version < info.PluginVersions.MinimumSupportVersion) {
                    // Pe バージョンがプラグインサポート最低バージョンを満たさない
                    throw new PluginInstallException($"[MIN] {BuildStatus.Version}  < {info.PluginVersions.MinimumSupportVersion}");
                }
            }
            if(!PluginUtility.IsUnlimitedVersion(info.PluginVersions.MaximumSupportVersion)) {
                if(info.PluginVersions.MaximumSupportVersion < BuildStatus.Version) {
                    // Pe バージョンがプラグインサポート最大バージョンを超過
                    throw new PluginInstallException($"[MAX] {info.PluginVersions.MaximumSupportVersion}  < {BuildStatus.Version}");
                }
            }

            var element = new PluginInstallItemElement(info, loadStateData.PluginVersion, isUpdate ? PluginInstallMode.Update : PluginInstallMode.New, LoggerFactory);

            // インストール対象のディレクトリを内部保持
            using(var context = TemporaryDatabaseBarrier.WaitWrite()) {
                context.GetDataTable("select * from InstallPlugins");

                var installPluginsEntityDao = new InstallPluginsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                if(installPluginsEntityDao.SelectExistsInstallPlugin(loadStateData.PluginId)) {
                    installPluginsEntityDao.DeleteInstallPlugin(loadStateData.PluginId);
                }
                installPluginsEntityDao.InsertInstallPlugin(loadStateData.PluginId, extractedDirectory.FullName, pluginFile.DirectoryName!, DatabaseCommonStatus.CreateCurrentAccount());

                context.Commit();
            }

            return element;
        }

        internal async Task InstallManualPluginTask(FileInfo archiveFile)
        {
            // 拡張子をアーカイブ種別としてそのまま使用する
            var exts = new HashSet<string>() {
                "7z",
                "zip",
            };
            var ext = archiveFile.Extension.Substring(1)?.ToLowerInvariant() ?? string.Empty;
            if(!exts.Contains(ext)) {
                throw new PluginInvalidArchiveKindException();
            }

            var element = await InstallPluginAsync(archiveFile, ext, true);
            InstallPluginItemsImpl.Add(element);
        }

        #endregion


        #region SettingEditorElementBase

        protected override void LoadImpl()
        {
            var pluginStates = MainDatabaseBarrier.ReadData(c => {
                var pluginsEntityDao = new PluginsEntityDao(c, DatabaseStatementLoader, c.Implementation, LoggerFactory);
                return pluginsEntityDao.SelectePlguinStateData().ToList();
            });

            // 標準テーマがなければ追加
            if(!pluginStates.Any(i => i.PluginId == DefaultTheme.Informations.PluginIdentifiers.PluginId)) {
                pluginStates.Insert(0, new Data.PluginStateData() {
                    PluginName = DefaultTheme.Informations.PluginIdentifiers.PluginName,
                    PluginId = DefaultTheme.Informations.PluginIdentifiers.PluginId,
                    State = Data.PluginState.Enable,
                });
            }

            foreach(var pluginState in pluginStates) {
                var plugin = PluginContainer.Plugins.FirstOrDefault(i => pluginState.PluginId == i.PluginInformations.PluginIdentifiers.PluginId);
                var element = new PluginSettingEditorElement(pluginState, plugin, PreferencesContextFactory, MainDatabaseBarrier, DatabaseStatementLoader, UserAgentFactory, PlatformTheme, ImageLoader, MediaConverter, DispatcherWrapper, LoggerFactory);
                element.Initialize();
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
