using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Library.Base
{
    /// <summary>
    /// ディレクトリを適当に移動させる。
    /// </summary>
    public class DirectoryMover
    {
        public DirectoryMover(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        private ILogger Logger { get; }

        #endregion

        #region function

        /// <summary>
        /// ディレクトリの移動。
        /// </summary>
        /// <remarks>
        /// <para>移動できない場合に<see cref="Copy(DirectoryInfo, DirectoryInfo)"/>を行い、処理後削除される。</para>
        /// </remarks>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public void Move(DirectoryInfo source, DirectoryInfo destination)
        {
            destination.Refresh();
            if(!destination.Exists) {
                try {
                    source.MoveTo(destination.FullName);
                } catch(IOException ex) {
                    Logger.LogWarning(ex, ex.Message);
                    Copy(source, destination);
                    try {
                        source.Delete(true);
                    } catch(UnauthorizedAccessException ex2) {
                        Logger.LogError(ex2, ex2.Message);
                    }
                }

                return;
            } else {
                Copy(source, destination);
                source.Delete(true);
            }
        }

        /// <summary>
        /// ディレクトリの複製。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public void Copy(DirectoryInfo source, DirectoryInfo destination)
        {
            var files = source.EnumerateFiles();
            foreach(var file in files) {
                if(!destination.Exists) {
                    destination.Create();
                }
                var destFilePath = Path.Combine(destination.FullName, file.Name);
                file.CopyTo(destFilePath, true);
            }

            var dirs = source.EnumerateDirectories();
            foreach(var dir in dirs) {
                var destDir = Directory.CreateDirectory(Path.Combine(destination.FullName, dir.Name));
                Copy(dir, destDir);
            }
        }

        #endregion
    }
}
