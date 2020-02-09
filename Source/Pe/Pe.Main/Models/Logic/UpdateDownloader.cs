using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public class UpdateDownloader
    {
        public UpdateDownloader(CustomConfiguration configuration, IUserAgentManager userAgentManager, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            Configuration = configuration;
            UserAgentManager = userAgentManager;
        }

        #region property

        ILogger Logger { get; }

        CustomConfiguration Configuration { get; }
        IUserAgentManager UserAgentManager { get; }

        #endregion

        #region function

        public async Task<bool> DownloadApplicationArchiveAsync(UpdateItemData updateItem, FileInfo donwloadFile)
        {
            try {
                Logger.LogInformation("アップデートファイルダウンロード: {0}, {1}", updateItem.ArchiveUri, donwloadFile);
                using(var userAgent = UserAgentManager.CreateAppUserAgent()) {
                    var content = await userAgent.GetAsync(updateItem.ArchiveUri);
                    if(!content.IsSuccessStatusCode) {
                        // まぁ来ないと思うよ
                        return false;
                    }

                    //TODO: ダウンロード進捗のあれこれ
                    using(var networkStream = await content.Content.ReadAsStreamAsync()) {
                        using(var localStream = donwloadFile.Create()) {
                            await networkStream.CopyToAsync(localStream);
                        }
                        return true;
                    }
                }
            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
            }

            return false;
        }

        #endregion

    }
}
