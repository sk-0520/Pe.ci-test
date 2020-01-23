using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public class UpdateChecker
    {
        public UpdateChecker(Configuration configuration, IUserAgentManager userAgentManager, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            Configuration = configuration;
            UserAgentManager = userAgentManager;
        }
        #region property

        ILogger Logger { get; }

        Configuration Configuration { get; }
        IUserAgentManager UserAgentManager { get; }

        #endregion

        #region function

        public async Task<UpdateItemData?> CheckApplicationUpdateAsync()
        {
            var uri = Configuration.General.UpdateCheckUri;

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
                var result = updateData.Items
                    .Where(i => i.MinimumVersion <= BuildStatus.Version)
                    .OrderByDescending(i => i.Version)
                    .FirstOrDefault()
                ;

                return result;
            } catch(Exception ex) {
                Logger.LogWarning(ex, ex.Message);
            }
            return null;
        }

        #endregion
    }
}
