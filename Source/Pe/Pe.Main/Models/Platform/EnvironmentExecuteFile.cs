using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Platform
{
    /// <summary>
    /// 環境変数PATHから抽出したファイルパス。
    /// </summary>
    public class EnvironmentPathExecuteItem
    {
        public EnvironmentPathExecuteItem(DirectoryInfo directory, FileInfo file)
        {
            Directory = directory;
            File = file;
        }

        #region property

        /// <summary>
        /// パスに設定されているディレクトリ。
        /// </summary>
        public DirectoryInfo Directory { get; }
        /// <summary>
        /// パスから取得したファイル。
        /// </summary>
        public FileInfo File { get; }

        #endregion
    }

    /// <summary>
    /// 環境変数PATHの実行ファイル処理。
    /// </summary>
    public class EnvironmentExecuteFile
    {
        public EnvironmentExecuteFile(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        private ILogger Logger { get; }

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
                    .Select(i => i.Trim().ToLowerInvariant())
                    .OrderBy(i => i == ".exe" ? 0 : 1)
                    .ThenBy(i => i)
                    .Select(i => addWildcard ? "*" + i : i)
                    .ToList()
                ;
                if(result.Any()) {
                    // EXEが無いって事はないだろうけど実行できないしもうどうでもいい
                    return result;
                }
            }

            return new[] { "*.exe", "*.bat", "*.com" };
        }

        /// <summary>
        /// 環境変数PATHから実行形式の一覧を取得。
        /// </summary>
        /// <returns></returns>
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
            var dirPaths = path
                .Split(';')
                .Where(i => !string.IsNullOrWhiteSpace(i))
            ;
            var result = new List<EnvironmentPathExecuteItem>();
            foreach(var dirPath in dirPaths) {
                try {
                    var dir = new DirectoryInfo(dirPath);
                    dir.Refresh();
                    if(!dir.Exists) {
                        Logger.LogInformation("存在しない PATH は無視: {0}", dir.FullName);
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
        public EnvironmentPathExecuteItem? Get(string fileName, IReadOnlyCollection<EnvironmentPathExecuteItem> items)
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
            if(LastSearch + CacheTime < DateTime.UtcNow) {
                LastSearch = DateTime.UtcNow;
                var environmentExecuteFile = new EnvironmentExecuteFile(loggerFactory);
                var pathItems = environmentExecuteFile.GetPathExecuteFiles();
                PathItemsCache.AddRange(pathItems);
            }

            return PathItemsCache;
        }

        #endregion
    }

    public static class EnvironmentPathExecuteFileCacheExtensions
    {
        #region function

        /// <summary>
        /// コマンドから実ファイルを取得。
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="command"></param>
        /// <param name="loggerFactory"></param>
        /// <returns></returns>
        public static EnvironmentPathExecuteItem? FindExecuteItem(this EnvironmentPathExecuteFileCache cache, string command, ILoggerFactory loggerFactory)
        {
            var pathItems = cache.GetItems(loggerFactory);
            var environmentExecuteFile = new EnvironmentExecuteFile(loggerFactory);
            var pathItem = environmentExecuteFile.Get(command, pathItems);
            return pathItem;
        }

        /// <summary>
        /// 指定されたパスがコマンドとして有効ならフルパスへ変換する。
        /// </summary>
        /// <param name="path">対象パス。</param>
        /// <returns>フルパス。<param name="path" />がすでにフルパスの場合やコマンドが見つからない場合は<param name="path" />をそのまま返す。</returns>
        public static string ToFullPathIfExistsCommand(this EnvironmentPathExecuteFileCache cache, string path, ILoggerFactory loggerFactory)
        {
            if(!Path.IsPathRooted(path)) {
                var pathItem = cache.FindExecuteItem(path, loggerFactory);
                if(pathItem != null) {
                    return pathItem.File.FullName;
                }
            }

            return path;
        }

        #endregion
    }
}
