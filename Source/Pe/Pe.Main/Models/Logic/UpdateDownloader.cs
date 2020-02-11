using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
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

        public async Task DownloadApplicationArchiveAsync(UpdateItemData updateItem, FileInfo donwloadFile)
        {
            Logger.LogInformation("アップデートファイルダウンロード: {0}, {1}", updateItem.ArchiveUri, donwloadFile);
            using(var userAgent = UserAgentManager.CreateAppUserAgent()) {
                var content = await userAgent.GetAsync(updateItem.ArchiveUri);

                //TODO: ダウンロード進捗のあれこれ
                using(var networkStream = await content.Content.ReadAsStreamAsync()) {
                    using(var localStream = donwloadFile.Create()) {
                        await networkStream.CopyToAsync(localStream);
                    }
                }
            }
        }

        public async Task<ReleaseNoteItemData> DownloadReleaseNoteAsync(UpdateItemData updateItem)
        {
            Logger.LogInformation("リリースノートダウンロード: {0}, {1}", updateItem.NoteMime, updateItem.NoteUri);
            using(var userAgent = UserAgentManager.CreateAppUserAgent()) {
                using(var networkStream = await userAgent.GetStreamAsync(updateItem.NoteUri)) {
                    //using var memoryStream = new MemoryStream();
                    //await networkStream.CopyToAsync(memoryStream);
                    SerializerBase serializer = updateItem.NoteMime switch
                    {
                        ReleaseNoteMime.Json => new JsonDataSerializer(),
                        _ => throw new NotImplementedException()
                    };
                    //memoryStream.Position = 0;
                    return serializer.Load<ReleaseNoteItemData>(networkStream);
                }
            }
        }

        #endregion

        }
}
