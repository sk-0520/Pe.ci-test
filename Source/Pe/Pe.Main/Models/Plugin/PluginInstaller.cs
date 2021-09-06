using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    internal class PluginInstaller
    {
        public PluginInstaller(PluginContainer pluginContainer, IPluginConstructorContext pluginConstructorContext, Func<IDisposable> pauseReceiveLog, EnvironmentParameters environmentParameters, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
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

        ILoggerFactory LoggerFactory { get; }
        ILogger Logger { get; }

        PluginContainer PluginContainer { get; }
        IPluginConstructorContext PluginConstructorContext { get; }
        Func<IDisposable> PauseReceiveLog { get; }
        EnvironmentParameters EnvironmentParameters { get; }
        IDatabaseStatementLoader DatabaseStatementLoader { get; }

        IReadOnlySet<string> Extensions { get; } = new HashSet<string>() {
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
        public bool CancelInstall(Guid pluginId, ICollection<IPluginId> installPluginItems, ITemporaryDatabaseBarrier temporaryDatabaseBarrier)
        {
            var removeTarget = installPluginItems.FirstOrDefault(i => i.PluginId == pluginId);
            if(removeTarget == null) {
                return false;
            }
            installPluginItems.Remove(removeTarget);

            //string extractDirectoryPath;
            using(var context = temporaryDatabaseBarrier.WaitWrite()) {
                var installPluginsEntityDao = new InstallPluginsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                if(!installPluginsEntityDao.SelectExistsInstallPlugin(removeTarget.PluginId)) {
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

        private async Task<PluginInstallData> InstallPluginAsync(string pluginName, FileInfo archiveFile, string archiveKind, bool isManual, IEnumerable<PluginInstallData> installPluginItems, ITemporaryDatabaseBarrier temporaryDatabaseBarrier)
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

            var installTargetPlugin = installPluginItems.FirstOrDefault(i => i.PluginId == loadStateData.PluginId);
            if(installTargetPlugin != null) {
                if(loadStateData.PluginVersion <= installTargetPlugin.PluginVersion) {
                    // すでに同一・新規バージョンがインストール対象になっている
                    throw new PluginInstallException($"{loadStateData.PluginVersion}  <= {installTargetPlugin.PluginVersion}");
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

            // インストール対象のディレクトリを内部保持
            using(var context = temporaryDatabaseBarrier.WaitWrite()) {
                var installPluginsEntityDao = new InstallPluginsEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                if(installPluginsEntityDao.SelectExistsInstallPlugin(loadStateData.PluginId)) {
                    installPluginsEntityDao.DeleteInstallPlugin(loadStateData.PluginId);
                }
                installPluginsEntityDao.InsertInstallPlugin(data, DatabaseCommonStatus.CreateCurrentAccount());

                context.Commit();
            }

            return data;
        }

        private string GetArchiveExtension(FileInfo archiveFile)
        {
            var ext = archiveFile.Extension.Substring(1)?.ToLowerInvariant() ?? string.Empty;
            if(!Extensions.Contains(ext)) {
                throw new PluginInvalidArchiveKindException();
            }
            return ext;
        }

        private string GetPluginName(FileInfo archiveFile)
        {
            var pluginFileName = Path.GetFileNameWithoutExtension(archiveFile.Name);
            if(string.IsNullOrEmpty(pluginFileName)) {
                throw new Exception($"ファイル名不明: {archiveFile}");
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

        #endregion
    }
}
