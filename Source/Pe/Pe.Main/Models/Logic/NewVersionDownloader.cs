using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    /// <summary>
    /// アップデートアーカイブのダウンロード処理担当。
    /// </summary>
    public class NewVersionDownloader
    {
        public NewVersionDownloader(ApplicationConfiguration applicationConfiguration, IUserAgentManager userAgentManager, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            ApplicationConfiguration = applicationConfiguration;
            UserAgentManager = userAgentManager;
        }

        #region property

        private ILogger Logger { get; }

        private ApplicationConfiguration ApplicationConfiguration { get; }
        private IUserAgentManager UserAgentManager { get; }

        #endregion

        #region function

        private string ToCompareValue(string s)
        {
            return s
                .Trim()
                .ToLowerInvariant()
                .Replace("-", string.Empty)
                .Replace("_", string.Empty)
            ;
        }

        /// <summary>
        /// チェックサム処理。
        /// </summary>
        /// <param name="updateItem"></param>
        /// <param name="targetFile"></param>
        /// <param name="userNotifyProgress"></param>
        /// <returns>[非同期] 真: チェックサムOK。</returns>
        public async Task<bool> ChecksumAsync(IReadOnlyNewVersionItemData updateItem, FileInfo targetFile, UserNotifyProgress userNotifyProgress)
        {
            await Task.Delay(0);
            userNotifyProgress.Start();

            if(!targetFile.Exists) {
                Logger.LogWarning("検査ファイルが存在しない: {0}", targetFile);
                return false;
            }

            if(targetFile.Length != updateItem.ArchiveSize) {
                Logger.LogWarning("ファイルサイズが異なる: ファイル {0}, 定義 {1}", targetFile.Length, updateItem.ArchiveSize);
                return false;
            }
            
            Logger.LogInformation("ハッシュ: {0}, {1}", updateItem.ArchiveHashKind, updateItem.ArchiveHashValue);
            using(var hashAlgorithm = HashUtility.Create(updateItem.ArchiveHashKind)) {
                if(hashAlgorithm == null) {
                    Logger.LogError("ハッシュ不明: {0}", updateItem.ArchiveHashKind);
                    return false;
                }

                using var stream = targetFile.OpenRead();

                var buffer = new byte[1024 * 4];
                long totalReadSize = 0;
                while(true) {
                    var readSize = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if(readSize == 0) {
                        break;
                    }
                    hashAlgorithm.TransformBlock(buffer, 0, readSize, buffer, 0);
                    totalReadSize += readSize;
                    userNotifyProgress.Report(totalReadSize / (double)updateItem.ArchiveSize, string.Empty);
                }
                hashAlgorithm.TransformFinalBlock(buffer, 0, 0);
                var hash = ToCompareValue(BitConverter.ToString(hashAlgorithm.Hash!));

                Logger.LogInformation("算出ハッシュ: {0}", hash);
                userNotifyProgress.Report(1, hash);

                userNotifyProgress.End();


                return hash == ToCompareValue(updateItem.ArchiveHashValue);
            }
        }

        /// <summary>
        /// アーカイブのダウンロード。
        /// </summary>
        /// <param name="updateItem"></param>
        /// <param name="donwloadFile"></param>
        /// <param name="userNotifyProgress"></param>
        /// <returns></returns>
        public async Task DownloadArchiveAsync(NewVersionItemData updateItem, FileInfo donwloadFile, UserNotifyProgress userNotifyProgress)
        {
            Logger.LogInformation("アップデートファイルダウンロード: {0}, {1}", updateItem.ArchiveUri, donwloadFile);
            userNotifyProgress.Start();

            using(var userAgent = UserAgentManager.CreateAppHttpUserAgent()) {
                var content = await userAgent.GetAsync(updateItem.ArchiveUri);

                //NOTE: long が使えない！
                int totalDownloadedSize = 0;
                const int downloadChunkSize = 1024 * 4;
                var sizePerTime = new SizePerTime(TimeSpan.FromSeconds(1));

                sizePerTime.Start();

                using(var networkStream = await content.Content.ReadAsStreamAsync()) {
                    using(var localStream = donwloadFile.Create()) {
                        var downloadChunk = new byte[downloadChunkSize];
                        var sizeConverter = new SizeConverter();
                        var units = new[] {
                            Properties.Resources.String_Download_Seconds_Byte,
                            Properties.Resources.String_Download_Seconds_KB,
                            Properties.Resources.String_Download_Seconds_MB,
                            Properties.Resources.String_Download_Seconds_GB,
                        };
                        var format = Properties.Resources.String_Download_Seconds_Format_DOTNET;
                        while(true) {
                            var donwloadSize = await networkStream.ReadAsync(downloadChunk, 0, downloadChunk.Length);
                            if(0 < donwloadSize) {
                                await localStream.WriteAsync(downloadChunk, 0, donwloadSize);
                                totalDownloadedSize += donwloadSize;
                                sizePerTime.Add(donwloadSize);
                                var size = sizeConverter.ConvertHumanReadableByte(sizePerTime.Size, format, units);
                                userNotifyProgress.Report(totalDownloadedSize / (double)updateItem.ArchiveSize, size);
                            } else {
                                userNotifyProgress.End();
                                break;
                            }
                        }
                    }
                }
            }
        }

        #endregion

    }
}
