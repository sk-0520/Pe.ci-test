using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.RightsManagement;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data.ServerApi;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Standard.Base;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    /// <summary>
    /// 新バージョン確認。
    /// </summary>
    public class NewVersionChecker
    {
        public NewVersionChecker(ApplicationConfiguration applicationConfiguration, IUserAgentManager userAgentManager, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            ApplicationConfiguration = applicationConfiguration;
            UserAgentManager = userAgentManager;
        }

        #region property

        /// <inheritdoc cref="ILogger"/>
        private ILogger Logger { get; }
        private ILoggerFactory LoggerFactory { get; }

        private ApplicationConfiguration ApplicationConfiguration { get; }
        private IUserAgentManager UserAgentManager { get; }

        #endregion

        #region function

        public async Task<NewVersionData?> RequestUpdateDataAsync(Uri uri)
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
                var updateData = System.Text.Json.JsonSerializer.Deserialize<NewVersionData>(content);
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
        public async Task<NewVersionItemData?> CheckApplicationNewVersionAsync(CancellationToken token)
        {
            using var agent = UserAgentManager.CreateUserAgent();
            foreach(var updateCheckUrl in ApplicationConfiguration.General.UpdateCheckUrlItems) {
                var uri = new Uri(
                    TextUtility.ReplaceFromDictionary(
                        updateCheckUrl,
                        new Dictionary<string, string>() {
                            ["CACHE-CLEAR"] = DateTime.UtcNow.ToBinary().ToString(CultureInfo.InvariantCulture),
                        }
                    )
                );
                Logger.LogInformation("check app update: {uri}", uri);

                try {
                    var response = await agent.GetAsync(uri, token);
                    if(!response.IsSuccessStatusCode) {
                        Logger.LogWarning("GetAsync: {0}, {1}", response.StatusCode, uri);
                        continue;
                    }
                    var content = await response.Content.ReadAsStringAsync(token);

                    //TODO: Serializer.cs に統合したい
                    var updateData = System.Text.Json.JsonSerializer.Deserialize<NewVersionData>(content);
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
            }

            return null;
        }

        public Task<NewVersionItemData?> CheckApplicationNewVersionAsync() => CheckApplicationNewVersionAsync(CancellationToken.None);

        /// <summary>
        /// プラグイン用URIの構築。
        /// </summary>
        /// <param name="baseUrl">元URL。</param>
        /// <param name="pluginId">プラグインID。</param>
        /// <param name="pluginVersion">プラグインバージョン。</param>
        /// <returns>構築したURI。構築できなかった場合は<c>null</c></returns>
        private Uri? BuildPluginUri(string baseUrl, PluginId pluginId, Version pluginVersion)
        {
            if(string.IsNullOrWhiteSpace(baseUrl)) {
                return null;
            }

            var versionConverter = new VersionConverter();

            var map = new Dictionary<string, string>() {
                ["TIMESTAMP-NUMBER"] = DateTime.UtcNow.ToBinary().ToString(CultureInfo.InvariantCulture),
                ["APP-VERSION"] = versionConverter.ConvertNormalVersion(BuildStatus.Version),
                ["APP-REVISION"] = BuildStatus.Revision,
                ["PLUGIN-ID"] = pluginId.ToString(),
                ["PLUGIN-VERSION"] = versionConverter.ConvertNormalVersion(pluginVersion),
            };

            var replaced = TextUtility.ReplaceFromDictionary(baseUrl, map);
            if(Uri.TryCreate(replaced, UriKind.RelativeOrAbsolute, out var uri)) {
                return uri;
            }

            return null;
        }

        public static NewVersionItemData? GetNewVersionItem(Version pluginVersion, IEnumerable<NewVersionItemData> items)
        {
            return items
                .Where(i => i.Platform == ProcessArchitecture.ApplicationArchitecture)
                .Where(i => i.MinimumVersion <= BuildStatus.Version)
                .Where(i => pluginVersion < i.Version)
                .OrderByDescending(i => i.Version)
                .FirstOrDefault()
            ;
        }



        /// <summary>
        /// プラグインの新バージョン確認。
        /// </summary>
        /// <param name="plugin">プラグイン。</param>
        /// <returns>新バージョンがあれば新情報。なければ<c>null</c>。</returns>
        public async Task<NewVersionItemData?> CheckPluginNewVersionAsync(PluginId pluginId, Version pluginVersion, IEnumerable<string> urls)
        {
            Debug.Assert(pluginId != ContentTypeTextNet.Pe.Plugins.DefaultTheme.DefaultTheme.Information.PluginIdentifiers.PluginId);

            foreach(var url in urls) {
                var uri = BuildPluginUri(url, pluginId, pluginVersion);
                if(uri is null) {
                    continue;
                }

                var updateData = await RequestUpdateDataAsync(uri);
                if(updateData == null) {
                    continue;
                }

                var result = GetNewVersionItem(pluginVersion, updateData.Items);
                return result;
            }

            var apiResult = await CheckPluginNewVersionByApiAsync(pluginId, pluginVersion);

            return apiResult;
        }

        public async Task<PluginInformationItemData?> GetPluginVersionInfoByApiAsync(PluginId pluginId)
        {
            var json = JsonSerializer.Serialize(new {
                plugin_ids = new[] {
                    pluginId.ToString(),
                },
            });
            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            using var agent = UserAgentManager.CreateUserAgent();
            var response = await agent.PostAsync(ApplicationConfiguration.Api.ServerPluginInformation, content);

            if(!response.IsSuccessStatusCode) {
                return null;
            }

            using var stream = await response.Content.ReadAsStreamAsync();
            var serializer = new JsonTextSerializer();
            var result = serializer.Load<ServerApiResultData<PluginInformationResultData>>(stream);
            if(result.Data is not null && result.Data.Plugins.TryGetValue(pluginId.Id, out var item)) {
                return item;
            }

            return null;
        }

        private async Task<NewVersionItemData?> CheckPluginNewVersionByApiAsync(PluginId pluginId, Version pluginVersion)
        {
            var item = await GetPluginVersionInfoByApiAsync(pluginId);
            if(item is null) {
                return null;
            }

            var uri = new Uri(item.CheckUrl);
            var updateData = await RequestUpdateDataAsync(uri);
            if(updateData is null) {
                return null;
            }

            return GetNewVersionItem(pluginVersion, updateData.Items);
        }

        /// <summary>
        /// アーカイブ種別から拡張子を取得。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetExtension(NewVersionItemData data)
        {
            var kind = (data.ArchiveKind ?? string.Empty).Trim().ToLowerInvariant();

            return kind switch {
                "zip" => "zip",
                "7z" => "7z",
                _ => kind ?? string.Empty,
            };
        }

        #endregion
    }
}
