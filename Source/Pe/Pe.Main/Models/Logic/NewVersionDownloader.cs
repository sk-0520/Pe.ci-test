using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Bridge.Models;
using System.Threading;

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
        internal int ChecksumSize { get; init; } = 1024 * 2;
        internal int DownloadChunkSize { get; init; } = 1024 * 4;

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
        public async Task<bool> ChecksumAsync(IReadOnlyNewVersionItemData updateItem, FileInfo targetFile, UserNotifyProgress userNotifyProgress, CancellationToken cancellationToken)
        {
            await Task.Delay(0, cancellationToken);
            userNotifyProgress.Start();

            if(!targetFile.Exists) {
                Logger.LogWarning("検査ファイルが存在しない: {File}", targetFile);
                return false;
            }

            if(targetFile.Length != updateItem.ArchiveSize) {
                Logger.LogWarning("ファイルサイズが異なる: ファイル {0}, 定義 {1}", targetFile.Length, updateItem.ArchiveSize);
                return false;
            }

            Logger.LogInformation("ハッシュ: {0}, {1}", updateItem.ArchiveHashKind, updateItem.ArchiveHashValue);
            using(var hashAlgorithm = HashUtility.Create(updateItem.ArchiveHashKind)) {
                using var stream = targetFile.OpenRead();
                using var checkSumBuffer = new ArrayPoolObject<byte>(ChecksumSize);
                long totalReadSize = 0;
                while(true) {
                    var readSize = await stream.ReadAsync(checkSumBuffer.Items, 0, checkSumBuffer.Items.Length, cancellationToken);
                    if(readSize == 0) {
                        break;
                    }
                    hashAlgorithm.TransformBlock(checkSumBuffer.Items, 0, readSize, checkSumBuffer.Items, 0);
                    totalReadSize += readSize;
                    userNotifyProgress.Report(totalReadSize / (double)updateItem.ArchiveSize, string.Empty);
                }
                hashAlgorithm.TransformFinalBlock(checkSumBuffer.Items, 0, 0);
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
        /// <param name="downloadFile"></param>
        /// <param name="userNotifyProgress"></param>
        /// <returns></returns>
        public async Task DownloadArchiveAsync(NewVersionItemData updateItem, FileInfo downloadFile, UserNotifyProgress userNotifyProgress, CancellationToken cancellationToken)
        {
            Logger.LogInformation("アップデートファイルダウンロード: {0}, {1}", updateItem.ArchiveUri, downloadFile);
            userNotifyProgress.Start();

            using(var userAgent = UserAgentManager.CreateAppHttpUserAgent()) {
                var content = await userAgent.GetAsync(updateItem.ArchiveUri, cancellationToken);

                //NOTE: long が使えない！
                int totalDownloadedSize = 0;
                var sizePerTime = new SizePerTime(TimeSpan.FromSeconds(1));

                sizePerTime.Start();

                using(var networkStream = await content.Content.ReadAsStreamAsync(cancellationToken)) {
                    using var downloadChunkBuffer = new ArrayPoolObject<byte>(DownloadChunkSize);
                    using var localStream = downloadFile.Create();
                    var sizeConverter = new SizeConverter();
                    var units = new[] {
                            Properties.Resources.String_Download_Seconds_Byte,
                            Properties.Resources.String_Download_Seconds_KB,
                            Properties.Resources.String_Download_Seconds_MB,
                            Properties.Resources.String_Download_Seconds_GB,
                        };
                    var format = Properties.Resources.String_Download_Seconds_Format_DOTNET;
                    while(true) {
                        var downloadSize = await networkStream.ReadAsync(downloadChunkBuffer.Items, 0, downloadChunkBuffer.Length, cancellationToken);
                        if(0 < downloadSize) {
                            await localStream.WriteAsync(downloadChunkBuffer.Items, 0, downloadSize, cancellationToken);
                            totalDownloadedSize += downloadSize;
                            sizePerTime.Add(downloadSize);
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

        #endregion

    }
}
