using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Core.Models.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Addon;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Theme;
using ContentTypeTextNet.Pe.Plugins.DefaultTheme;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    internal abstract class PluginContainerBase
    {
        #region function

        /// <summary>
        /// コンテナの保持しているプラグイン一覧。
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<IPlugin> Plugins { get; }

        #endregion
    }

    internal class PluginContainer: PluginContainerBase
    {
        public PluginContainer(AddonContainer addonContainer, ThemeContainer themeContainer, EnvironmentParameters environmentParameters, ILoggerFactory loggerFactory)
        {
            Addon = addonContainer;
            Theme = themeContainer;
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            EnvironmentParameters = environmentParameters;
        }

        #region property

        ILogger Logger { get; }
        ILoggerFactory LoggerFactory { get; }
        EnvironmentParameters EnvironmentParameters { get; }
        HashSet<IPlugin> PluginsImpl { get; } = new HashSet<IPlugin>();
        /// <summary>
        /// アドオン用コンテナ。
        /// </summary>
        public AddonContainer Addon { get; }
        /// <summary>
        /// テーマ用コンテナ。
        /// </summary>
        public ThemeContainer Theme { get; }

        #endregion

        #region function

        public void UninstallPlugin(IPluginIdentifiers pluginIdentifiers, IDatabaseContexts mainContexts, IDatabaseContexts fileContexts, IDatabaseStatementLoader statementLoader, DirectoryInfo pluginDirectory)
        {
            Logger.LogInformation("プラグインアンインストール: {0}", pluginIdentifiers);

            // ファイル周り破棄
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

            // デカいデータ破棄
            var pluginValuesEntityDao = new PluginValuesEntityDao(fileContexts.Context, statementLoader, fileContexts.Implementation, LoggerFactory);
            pluginValuesEntityDao.DeletePluginValuesByPluginId(pluginIdentifiers.PluginId);

            // ウィジェットデータ破棄
            var pluginWidgetSettingsEntityDao = new PluginWidgetSettingsEntityDao(mainContexts.Context, statementLoader, mainContexts.Implementation, LoggerFactory);
            pluginWidgetSettingsEntityDao.DeletePluginWidgetSettingsByPluginId(pluginIdentifiers.PluginId);

            // ランチャーアイテム系列の対象データを連鎖的に破棄(キー設定はきつない？)
            var launcherAddonsEntityDao = new LauncherAddonsEntityDao(mainContexts.Context, statementLoader, mainContexts.Implementation, LoggerFactory);
            var deleteTargetLauncherItemIds = launcherAddonsEntityDao.SelectLauncherItemIdsByPluginId(pluginIdentifiers.PluginId).ToArray();
            launcherAddonsEntityDao.DeleteLauncherAddonsByPluginId(pluginIdentifiers.PluginId);

            var pluginLauncherItemSettingsEntityDao = new PluginLauncherItemSettingsEntityDao(mainContexts.Context, statementLoader, mainContexts.Implementation, LoggerFactory);
            pluginLauncherItemSettingsEntityDao.DeletePluginLauncherItemSettingsByPluginId(pluginIdentifiers.PluginId);

            foreach(var deleteTargetLauncherItemId in deleteTargetLauncherItemIds) {
                // ファイルDB側破棄
                var launcherItemIconsEntityDao = new LauncherItemIconsEntityDao(fileContexts.Context, statementLoader, fileContexts.Implementation, LoggerFactory);
                launcherItemIconsEntityDao.DeleteAllSizeImageBinary(deleteTargetLauncherItemId);

                var launcherItemIconStatusEntityDao = new LauncherItemIconStatusEntityDao(fileContexts.Context, statementLoader, fileContexts.Implementation, LoggerFactory);
                launcherItemIconStatusEntityDao.DeleteAllSizeLauncherItemIconState(deleteTargetLauncherItemId);

                // 通常DB側破棄
                var launcherEnvVarsEntityDao = new LauncherEnvVarsEntityDao(mainContexts.Context, statementLoader, mainContexts.Implementation, LoggerFactory);
                launcherEnvVarsEntityDao.DeleteEnvVarItemsByLauncherItemId(deleteTargetLauncherItemId);

                var launcherFilesEntityDao = new LauncherFilesEntityDao(mainContexts.Context, statementLoader, mainContexts.Implementation, LoggerFactory);
                launcherFilesEntityDao.DeleteFileByLauncherItemId(deleteTargetLauncherItemId);

                var launcherGroupItemsEntityDao = new LauncherGroupItemsEntityDao(mainContexts.Context, statementLoader, mainContexts.Implementation, LoggerFactory);
                launcherGroupItemsEntityDao.DeleteGroupItemsByLauncherItemId(deleteTargetLauncherItemId);

                var launcherItemHistoriesEntityDao = new LauncherItemHistoriesEntityDao(mainContexts.Context, statementLoader, mainContexts.Implementation, LoggerFactory);
                launcherItemHistoriesEntityDao.DeleteHistoriesByLauncherItemId(deleteTargetLauncherItemId);

                var launcherTagsEntityDao = new LauncherTagsEntityDao(mainContexts.Context, statementLoader, mainContexts.Implementation, LoggerFactory);
                launcherTagsEntityDao.DeleteTagByLauncherItemId(deleteTargetLauncherItemId);

                var launcherRedoItemsEntityDao = new LauncherRedoItemsEntityDao(mainContexts.Context, statementLoader, mainContexts.Implementation, LoggerFactory);
                launcherRedoItemsEntityDao.DeleteRedoItemByLauncherItemId(deleteTargetLauncherItemId);

                var launcherRedoSuccessExitCodesEntityDao = new LauncherRedoSuccessExitCodesEntityDao(mainContexts.Context, statementLoader, mainContexts.Implementation, LoggerFactory);
                launcherRedoSuccessExitCodesEntityDao.DeleteSuccessExitCodes(deleteTargetLauncherItemId);

                var launcherItemsEntityDao = new LauncherItemsEntityDao(mainContexts.Context, statementLoader, mainContexts.Implementation, LoggerFactory);
                launcherItemsEntityDao.DeleteLauncherItem(deleteTargetLauncherItemId);
            }

            var pluginSettingsEntityDao = new PluginSettingsEntityDao(mainContexts.Context, statementLoader, mainContexts.Implementation, LoggerFactory);
            pluginSettingsEntityDao.DeleteAllPluginSettings(pluginIdentifiers.PluginId);

            var pluginsEntityDao = new PluginsEntityDao(mainContexts.Context, statementLoader, mainContexts.Implementation, LoggerFactory);
            pluginsEntityDao.DeletePlugin(pluginIdentifiers.PluginId);
        }

        public FileInfo? GetPluginFile(DirectoryInfo pluginDirectory, string pluginName, IReadOnlyList<string> extensions)
        {
            foreach(var extension in extensions) {
                var pluginFileName = PathUtility.AppendExtension(pluginName, extension);
                var pluginPath = Path.Combine(pluginDirectory.FullName, pluginFileName);
                bool existsPlugin;
                try {
                    existsPlugin = File.Exists(pluginPath);
                } catch(Exception ex) {
                    Logger.LogError(ex, "プラグイン実ファイル取得失敗: {0}, {1}", ex.Message, pluginPath);
                    continue;
                }

                if(existsPlugin) {
                    return new FileInfo(pluginPath);
                }
            }

            return null;
        }

        /// <summary>
        /// プラグインの実ファイル一覧を取得。
        /// <para>Pe 付属のプラグイン(<see cref="DefaultTheme"/>とか)は含まれない。</para>
        /// </summary>
        /// <param name="baseDirectory"></param>
        /// <returns></returns>
        public IEnumerable<FileInfo> GetPluginFiles(DirectoryInfo baseDirectory, IReadOnlyList<string> extensions)
        {
            var pluginDirs = baseDirectory.EnumerateDirectories();
            foreach(var pluginDir in pluginDirs) {
                var pluginFile = GetPluginFile(pluginDir, pluginDir.Name, extensions);
                if(pluginFile != null) {
                    yield return pluginFile;
                }
            }
        }

        /// <summary>
        /// プラグインの読み込み。
        /// <para>検証結果がダメダメな場合は解放される。</para>
        /// </summary>
        /// <param name="pluginFile"></param>
        /// <returns>読み込み結果。</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public PluginLoadStateData LoadPlugin(FileInfo pluginFile, List<PluginStateData> pluginStateItems, Version applicationVersion, PluginConstructorContext pluginConstructorContext, Func<IDisposable> pauseReceiveLog)
        {
            var pluginBaseName = Path.GetFileNameWithoutExtension(pluginFile.Name);
            var currentPlugin = pluginStateItems.FirstOrDefault(i => string.Equals(pluginBaseName, i.PluginName, StringComparison.InvariantCultureIgnoreCase));
            if(currentPlugin != null) {
                if(currentPlugin.State == PluginState.Disable) {
                    Logger.LogInformation("(名前判定)プラグイン読み込み停止中: {0}, {1}", currentPlugin.PluginName, currentPlugin.PluginId);
                    return new PluginLoadStateData(currentPlugin.PluginId, currentPlugin.PluginName, new Version(), PluginState.Disable, null, null);
                }
            }
            var libraryDirectories = new[] {
                EnvironmentParameters.ApplicationDirectory,
            };
            var loadContext = new PluginAssemblyLoadContext(pluginFile, libraryDirectories, LoggerFactory);
            Assembly pluginAssembly;
            try {
                pluginAssembly = loadContext.Load();
            } catch(Exception ex) {
                Logger.LogError(ex, "プラグインアセンブリ読み込み失敗: {0}", pluginFile.Name);
                loadContext.Unload();
                return new PluginLoadStateData(currentPlugin?.PluginId ?? Guid.Empty, currentPlugin?.PluginName ?? pluginFile.Name, new Version(), PluginState.IllegalAssembly, new WeakReference<PluginAssemblyLoadContext>(loadContext), null);
            }

            Type? pluginInterfaceImpl = null;
            try {
                // 型情報が変な場合、例外が投げられるがそいつはなんかもう解放できなくなる
                var pluginTypes = pluginAssembly.GetTypes();//.Where(i => !(i.IsAbstract || i.IsNotPublic));
                foreach(var pluginType in pluginTypes) {
                    if(pluginType.IsAbstract || pluginType.IsNotPublic) {
                        continue;
                    }
                    Logger.LogDebug("{0}", pluginType.FullName);

                    var typeInterfaces = pluginType.GetInterfaces();
                    foreach(var typeInterface in typeInterfaces) {
                        Logger.LogDebug("> {0}", typeInterface.FullName);
                    }
                    var plugins = typeInterfaces.FirstOrDefault(i => i == typeof(IPlugin));
                    if(plugins != null) {
                        pluginInterfaceImpl = pluginType;
                        break;
                    }
                }
            } catch(Exception ex) {
                Logger.LogError(ex, "プラグインアセンブリ リフレクション失敗: {0}", pluginFile.Name);
                loadContext.Unload();
                return new PluginLoadStateData(currentPlugin?.PluginId ?? Guid.Empty, currentPlugin?.PluginName ?? pluginFile.Name, new Version(), PluginState.IllegalAssembly, new WeakReference<PluginAssemblyLoadContext>(loadContext), null);
            }

            if(pluginInterfaceImpl == null) {
                Logger.LogError("プラグインアセンブリからプラグインインターフェイス取得できず: {0}, {1}", pluginAssembly.FullName, pluginFile.FullName);
                loadContext.Unload();
                return new PluginLoadStateData(currentPlugin?.PluginId ?? Guid.Empty, currentPlugin?.PluginName ?? pluginFile.Name, new Version(), PluginState.IllegalAssembly, new WeakReference<PluginAssemblyLoadContext>(loadContext), null);
            }

            Logger.LogDebug("[{0}]", pluginInterfaceImpl.FullName);
            foreach(var constructor in pluginInterfaceImpl.GetConstructors()) {
                var paras = string.Join(", ", constructor.GetParameters().Select(i => $"{i.ParameterType.FullName} {i.Name}"));
                Logger.LogDebug("-> {0}", paras);

            }

            IPlugin plugin;
            try {
                // コンストラクタ時にメモリログが参照に残るのを抑制
                using(pauseReceiveLog()) {
                    var obj = Activator.CreateInstance(pluginInterfaceImpl, new[] { pluginConstructorContext })!;
                    plugin = (IPlugin)obj ?? throw new Exception($"{nameof(IPlugin)}へのキャスト失敗: {obj}");
                }
            } catch(Exception ex) {
                Logger.LogError(ex, "プラグインインターフェイスを生成できず: {0}, {1}, {2}", ex.Message, pluginAssembly.FullName, pluginFile.FullName);
                loadContext.Unload();
                return new PluginLoadStateData(currentPlugin?.PluginId ?? Guid.Empty, currentPlugin?.PluginName ?? pluginFile.Name, new Version(), PluginState.IllegalAssembly, new WeakReference<PluginAssemblyLoadContext>(loadContext), null);
            }

            IPluginInformations info;
            // プラグイン情報取得時にメモリログに参照が残るのをよく抑制(情報取得だけの局所的処理)
            using(pauseReceiveLog()) {
                info = plugin.PluginInformations;
            }
            var pluginId = info.PluginIdentifiers.PluginId;
            var pluginName = new string(info.PluginIdentifiers.PluginName.ToCharArray()); // 一応複製

            var loadedCurrentPlugin = pluginStateItems.FirstOrDefault(i => i.PluginId == pluginId);
            if(loadedCurrentPlugin != null) {
                if(loadedCurrentPlugin.State == PluginState.Disable) {
                    Logger.LogInformation("(ID判定)プラグイン読み込み停止中: {0}({1}), {2}", loadedCurrentPlugin.PluginName, pluginName, loadedCurrentPlugin.PluginId);
                    loadContext.Unload();
                    return new PluginLoadStateData(loadedCurrentPlugin.PluginId, pluginName, new Version(), PluginState.Disable, new WeakReference<PluginAssemblyLoadContext>(loadContext), null);
                }
            }

            var pluginVersion = (Version)info.PluginVersions.PluginVersion.Clone();

            var unlimitVersion = new Version(0, 0, 0);

            if(info.PluginVersions.MinimumSupportVersion != unlimitVersion) {
                var ok = info.PluginVersions.MinimumSupportVersion <= applicationVersion;
                if(!ok) {
                    Logger.LogWarning("プラグインサポート最低バージョン({0}): {1}, {2}", info.PluginVersions.MinimumSupportVersion, pluginName, pluginId);
                    loadContext.Unload();
                    return new PluginLoadStateData(pluginId, pluginName, pluginVersion, PluginState.IllegalVersion, new WeakReference<PluginAssemblyLoadContext>(loadContext), null);
                }
            }

            if(info.PluginVersions.MaximumSupportVersion != unlimitVersion) {
                var ok = applicationVersion <= info.PluginVersions.MaximumSupportVersion;
                if(!ok) {
                    Logger.LogWarning("プラグインサポート最高バージョン({0}): {1}, {2}", info.PluginVersions.MaximumSupportVersion, pluginName, pluginId);
                    loadContext.Unload();
                    return new PluginLoadStateData(pluginId, pluginName, pluginVersion, PluginState.IllegalVersion, new WeakReference<PluginAssemblyLoadContext>(loadContext), null);
                }
            }

            // 読み込み対象！
            Logger.LogInformation("プラグイン読み込み対象: {0}, {1}", pluginName, pluginId);
            return new PluginLoadStateData(pluginId, pluginName, pluginVersion, PluginState.Enable, new WeakReference<PluginAssemblyLoadContext>(loadContext), plugin);
        }

        /// <summary>
        /// プラグインの実体をコンテナに取り込み。
        /// </summary>
        /// <param name="plugin"></param>
        public void AddPlugin(IPlugin plugin)
        {
            PluginsImpl.Add(plugin);

            if(plugin is ITheme theme) {
                Theme.Add(theme);
            }
            if(plugin is IAddon addon) {
                Addon.Add(addon);
            }
        }

        #endregion

        #region PluginContainerBase

        /// <inheritdoc cref="PluginContainerBase.Plugins"/>
        public override IEnumerable<IPlugin> Plugins => PluginsImpl;


        #endregion
    }
}
