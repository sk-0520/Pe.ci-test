using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
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

            #region property

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

            #region property

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

        internal PluginsSettingEditorElement(PluginContainer pluginContainer, PluginConstructorContext pluginConstructorContext, Func<IDisposable> pauseReceiveLog, PreferencesContextFactory preferencesContextFactory, ISettingNotifyManager settingNotifyManager, IClipboardManager clipboardManager, IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, ITemporaryDatabaseBarrier temporaryDatabaseBarrier, IDatabaseStatementLoader statementLoader, IIdFactory idFactory, EnvironmentParameters environmentParameters, IHttpUserAgentFactory userAgentFactory, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IPolicy policy, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(settingNotifyManager, clipboardManager, mainDatabaseBarrier, largeDatabaseBarrier, temporaryDatabaseBarrier, statementLoader, idFactory, imageLoader, mediaConverter, policy, dispatcherWrapper, loggerFactory)
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

        public void CancelInstall(Guid pluginId)
        {
            var removeTarget = InstallPluginItemsImpl.FirstOrDefault(i => i.Data.PluginId == pluginId);
            if(removeTarget == null) {
                return;
            }
            InstallPluginItemsImpl.Remove(removeTarget);

            //string extractDirectoryPath;
            using(var context = TemporaryDatabaseBarrier.WaitWrite()) {
                var installPluginsEntityDao = new InstallPluginsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                if(!installPluginsEntityDao.SelectExistsInstallPlugin(removeTarget.Data.PluginId)) {
                    return;
                }

                //extractDirectoryPath = installPluginsEntityDao.SelectExtractedDirectoryPath(removeTarget.Data.PluginId);
                installPluginsEntityDao.DeleteInstallPlugin(removeTarget.Data.PluginId);

                context.Commit();
            }

            // 展開ディレクトリは破棄しない(起動処理にお任せ)
        }

        private Task<DirectoryInfo> ExtractArchiveAsync(FileInfo archiveFile, string archiveKind, bool isManual)
        {
            Debug.Assert(new[] { "7z", "zip" }.Contains(archiveKind)); // enum 作っておかないからこうなる

            var dirName = Path.GetFileNameWithoutExtension(archiveFile.Name) + DateTime.Now.ToString("_yyyy-MM-dd'T'HHmmss");
            var baseDir = isManual
                ? EnvironmentParameters.TemporaryPluginManualExtractDirectory
                : EnvironmentParameters.TemporaryPluginAutomaticExtractDirectory
            ;
            var dirPath = Path.Join(baseDir.FullName, dirName);
            var extractDirectory = new DirectoryInfo(dirPath);

            return Task.Run(() => {
                extractDirectory.Refresh();
                if(extractDirectory.Exists) {
                    // 展開ディレクトリが既にある
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
            var file = PluginContainer.GetPluginFile(pluginDirectory, pluginName, EnvironmentParameters.ApplicationConfiguration.Plugin.Extentions);
            if(file != null) {
                return Task.FromResult<FileInfo?>(file);
            }

            // ファイルなければディレクトリを掘り下げていく
            return Task.Run(() => {
                var dirs = pluginDirectory.EnumerateDirectories();
                foreach(var dir in dirs) {
                    var file = PluginContainer.GetPluginFile(dir, pluginName, EnvironmentParameters.ApplicationConfiguration.Plugin.Extentions);
                    if(file != null) {
                        return file;
                    }
                }

                return default;
            });
        }

        private async Task<PluginInstallItemElement> InstallPluginAsync(string pluginName, FileInfo archiveFile, string archiveKind, bool isManual)
        {
            var extractedDirectory = await ExtractArchiveAsync(archiveFile, archiveKind, isManual);

            var pluginFile = await GetPluginFileAsync(extractedDirectory, pluginName, EnvironmentParameters.ApplicationConfiguration.Plugin.Extentions);
            if(pluginFile == null) {
                // プラグインが見つかんない
                extractedDirectory.Delete(true);
                throw new PluginNotFoundException(extractedDirectory.FullName);
            }

            var loadStateData = PluginContainer.LoadPlugin(pluginFile, Enumerable.Empty<PluginStateData>().ToList(), BuildStatus.Version, PluginConstructorContext, PauseReceiveLog);
            if(loadStateData.PluginId == Guid.Empty || loadStateData.LoadState != PluginState.Enable) {
                // なんかもうダメっぽい
                throw new PluginBrokenException(extractedDirectory.FullName);
            }
            Debug.Assert(loadStateData.Plugin != null);
            Debug.Assert(loadStateData.WeekLoadContext != null);
            if(loadStateData.WeekLoadContext.TryGetTarget(out var pluginLoadContext)) {
                try {
                    pluginLoadContext.Unload();
                } catch(InvalidOperationException ex) {
                    Logger.LogError(ex, ex.Message);
                }
            }


            var isUpdate = false;

            var installTargetPlugin = InstallPluginItems.FirstOrDefault(i => i.Data.PluginId == loadStateData.PluginId);
            if(installTargetPlugin != null) {
                if(loadStateData.PluginVersion <= installTargetPlugin.Data.PluginVersion) {
                    // すでに同一・新規バージョンがインストール対象になっている
                    throw new PluginInstallException($"{loadStateData.PluginVersion}  <= {installTargetPlugin.Data.PluginVersion}");
                }
            } else {
                var installedPlugin = PluginContainer.Plugins.FirstOrDefault(i => i.PluginInformations.PluginIdentifiers.PluginId == loadStateData.PluginId);
                if(installedPlugin != null) {
                    if(loadStateData.PluginVersion <= installedPlugin.PluginInformations.PluginVersions.PluginVersion) {
                        // すでに同一・新規バージョンがインストールされている
                        throw new PluginInstallException($"{loadStateData.PluginVersion}  <= {installedPlugin.PluginInformations.PluginVersions.PluginVersion}");
                    }
                    isUpdate = true;
                }
            }

            var data = new PluginInstallData(loadStateData.PluginId, loadStateData.PluginName, loadStateData.PluginVersion, isUpdate ? PluginInstallMode.Update : PluginInstallMode.New, extractedDirectory.FullName, pluginFile.DirectoryName!);
            var element = new PluginInstallItemElement(data, LoggerFactory);
            element.Initialize();

            // インストール対象のディレクトリを内部保持
            using(var context = TemporaryDatabaseBarrier.WaitWrite()) {
                var installPluginsEntityDao = new InstallPluginsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                if(installPluginsEntityDao.SelectExistsInstallPlugin(loadStateData.PluginId)) {
                    installPluginsEntityDao.DeleteInstallPlugin(loadStateData.PluginId);
                }
                installPluginsEntityDao.InsertInstallPlugin(element.Data, DatabaseCommonStatus.CreateCurrentAccount());

                context.Commit();
            }

            return element;
        }

        private void MergeInstallPlugin(PluginInstallItemElement element)
        {
            var oldElement = InstallPluginItemsImpl.FirstOrDefault(i => i.Data.PluginId == element.Data.PluginId);
            if(oldElement != null) {
                InstallPluginItemsImpl.Remove(oldElement);
            }
            InstallPluginItemsImpl.Add(element);

        }

        internal async Task InstallLocalPluginAsync(FileInfo archiveFile)
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
            var pluginFileName = Path.GetFileNameWithoutExtension(archiveFile.Name)!;

            var element = await InstallPluginAsync(pluginFileName, archiveFile, ext, true);
            MergeInstallPlugin(element);
        }

        #endregion


        #region SettingEditorElementBase

        protected override void LoadImpl()
        {
            IList<PluginStateData> pluginStates;
            using(var context = MainDatabaseBarrier.WaitRead()) {
                var pluginsEntityDao = new PluginsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                pluginStates = pluginsEntityDao.SelectePlguinStateData().ToList();
            }

            IList<PluginInstallData> installDataItems;
            using(var context = TemporaryDatabaseBarrier.WaitRead()) {
                var installPluginsEntityDao = new InstallPluginsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                installDataItems = installPluginsEntityDao.SelectInstallPlugins().ToList();
            }

            foreach(var installDataItem in installDataItems) {
                var element = new PluginInstallItemElement(installDataItem, LoggerFactory);
                element.Initialize();
                InstallPluginItemsImpl.Add(element);
            }

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
                var element = new PluginSettingEditorElement(pluginState, plugin, PreferencesContextFactory, MainDatabaseBarrier, DatabaseStatementLoader, UserAgentFactory, PlatformTheme, ImageLoader, MediaConverter, Policy, DispatcherWrapper, LoggerFactory);
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
