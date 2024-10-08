using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    /// <summary>
    /// プラグインアンインストール処理。
    /// </summary>
    public class PluginUninstaller
    {
        public PluginUninstaller(IDatabaseContextsPack databaseContextsPack, IDatabaseStatementLoader statementLoader, EnvironmentParameters environmentParameters, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());

            DatabaseContextsPack = databaseContextsPack;

            StatementLoader = statementLoader;
            EnvironmentParameters = environmentParameters;
        }

        #region property

        private IDatabaseContextsPack DatabaseContextsPack { get; }
        private IDatabaseStatementLoader StatementLoader { get; }
        private EnvironmentParameters EnvironmentParameters { get; }

        /// <inheritdoc cref="ILoggerFactory"/>
        private ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        private ILogger Logger { get; }

        private IDatabaseContexts MainContexts => DatabaseContextsPack.Main;
        private IDatabaseContexts FileContexts => DatabaseContextsPack.Large;
        private IDatabaseContexts TemporaryContexts => DatabaseContextsPack.Temporary;

        #endregion

        #region function

        private void UninstallFiles(IPluginIdentifiers pluginIdentifiers)
        {
            var pluginDirManager = new PluginDirectoryManager(EnvironmentParameters);
            var dataDirItems = new[] {
                new { Target = "一時", Directory = pluginDirManager.GetTemporaryDirectory(pluginIdentifiers) },
                new { Target = "端末", Directory = pluginDirManager.GetMachineDirectory(pluginIdentifiers) },
                new { Target = "ユーザー", Directory = pluginDirManager.GetUserDirectory(pluginIdentifiers) },
            };
            foreach(var dataDirItem in dataDirItems) {
                dataDirItem.Directory.Refresh();
                if(dataDirItem.Directory.Exists) {
                    Logger.LogDebug("プラグインデータディレクトリ削除: {0}, {1}", dataDirItem.Target, dataDirItem.Directory);
                    dataDirItem.Directory.Delete(true);
                } else {
                    Logger.LogDebug("プラグインデータディレクトリなし: {0}, {1}", dataDirItem.Target, dataDirItem.Directory);
                }
            }

            var moduleDirectory = pluginDirManager.GetModuleDirectory(pluginIdentifiers);
            moduleDirectory.Delete(true);
        }

        private void UninstallPersistence(IPluginIdentifiers pluginIdentifiers)
        {
            // デカいデータ破棄
            var pluginValuesEntityDao = new PluginValuesEntityDao(FileContexts.Context, StatementLoader, FileContexts.Implementation, LoggerFactory);
            pluginValuesEntityDao.DeletePluginValuesByPluginId(pluginIdentifiers.PluginId);

            // ウィジェットデータ破棄
            var pluginWidgetSettingsEntityDao = new PluginWidgetSettingsEntityDao(MainContexts.Context, StatementLoader, MainContexts.Implementation, LoggerFactory);
            pluginWidgetSettingsEntityDao.DeletePluginWidgetSettingsByPluginId(pluginIdentifiers.PluginId);

            // ランチャーアイテム系列の対象データを連鎖的に破棄(キー設定はきつない？)
            var launcherAddonsEntityDao = new LauncherAddonsEntityDao(MainContexts.Context, StatementLoader, MainContexts.Implementation, LoggerFactory);
            var deleteTargetLauncherItemIds = launcherAddonsEntityDao.SelectLauncherItemIdsByPluginId(pluginIdentifiers.PluginId).ToArray();
            launcherAddonsEntityDao.DeleteLauncherAddonsByPluginId(pluginIdentifiers.PluginId);

            var pluginVersionChecksEntityDao = new PluginVersionChecksEntityDao(MainContexts.Context, StatementLoader, MainContexts.Implementation, LoggerFactory);
            pluginVersionChecksEntityDao.DeletePluginVersionChecks(pluginIdentifiers.PluginId);

            var pluginLauncherItemSettingsEntityDao = new PluginLauncherItemSettingsEntityDao(MainContexts.Context, StatementLoader, MainContexts.Implementation, LoggerFactory);
            pluginLauncherItemSettingsEntityDao.DeletePluginLauncherItemSettingsByPluginId(pluginIdentifiers.PluginId);

            foreach(var deleteTargetLauncherItemId in deleteTargetLauncherItemIds) {
                var launcherEntityEraser = new LauncherEntityEraser(deleteTargetLauncherItemId, LauncherItemKind.Addon, MainContexts, FileContexts, TemporaryContexts, StatementLoader, LoggerFactory);
                launcherEntityEraser.Execute();
            }

            var pluginSettingsEntityDao = new PluginSettingsEntityDao(MainContexts.Context, StatementLoader, MainContexts.Implementation, LoggerFactory);
            pluginSettingsEntityDao.DeleteAllPluginSettings(pluginIdentifiers.PluginId);

            var pluginsEntityDao = new PluginsEntityDao(MainContexts.Context, StatementLoader, MainContexts.Implementation, LoggerFactory);
            pluginsEntityDao.DeletePlugin(pluginIdentifiers.PluginId);
        }

        public void Uninstall(IPluginIdentifiers pluginIdentifiers)
        {
            Logger.LogInformation("プラグインアンインストール: {0}", pluginIdentifiers);

            UninstallPersistence(pluginIdentifiers);
            UninstallFiles(pluginIdentifiers);
        }

        #endregion
    }
}
