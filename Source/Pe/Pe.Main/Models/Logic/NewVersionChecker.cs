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
using ContentTypeTextNet.Pe.Core.Models.Serialization;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data.ServerApi;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Bridge.Models;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    /// <summary>
    /// 新バージョン確認。
    /// </summary>
    public class NewVersionChecker
    {
        public NewVersionChecker(IApplicationInformation applicationInformation, IUserAgentManager userAgentManager, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            ApplicationInformation = applicationInformation;
            UserAgentManager = userAgentManager;
        }

        #region property

        /// <inheritdoc cref="ILogger"/>
        private ILogger Logger { get; }

        private IApplicationInformation ApplicationInformation { get; }
        private IUserAgentManager UserAgentManager { get; }

        #endregion

        #region function

        public async Task<NewVersionData?> RequestUpdateDataAsync(Uri uri, CancellationToken cancellationToken)
        {
            using var agent = UserAgentManager.CreateUserAgent();
            try {
                var response = await agent.GetAsync(uri, CancellationToken.None);
                if(!response.IsSuccessStatusCode) {
                    Logger.LogWarning("GetAsync: {0}, {1}", response.StatusCode, uri);
                    return null;
                }
                var content = await response.Content.ReadAsStringAsync(cancellationToken);

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
        /// <returns>新バージョンがあれば新情報。なければ<see langword="null" />。</returns>
        public async Task<NewVersionItemData?> CheckApplicationNewVersionAsync(IEnumerable<string> updateCheckUrlItems, CancellationToken cancellationToken)
        {
            using var agent = UserAgentManager.CreateUserAgent();
            foreach(var updateCheckUrl in updateCheckUrlItems) {
                var uri = new Uri(
                    TextUtility.ReplaceFromDictionary(
                        updateCheckUrl,
                        new Dictionary<string, string>() {
                            ["CACHE-CLEAR"] = DateTime.UtcNow.ToBinary().ToString(CultureInfo.InvariantCulture),
                        }
                    )
                );
                Logger.LogInformation("check app update: {Uri}", uri);

                try {
                    var response = await agent.GetAsync(uri, cancellationToken);
                    if(!response.IsSuccessStatusCode) {
                        Logger.LogWarning("GetAsync: {0}, {1}", response.StatusCode, uri);
                        continue;
                    }
                    var content = await response.Content.ReadAsStringAsync(cancellationToken);

                    //TODO: Serializer.cs に統合したい
                    var updateData = System.Text.Json.JsonSerializer.Deserialize<NewVersionData>(content);
                    if(updateData == null) {
                        Logger.LogError("復元失敗: {0}", content);
                        return null;
                    }
                    var result = updateData.Items
                        .Where(i => i.Platform == ApplicationInformation.Architecture)
                        .Where(i => i.MinimumVersion <= ApplicationInformation.Version)
                        .Where(i => ApplicationInformation.Version < i.Version)
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

        /// <summary>
        /// プラグイン用URIの構築。
        /// </summary>
        /// <param name="baseUrl">元URL。</param>
        /// <param name="pluginId">プラグインID。</param>
        /// <param name="pluginVersion">プラグインバージョン。</param>
        /// <returns>構築したURI。構築できなかった場合は<see langword="null" /></returns>
        protected internal Uri? BuildPluginUri(string baseUrl, PluginId pluginId, Version pluginVersion)
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
            if(Uri.TryCreate(replaced, UriKind.Absolute, out var uri)) {
                return uri;
            }

            return null;
        }

        public NewVersionItemData? GetPluginNewVersionItem(Version pluginVersion, IEnumerable<NewVersionItemData> items)
        {
            return items
                .Where(i => i.Platform == ApplicationInformation.Architecture)
                .Where(i => i.MinimumVersion <= ApplicationInformation.Version)
                .Where(i => pluginVersion < i.Version)
                .OrderByDescending(i => i.Version)
                .FirstOrDefault()
            ;
        }

        /// <summary>
        /// プラグインの新バージョン確認。
        /// </summary>
        /// <param name="plugin">プラグイン。</param>
        /// <returns>新バージョンがあれば新情報。なければ<see langword="null" />。</returns>
        public async Task<NewVersionItemData?> CheckPluginNewVersionAsync(Uri apiServerPluginInformation, PluginId pluginId, Version pluginVersion, IEnumerable<string> urls, CancellationToken cancellationToken)
        {
            Debug.Assert(pluginId != ContentTypeTextNet.Pe.Plugins.DefaultTheme.DefaultTheme.Information.PluginIdentifiers.PluginId);

            foreach(var url in urls) {
                var uri = BuildPluginUri(url, pluginId, pluginVersion);
                if(uri is null) {
                    continue;
                }

                var updateData = await RequestUpdateDataAsync(uri, cancellationToken);
                if(updateData == null) {
                    continue;
                }

                var result = GetPluginNewVersionItem(pluginVersion, updateData.Items);
                return result;
            }

            var apiResult = await CheckPluginNewVersionByApiAsync(apiServerPluginInformation, pluginId, pluginVersion, cancellationToken);

            return apiResult;
        }

        public async Task<PluginInformationItemData?> GetPluginVersionInfoByApiAsync(Uri apiServerPluginInformation, PluginId pluginId, CancellationToken cancellationToken)
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
            var response = await agent.PostAsync(apiServerPluginInformation, content, cancellationToken);

            if(!response.IsSuccessStatusCode) {
                return null;
            }

            using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var serializer = new JsonTextSerializer();
            var result = serializer.Load<ServerApiResultData<PluginInformationResultData>>(stream);
            if(result.Data is not null && result.Data.Plugins.TryGetValue(pluginId.Id, out var item)) {
                return item;
            }

            return null;
        }

        private async Task<NewVersionItemData?> CheckPluginNewVersionByApiAsync(Uri apiServerPluginInformation, PluginId pluginId, Version pluginVersion, CancellationToken cancellationToken)
        {
            var item = await GetPluginVersionInfoByApiAsync(apiServerPluginInformation, pluginId, cancellationToken);
            if(item is null) {
                return null;
            }

            var uri = new Uri(item.CheckUrl);
            var updateData = await RequestUpdateDataAsync(uri, cancellationToken);
            if(updateData is null) {
                return null;
            }

            return GetPluginNewVersionItem(pluginVersion, updateData.Items);
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
                _ => kind,
            };
        }

        #endregion
    }
}
