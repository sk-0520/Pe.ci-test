using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Library.Base;
using ContentTypeTextNet.Pe.Core.Models.Serialization;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Library.Database;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    /// <summary>
    /// プラグインインストール。
    /// </summary>
    internal class PluginInstaller
    {
        public PluginInstaller(PluginContainer pluginContainer, IPluginConstructorContext pluginConstructorContext, PauseReceiveLogDelegate pauseReceiveLog, EnvironmentParameters environmentParameters, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            PluginContainer = pluginContainer;
            PluginConstructorContext = pluginConstructorContext;
            PauseReceiveLog = pauseReceiveLog;
            EnvironmentParameters = environmentParameters;
            DatabaseStatementLoader = databaseStatementLoader;
        }

        #region property

        private ILoggerFactory LoggerFactory { get; }
        private ILogger Logger { get; }

        private PluginContainer PluginContainer { get; }
        private IPluginConstructorContext PluginConstructorContext { get; }
        private PauseReceiveLogDelegate PauseReceiveLog { get; }
        private EnvironmentParameters EnvironmentParameters { get; }
        private IDatabaseStatementLoader DatabaseStatementLoader { get; }

        private IReadOnlySet<string> Extensions { get; } = new HashSet<string>() {
            "7z",
            "zip",
        };

        #endregion

        #region function

        /// <summary>
        /// 現在のインストール対象プラグインからインストールの取消。
        /// </summary>
        /// <param name="pluginId">対象プラグインID。</param>
        /// <param name="installPluginItems">インストール対象プラグイン。</param>
        /// <param name="temporaryDatabaseBarrier"></param>
        /// <returns>キャンセルできたか。偽の場合はキャンセル失敗って言うより対象になかったって感じ。</returns>
        public bool CancelInstall(PluginId pluginId, IEnumerable<IPluginId> installPluginItems, ITemporaryDatabaseBarrier temporaryDatabaseBarrier)
        {
            var removeTarget = installPluginItems.FirstOrDefault(i => i.PluginId == pluginId);
            if(removeTarget == null) {
                return false;
            }

            //string extractDirectoryPath;
            using(var context = temporaryDatabaseBarrier.WaitWrite()) {
                var installPluginsEntityDao = new InstallPluginsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                if(!installPluginsEntityDao.SelectExistsInstallPluginByPluginId(removeTarget.PluginId)) {
                    return false;
                }

                //extractDirectoryPath = installPluginsEntityDao.SelectExtractedDirectoryPath(removeTarget.Data.PluginId);
                installPluginsEntityDao.DeleteInstallPlugin(removeTarget.PluginId);

                context.Commit();
            }

            // 展開ディレクトリは破棄しない(起動処理にお任せ)
            return true;
        }

        private Task<DirectoryInfo> ExtractArchiveAsync(FileInfo archiveFile, string archiveKind, bool isManual)
        {
            Debug.Assert(new[] { "7z", "zip" }.Contains(archiveKind)); // enum 作っておかないからこうなる

            var dirName = Path.GetFileNameWithoutExtension(archiveFile.Name) + DateTime.Now.ToString("_yyyy-MM-dd'T'HHmmss", CultureInfo.InvariantCulture);
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
                var progress = new UserNotifyProgress(new NullDoubleProgress(LoggerFactory), new NullStringProgress(LoggerFactory));
                archiveExtractor.Extract(archiveFile, extractDirectory, archiveKind, progress);

                return extractDirectory;
            });
        }

        private Task<FileInfo?> GetPluginFileAsync(DirectoryInfo pluginDirectory, string pluginName, IReadOnlyList<string> extensions)
        {
            var file = PluginContainer.GetPluginFile(pluginDirectory, pluginName, extensions);
            if(file != null) {
                return Task.FromResult<FileInfo?>(file);
            }

            // ファイルなければディレクトリを掘り下げていく
            return Task.Run(() => {
                var dirs = pluginDirectory.EnumerateDirectories();
                foreach(var dir in dirs) {
                    var file = PluginContainer.GetPluginFile(dir, pluginName, extensions);
                    if(file != null) {
                        return file;
                    }
                }

                return default;
            });
        }

        public string GetArchiveExtension(FileInfo archiveFile)
        {
            var ext = archiveFile.Extension.Substring(1).ToLowerInvariant();
            if(!Extensions.Contains(ext)) {
                throw new PluginInvalidArchiveKindException();
            }
            return ext;
        }

        /// <summary>
        /// アーカイブファイルからプラグイン名を取得。
        /// </summary>
        /// <remarks>
        /// <para>性善説を信じたファイル名判定。</para>
        /// </remarks>
        /// <param name="archiveFile"></param>
        /// <returns></returns>
        /// <exception cref="PluginArchiveNameException" />
        public string GetPluginName(FileInfo archiveFile)
        {
            var pluginFileName = Path.GetFileNameWithoutExtension(archiveFile.Name);
            if(string.IsNullOrEmpty(pluginFileName)) {
                throw new PluginArchiveNameException($"ファイル名不明: {archiveFile}");
            }

            var endWords = new[] {
                "_x86",
                "_x64",
                "_AnyCPU",
            };
            foreach(var endWord in endWords) {
                if(pluginFileName.EndsWith(endWord, StringComparison.InvariantCultureIgnoreCase)) {
                    return pluginFileName.Substring(0, pluginFileName.Length - endWord.Length);
                }
            }

            return pluginFileName;
        }

        private IPluginLoadState LoadDirectPlugin(FileInfo pluginFile, DirectoryInfo pluginDirectory)
        {
            var loadStateData2 = PluginContainer.LoadPlugin(pluginFile, Enumerable.Empty<PluginStateData>().ToList(), BuildStatus.Version, PluginConstructorContext, PauseReceiveLog);
            if(loadStateData2.PluginId == PluginId.Empty || loadStateData2.LoadState != PluginState.Enable) {
                // なんかもうダメっぽい
                throw new PluginBrokenException(pluginDirectory.FullName);
            }
            Debug.Assert(loadStateData2.Plugin != null);
            var weakLoadContext = new WeakReference<PluginAssemblyLoadContext>(loadStateData2.FreeLoadContext());
            if(weakLoadContext.TryGetTarget(out var pluginLoadContext)) {
                try {
                    pluginLoadContext.Unload();
                } catch(InvalidOperationException ex) {
                    Logger.LogError(ex, ex.Message);
                }
            }

            return loadStateData2;
        }

        private IPluginLoadState LoadProcessPlugin(FileInfo pluginFile, DirectoryInfo pluginDirectory)
        {
            var applicationBoot = new ApplicationBoot(EnvironmentParameters, LoggerFactory);
            var keyValueArguments = new Dictionary<string, string> {
                [InterProcessCommunicationManager.CommandLineKeyIpcFile] = pluginFile.FullName,
            };

            IpcDataPluginStatus? data = null;
            applicationBoot.TryExecuteIpc(IpcMode.GetPluginStatus, keyValueArguments, Array.Empty<string>(), (c, o) => {
                var binary = Encoding.UTF8.GetBytes(o);
                using var stream = new MemoryReleaseStream(binary);

                var serializer = new JsonTextSerializer();
                data = serializer.Load<IpcDataPluginStatus>(stream);
            });
            if(data is null) {
                throw new PluginBrokenException();
            }

            return data;
        }

        public IPluginLoadState LoadPluginInfo(FileInfo pluginFile)
        {
            return LoadDirectPlugin(pluginFile, pluginFile.Directory!);
        }

        /// <summary>
        /// プラグインアーカイブファイルをプラグインインストール完了(仮)状態へ処理する。
        /// </summary>
        /// <param name="pluginName"></param>
        /// <param name="archiveFile"></param>
        /// <param name="archiveKind"></param>
        /// <param name="isManual"></param>
        /// <param name="installPluginItems"></param>
        /// <param name="pluginInstallAssemblyMode">プラグインアセンブリの読み込み方法。</param>
        /// <param name="temporaryDatabaseBarrier"></param>
        /// <returns></returns>
        public async Task<PluginInstallData> InstallPluginArchiveAsync(FileInfo archiveFile, string archiveKind, bool isManual, IEnumerable<PluginInstallData> installPluginItems, PluginInstallAssemblyMode pluginInstallAssemblyMode, ITemporaryDatabaseBarrier temporaryDatabaseBarrier)
        {
            var extractedDirectory = await ExtractArchiveAsync(archiveFile, archiveKind, isManual).ConfigureAwait(false);

            var pluginFile = await GetPluginFileAsync(extractedDirectory, string.Empty, Array.Empty<string>()).ConfigureAwait(false);
            if(pluginFile == null) {
                // プラグインが見つかんない
                extractedDirectory.Delete(true);
                throw new PluginNotFoundException(extractedDirectory.FullName);
            }

            var loadStateData = pluginInstallAssemblyMode switch {
                PluginInstallAssemblyMode.Direct => LoadDirectPlugin(pluginFile, extractedDirectory),
                PluginInstallAssemblyMode.Process => LoadProcessPlugin(pluginFile, extractedDirectory),
                _ => throw new NotImplementedException(),
            };

            var isUpdate = false;

            var installTargetPlugin = installPluginItems.FirstOrDefault(i => i.PluginId == loadStateData.PluginId);
            if(installTargetPlugin != null) {
                if(loadStateData.PluginVersion <= installTargetPlugin.PluginVersion) {
                    // すでに同一・新規バージョンがインストール対象になっている
                    throw new PluginInstallException($"{loadStateData.PluginVersion}  <= {installTargetPlugin.PluginVersion}");
                }
            } else {
                var installedPlugin = PluginContainer.Plugins.FirstOrDefault(i => i.PluginInformation.PluginIdentifiers.PluginId == loadStateData.PluginId);
                if(installedPlugin != null) {
                    if(loadStateData.PluginVersion <= installedPlugin.PluginInformation.PluginVersions.PluginVersion) {
                        // すでに同一・新規バージョンがインストールされている
                        throw new PluginInstallException($"{loadStateData.PluginVersion}  <= {installedPlugin.PluginInformation.PluginVersions.PluginVersion}");
                    }
                    isUpdate = true;
                }
            }

            var data = new PluginInstallData(loadStateData.PluginId, loadStateData.PluginName, loadStateData.PluginVersion, isUpdate ? PluginInstallMode.Update : PluginInstallMode.New, extractedDirectory.FullName, pluginFile.DirectoryName!);

            // インストール対象のディレクトリを内部保持
            using(var context = temporaryDatabaseBarrier.WaitWrite()) {
                var installPluginsEntityDao = new InstallPluginsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                if(installPluginsEntityDao.SelectExistsInstallPluginByPluginId(loadStateData.PluginId)) {
                    installPluginsEntityDao.DeleteInstallPlugin(loadStateData.PluginId);
                }
                installPluginsEntityDao.InsertInstallPlugin(data, DatabaseCommonStatus.CreateCurrentAccount());

                context.Commit();
            }

            return data;
        }

        #endregion
    }
}
