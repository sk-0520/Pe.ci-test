using System;
using System.IO;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Library.Base
{
    /// <summary>
    /// ディレクトリクリーンアップ処理。
    /// </summary>
    /// <remarks>
    /// <para>指定ディレクトリ以下のファイルを再帰的に削除する。</para>
    /// </remarks>
    public class DirectoryCleaner
    {
        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="directory">対象ディレクトリ。</param>
        /// <param name="retryCount">ディレクトリ作成失敗時の再試行回数。</param>
        /// <param name="waitTime">ディレクトリ作成失敗時の再試行前に待機する時間。</param>
        /// <param name="loggerFactory"></param>
        public DirectoryCleaner(DirectoryInfo directory, int retryCount, TimeSpan waitTime, ILoggerFactory loggerFactory)
        {
            Directory = directory;
            if(retryCount < 1) {
                throw new ArgumentException(null, nameof(retryCount));
            }
            RetryCount = retryCount;
            if(waitTime <= TimeSpan.Zero) {
                throw new ArgumentException(null, nameof(waitTime));
            }
            WaitTime = waitTime;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        /// <summary>
        /// 対象ディレクトリ。
        /// </summary>
        public DirectoryInfo Directory { get; }
        /// <summary>
        /// 待機回数。
        /// </summary>
        private int RetryCount { get; }
        /// <summary>
        /// 一回の待機に対する待ち時間。
        /// </summary>
        private TimeSpan WaitTime { get; }
        private ILogger Logger { get; }

        #endregion

        #region function

        private bool DeleteDirectory(DirectoryInfo directory, bool directoryIsDelete = true)
        {
            if(!directory.Exists) {
                return false;
            }
            var files = directory.GetFiles();
            foreach(var file in files) {
                if(file.Attributes.HasFlag(FileAttributes.ReadOnly)) {
                    file.Attributes &= ~FileAttributes.ReadOnly;
                }
                file.Delete();
            }
            var dirs = directory.GetDirectories();
            foreach(var dir in dirs) {
                DeleteDirectory(dir, true);
            }

            if(directoryIsDelete) {
                directory.Delete(true);
            }

            return true;
        }

        private void CreateDirectory()
        {
            var counter = new Counter(RetryCount);
            foreach(var count in counter) {
                Directory.Create();
                Directory.Refresh();
                if(Directory.Exists) {
                    break;
                } else if(count.IsLast) {
                    Logger.LogError("ディレクトリ作成に失敗: {0}", Directory);
                    return;
                }
                Logger.LogInformation("ディレクトリ作成待機中: {0}/{1} {2}", count.CurrentCount, count.MaxCount, WaitTime);
                Thread.Sleep(WaitTime);
            }
        }

        /// <summary>
        /// <see cref="Directory"/>以下を綺麗にする。
        /// </summary>
        /// <param name="rootDelete"><see cref="Directory"/>その物を削除し、再作成するか。</param>
        public void Clear(bool rootDelete = false)
        {
            Directory.Refresh();
            if(Directory.Exists) {
                DeleteDirectory(Directory, rootDelete);
                if(rootDelete) {
                    CreateDirectory();
                }
            } else {
                CreateDirectory();
            }
        }

        #endregion
    }
}
