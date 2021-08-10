using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public class UpdateChecker
    {
        public UpdateChecker(ApplicationConfiguration applicationConfiguration, IUserAgentManager userAgentManager, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            ApplicationConfiguration = applicationConfiguration;
            UserAgentManager = userAgentManager;
        }

        #region property

        /// <inheritdoc cref="ILogger"/>
        ILogger Logger { get; }
        ILoggerFactory LoggerFactory { get; }

        ApplicationConfiguration ApplicationConfiguration { get; }
        IUserAgentManager UserAgentManager { get; }

        #endregion

        #region function

        async Task<UpdateData?> RequestUpdateDataAsync(Uri uri)
        {
            using var agent = UserAgentManager.CreateUserAgent();
            try {
                var response = await agent.GetAsync(uri, CancellationToken.None);
                if(!response.IsSuccessStatusCode) {
                    Logger.LogWarning("GetAsync: {0}, {1}", response.StatusCode, uri);
                    return null;
                }
                var content = await response.Content.ReadAsStringAsync();

                //TODO: Serializer.cs に統合したい
                var updateData = System.Text.Json.JsonSerializer.Deserialize<UpdateData>(content);
                if(updateData == null) {
                    Logger.LogError("復元失敗: {0}", content);
                    return null;
                }

                return updateData;
            } catch(Exception ex) {
                Logger.LogWarning(ex, ex.Message);
            }

            return null;
        }

        /// <summary>
        /// アプリケーションの新バージョン確認。
        /// </summary>
        /// <returns>新バージョンがあれば新情報。なければ<c>null</c>。</returns>
        public async Task<UpdateItemData?> CheckApplicationNewVersionAsync(CancellationToken token)
        {

            var uri = new Uri(
                TextUtility.ReplaceFromDictionary(
                    ApplicationConfiguration.General.UpdateCheckUri.OriginalString,
                    new Dictionary<string, string>() {
                        ["CACHE-CLEAR"] = DateTime.UtcNow.ToBinary().ToString()
                    }
                )
            );

            using var agent = UserAgentManager.CreateUserAgent();
            try {
                var response = await agent.GetAsync(uri, CancellationToken.None);
                if(!response.IsSuccessStatusCode) {
                    Logger.LogWarning("GetAsync: {0}, {1}", response.StatusCode, uri);
                    return null;
                }
                var content = await response.Content.ReadAsStringAsync();

                //TODO: Serializer.cs に統合したい
                var updateData = System.Text.Json.JsonSerializer.Deserialize<UpdateData>(content);
                if(updateData == null) {
                    Logger.LogError("復元失敗: {0}", content);
                    return null;
                }
                var result = updateData.Items
                    .Where(i => i.Platform == ProcessArchitecture.ApplicationArchitecture)
                    .Where(i => i.MinimumVersion <= BuildStatus.Version)
                    .Where(i => BuildStatus.Version < i.Version)
                    .OrderByDescending(i => i.Version)
                    .FirstOrDefault()
                ;

                //#if DEBUG
                //                result = updateData.Items.First();
                //#endif

                return result;
            } catch(Exception ex) {
                Logger.LogWarning(ex, ex.Message);
            }
            return null;
        }

        public Task<UpdateItemData?> CheckApplicationNewVersionAsync() => CheckApplicationNewVersionAsync(CancellationToken.None);

        Uri? BuildPluginUri(string baseUrl, IPlugin plugin)
        {
            if(string.IsNullOrWhiteSpace(baseUrl)) {
                return null;
            }

            var versionConverter = new VersionConverter();

            var map = new Dictionary<string, string>() {
                ["TIMESTAMP-NUMBER"] = DateTime.UtcNow.ToBinary().ToString(),
                ["APP-VERSION"] = versionConverter.ConvertNormalVersion(BuildStatus.Version),
                ["APP-REVISION"] = BuildStatus.Revision,
                ["PLUGIN-ID"] = plugin.PluginInformations.PluginIdentifiers.PluginId.ToString(),
                ["PLUGIN-VERSION"] = versionConverter.ConvertNormalVersion(plugin.PluginInformations.PluginVersions.PluginVersion),
            };

            var replaced = TextUtility.ReplaceFromDictionary(baseUrl, map);
            if(Uri.TryCreate(replaced, UriKind.RelativeOrAbsolute, out var uri)) {
                return uri;
            }

            return null;
        }

        /// <summary>
        /// プラグインの新バージョン確認。
        /// </summary>
        /// <param name="plugin">プラグイン。</param>
        /// <returns>新バージョンがあれば新情報。なければ<c>null</c>。</returns>
        public async Task<UpdateItemData?> CheckPluginNewVersionAsync(IPlugin plugin, IDatabaseContexts contexts, IDatabaseStatementLoader statementLoader)
        {
            var pluginId = plugin.PluginInformations.PluginIdentifiers.PluginId;
            Debug.Assert(pluginId != ContentTypeTextNet.Pe.Plugins.DefaultTheme.DefaultTheme.Informations.PluginIdentifiers.PluginId);

            var pluginVersionChecksEntityDao = new PluginVersionChecksEntityDao(contexts.Context, statementLoader, contexts.Implementation, LoggerFactory);
            var urls = pluginVersionChecksEntityDao.SelectPluginVersionCheckUrls(pluginId);

            var pluginVersion = plugin.PluginInformations.PluginVersions.PluginVersion;

            foreach(var url in urls) {
                var uri = BuildPluginUri(url, plugin);
                if(uri is null) {
                    continue;
                }

                var updateData = await RequestUpdateDataAsync(uri);
                if(updateData == null) {
                    continue;
                }

                var result = updateData.Items
                    .Where(i => i.Platform == ProcessArchitecture.ApplicationArchitecture)
                    .Where(i => i.MinimumVersion <= BuildStatus.Version)
                    .Where(i => pluginVersion < i.Version)
                    .OrderByDescending(i => i.Version)
                    .FirstOrDefault()
                ;

                return result;
            }

            // ここから #706 予定

            return null;
        }

        #endregion
    }
}
