using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public PluginInstaller(PluginContainer pluginContainer, EnvironmentParameters environmentParameters, IDatabaseStatementLoader databaseStatementLoader, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            PluginContainer = pluginContainer;
            EnvironmentParameters = environmentParameters;
            DatabaseStatementLoader = databaseStatementLoader;
        }

        #region property

        ILoggerFactory LoggerFactory { get; }
        ILogger Logger { get; }

        PluginContainer PluginContainer { get; }
        EnvironmentParameters EnvironmentParameters { get; }
        IDatabaseStatementLoader DatabaseStatementLoader { get; }

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

        #endregion
    }
}
