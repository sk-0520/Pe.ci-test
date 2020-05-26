using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Addon;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Theme;
using ContentTypeTextNet.Pe.Plugins.DefaultTheme;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    public class PluginContainer
    {
        public PluginContainer(AddonContainer addonContainer, ThemeContainer themeContainer, ILoggerFactory loggerFactory)
        {
            Addon = addonContainer;
            Theme = themeContainer;
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
        }

        #region property

        ILogger Logger { get; }
        ILoggerFactory LoggerFactory { get; }

        ISet<IPlugin> Plugins { get; } = new HashSet<IPlugin>();

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
                foreach(var extension in extensions) {
                    var pluginName = PathUtility.AppendExtension(pluginDir.Name, extension);
                    var pluginPath = Path.Combine(pluginDir.FullName, pluginName);
                    bool existsPlugin;
                    try {
                        existsPlugin = File.Exists(pluginPath);
                    } catch(Exception ex) {
                        Logger.LogError(ex, "プラグイン実ファイル取得失敗: {0}, {1}", ex.Message, pluginPath);
                        continue;
                    }

                    if(existsPlugin) {
                        yield return new FileInfo(pluginPath);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// プラグインの実体一覧を取得。
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IPlugin> GetPlugins()
        {
            yield return new DefaultTheme();
        }

        /// <summary>
        /// プラグインの実体をコンテナに取り込み。
        /// </summary>
        /// <param name="plugin"></param>
        public void AddPlugin(IPlugin plugin)
        {
            Plugins.Add(plugin);

            if(plugin is ITheme theme) {
                Theme.Add(theme);
            }
        }

        #endregion
    }
}
