using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Models
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

        ILogger Logger { get; }

        #endregion

        #region function

        /// <summary>
        /// ディレクトリの移動。
        /// <para>移動できない場合に<see cref="Copy(DirectoryInfo, DirectoryInfo)"/>を行う。</para>
        /// </summary>
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
                }

                return;
            } else {
                Copy(source, destination);
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
