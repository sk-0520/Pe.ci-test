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
using ContentTypeTextNet.Pe.Core.Models.DependencyInjection;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Addon;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Theme;
using ContentTypeTextNet.Pe.Plugins.DefaultTheme;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    internal class PluginContainer
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
        /// プラグイン一覧。
        /// </summary>
        public IReadOnlyCollection<IPlugin> Plugins => PluginsImpl;

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
            var currentPlugin = pluginStateItems.FirstOrDefault(i => string.Equals(pluginBaseName, i.Name, StringComparison.InvariantCultureIgnoreCase));
            if(currentPlugin != null) {
                if(currentPlugin.State == PluginState.Disable) {
                    Logger.LogInformation("(名前判定)プラグイン読み込み停止中: {0}, {1}", currentPlugin.Name, currentPlugin.PluginId);
                    return new PluginLoadStateData(currentPlugin.PluginId, currentPlugin.Name, new Version(), PluginState.Disable, null, null);
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
                return new PluginLoadStateData(currentPlugin?.PluginId ?? Guid.Empty, currentPlugin?.Name ?? pluginFile.Name, new Version(), PluginState.IllegalAssembly, new WeakReference<PluginAssemblyLoadContext>(loadContext), null);
            }

            Type? pluginInterfaceImpl = null;
            try {
                // 型情報が変な場合、例外が投げられるがそいつはなんかもう解放できなくなる
                var pluginTypes = pluginAssembly.GetTypes().Where(i => !(i.IsAbstract || i.IsNotPublic));
                foreach(var pluginType in pluginTypes) {
                    //if(pluginType.IsAbstract || pluginType.IsNotPublic) {
                    //    continue;
                    //}
                    Logger.LogInformation("{0}", pluginType.FullName);

                    var typeInterfaces = pluginType.GetInterfaces();
                    foreach(var typeInterface in typeInterfaces) {
                        Logger.LogInformation("> {0}", typeInterface.FullName);
                    }
                    var plugins = typeInterfaces.FirstOrDefault(i => i == typeof(IPlugin));
                    if(plugins != null) {
                        pluginInterfaceImpl = pluginType;
                        break;
                    }
                }

                // プラグインIFがバージョンにより取得できなかった場合はIFを名前から取得する
                foreach(var pluginType in pluginTypes) {
                    var typeInterfaces = pluginType.GetInterfaces();
                    var plugins = typeInterfaces.FirstOrDefault(i => i.FullName == typeof(IPlugin).FullName);
                    if(plugins != null) {
                        Logger.LogInformation("めっちゃくちゃ");
                        pluginInterfaceImpl = pluginType;
                        break;
                    }
                }
            } catch(Exception ex) {
                Logger.LogError(ex, "プラグインアセンブリ リフレクション失敗: {0}", pluginFile.Name);
                loadContext.Unload();
                return new PluginLoadStateData(currentPlugin?.PluginId ?? Guid.Empty, currentPlugin?.Name ?? pluginFile.Name, new Version(), PluginState.IllegalAssembly, new WeakReference<PluginAssemblyLoadContext>(loadContext), null);
            }

            if(pluginInterfaceImpl == null) {
                Logger.LogError("プラグインアセンブリからプラグインインターフェイス取得できず: {0}, {1}", pluginAssembly.FullName, pluginFile.FullName);
                loadContext.Unload();
                return new PluginLoadStateData(currentPlugin?.PluginId ?? Guid.Empty, currentPlugin?.Name ?? pluginFile.Name, new Version(), PluginState.IllegalAssembly, new WeakReference<PluginAssemblyLoadContext>(loadContext), null);
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
                return new PluginLoadStateData(currentPlugin?.PluginId ?? Guid.Empty, currentPlugin?.Name ?? pluginFile.Name, new Version(), PluginState.IllegalAssembly, new WeakReference<PluginAssemblyLoadContext>(loadContext), null);
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
                    Logger.LogInformation("(ID判定)プラグイン読み込み停止中: {0}({1}), {2}", loadedCurrentPlugin.Name, pluginName, loadedCurrentPlugin.PluginId);
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
    }
}
