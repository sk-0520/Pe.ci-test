using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public class UpdateChecker
    {
        public UpdateChecker(ApplicationConfiguration applicationConfiguration, IUserAgentManager userAgentManager, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            ApplicationConfiguration = applicationConfiguration;
            UserAgentManager = userAgentManager;
        }

        #region property

        ILogger Logger { get; }

        ApplicationConfiguration ApplicationConfiguration { get; }
        IUserAgentManager UserAgentManager { get; }

        #endregion

        #region function

        public async Task<UpdateItemData?> CheckApplicationUpdateAsync()
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

#if DEBUG
                result = updateData.Items.First();
#endif

                return result;
            } catch(Exception ex) {
                Logger.LogWarning(ex, ex.Message);
            }
            return null;
        }

        #endregion
    }
}
