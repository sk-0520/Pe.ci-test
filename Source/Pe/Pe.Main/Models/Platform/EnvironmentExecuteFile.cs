using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Platform
{
    public class EnvironmentPathExecuteItem
    {
        public EnvironmentPathExecuteItem(DirectoryInfo directory, FileInfo file)
        {
            Directory = directory;
            File = file;
        }

        #region property

        public DirectoryInfo Directory { get; }
        public FileInfo File { get; }

        #endregion
    }

    public class EnvironmentExecuteFile
    {
        public EnvironmentExecuteFile(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        ILogger Logger { get; }

        #endregion

        #region function

        /// <summary>
        /// 実行形式一覧を取得。
        /// </summary>
        /// <returns>*.ext の配列(<paramref name="addWildcard"/>が真の場合。偽なら * はつかない)</returns>
        public IReadOnlyList<string> GetSystemExecuteExtensions(bool addWildcard)
        {
            var dotExeExts = Environment.GetEnvironmentVariable("PATHEXT");
            if(!string.IsNullOrEmpty(dotExeExts)) {
                var result = dotExeExts
                    .Split(';')
                    .Where(i => ".X".Length <= i.Length)
                    .Where(i => i[0] == '.')
                    .Select(i => i.Trim().ToLower())
                    .OrderBy(i => i == ".exe" ? 0 : 1)
                    .ThenBy(i => i)
                    .Select(i => addWildcard ? "*" + i: i)
                    .ToList()
                ;
                if(result.Any()) {
                    // EXEが無いって事はないだろうけど実行できないしもうどうでもいい
                    return result;
                }
            }

            return new[] { "*.exe", "*.bat", "*.com" };
        }

        public IReadOnlyList<EnvironmentPathExecuteItem> GetPathExecuteFiles()
        {
            var path = Environment.GetEnvironmentVariable("PATH");
            if(string.IsNullOrWhiteSpace(path)) {
                return new List<EnvironmentPathExecuteItem>();
            }

            var rawExts = GetSystemExecuteExtensions(false)
                .Select(i => i.Substring(1))
                .Select(i => $"({Regex.Escape(i)})")
            ;

            var extRegex = new Regex(@".*\." + string.Join("|", rawExts) + "$");
            var dirPaths = path.Split(';');
            var result = new List<EnvironmentPathExecuteItem>();
            foreach(var dirPath in dirPaths) {
                try {
                    var dir = new DirectoryInfo(dirPath);
                    dir.Refresh();
                    if(!dir.Exists) {
                        Logger.LogInformation("skip dir: {0}", dir.FullName);
                        continue;
                    }
                    IEnumerable<FileInfo> files = dir.EnumerateFiles("*", SearchOption.TopDirectoryOnly);
                    foreach(var file in files) {
                        if(extRegex.IsMatch(file.Name)) {
                            var item = new EnvironmentPathExecuteItem(dir, file);
                            result.Add(item);
                        }
                    }
                } catch(Exception ex) {
                    Logger.LogWarning(ex, ex.Message);
                }
            }

            return result;
        }

        /// <summary>
        /// 指定したファイルが実行可能ファイルとして存在する場合に取得。
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public EnvironmentPathExecuteItem? Get(string? fileName, IReadOnlyCollection<EnvironmentPathExecuteItem> items)
        {
            if(string.IsNullOrWhiteSpace(fileName)) {
                return null;
            }

            var name = Path.GetFileNameWithoutExtension(fileName);
            return items.FirstOrDefault(i => string.Equals(name, Path.GetFileNameWithoutExtension(i.File.Name), StringComparison.OrdinalIgnoreCase));
        }

        #endregion
    }

    public class EnvironmentPathExecuteFileCache
    {
        public EnvironmentPathExecuteFileCache(TimeSpan cacheTime)
        {
            CacheTime = cacheTime;
        }

        #region property

        public static EnvironmentPathExecuteFileCache Instance { get; } = new EnvironmentPathExecuteFileCache(TimeSpan.FromHours(3));

        /// <summary>
        /// 次に %PATH% を検索するまでの時間。
        /// <para><see cref="LastSearch"/>に加算。</para>
        /// </summary>
        private TimeSpan CacheTime { get; }
        /// <summary>
        /// 最後に %PATH% を検索した時間。
        /// </summary>
        private DateTime LastSearch { get; set; } = DateTime.MinValue;
        private List<EnvironmentPathExecuteItem> PathItemsCache { get; } = new List<EnvironmentPathExecuteItem>();

        #endregion

        #region function

        public IReadOnlyList<EnvironmentPathExecuteItem> GetItems(ILoggerFactory loggerFactory)
        {
            if(LastSearch + CacheTime < DateTime.Now) {
                LastSearch = DateTime.Now;
                var environmentExecuteFile = new EnvironmentExecuteFile(loggerFactory);
                var pathItems = environmentExecuteFile.GetPathExecuteFiles();
                PathItemsCache.AddRange(pathItems);
            }

            return PathItemsCache;
        }
        #endregion
    }
}
