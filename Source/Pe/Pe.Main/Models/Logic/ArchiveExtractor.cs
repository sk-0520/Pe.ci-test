using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public class ArchiveExtractor
    {
        public ArchiveExtractor(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        ILogger Logger { get; }

        #endregion
        #region function

        public void Extract(FileInfo zipArchiveFile, DirectoryInfo extractDirectory)
        {
            var createdDirs = new HashSet<string>();

            // 使い方間違えてんのか知らんけど ZipFile.ExtractToDirectory ってやたら例外吐かん？
            using(var zipArchive = ZipFile.OpenRead(zipArchiveFile.FullName)) {
                foreach(var entry in zipArchive.Entries.Where(e => e.Name.Length > 0)) {
                    var expandPath = Path.Combine(extractDirectory.FullName, entry.FullName);
                    var dirPath = Path.GetDirectoryName(expandPath) ?? string.Empty;
                    if(!createdDirs.Contains(dirPath) && !Directory.Exists(dirPath)) {
                        Logger.LogTrace("作成: {0}", dirPath);
                        Directory.CreateDirectory(dirPath);
                        createdDirs.Add(dirPath);
                    }
                    Logger.LogTrace("展開: {0}", expandPath);
                    entry.ExtractToFile(expandPath, true);
                }
            }
        }

        #endregion
    }
}
