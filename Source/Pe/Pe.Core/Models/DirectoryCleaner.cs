using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public class DirectoryCleaner
    {
        public DirectoryCleaner(DirectoryInfo directory, int waitCount, TimeSpan waitTime, ILoggerFactory loggerFactory)
        {
            Directory = directory;
            WaitCount = waitCount;
            WaitTime = waitTime;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        /// <summary>
        /// 対象ディレクトリ。
        /// </summary>
        DirectoryInfo Directory { get; }
        /// <summary>
        /// 待機回数。
        /// </summary>
        int WaitCount { get; }
        /// <summary>
        /// 一回の待機に対する待ち時間。
        /// </summary>
        TimeSpan WaitTime { get; }
        ILogger Logger { get; }
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
                    file.Attributes = file.Attributes & ~FileAttributes.ReadOnly;
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
            var counter = new Counter(WaitCount);
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
