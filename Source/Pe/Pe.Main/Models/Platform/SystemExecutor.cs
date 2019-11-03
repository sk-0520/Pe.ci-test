using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Platform
{
    public class SystemExecuteItem
    {
        public SystemExecuteItem(DirectoryInfo directory, FileInfo file)
        {
            Directory = directory;
            File = file;
        }

        #region property

        public DirectoryInfo Directory { get; }
        public FileInfo File { get; }

        public string Name => Path.GetFileNameWithoutExtension(File.Name);

        #endregion

        #region object

        public override string ToString() => Name;

        #endregion
    }

    public class SystemExecutor
    {
        public SystemExecutor(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        ILogger Logger { get; }

        #endregion

        #region function

        public Process RunDLL(string command)
        {
            var rundll = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "rundll32.exe");
            var startupInfo = new ProcessStartInfo(rundll, command);

            return Process.Start(startupInfo);
        }

        /// <summary>
        /// タスクトレイ通知領域履歴を開く。
        /// </summary>
        /// <param name="appNonProcess"></param>
        public void OpenNotificationAreaHistory()
        {
            RunDLL("shell32.dll,Options_RunDLL 5");
        }

        /// <summary>
        /// 実行形式一覧を取得。
        /// </summary>
        /// <returns>*.ext の配列</returns>
        public IReadOnlyList<string> GetSystemExecuteExtensions()
        {
            var dotExeExts = Environment.GetEnvironmentVariable("PATHEXT");
            if(!string.IsNullOrEmpty(dotExeExts)) {
                var result =  dotExeExts
                    .Split(';')
                    .Where(i => ".X".Length <= i.Length)
                    .Where(i => i[0] == '.')
                    .Select(i => i.Trim().ToLower())
                    .OrderBy(i => i == ".exe" ? 0: 1)
                    .ThenBy(i => i)
                    .Select(i => "*" + i) // *.ext を生成
                    .ToList()
                ;
                if(result.Any()) {
                    // EXEが無いって事はないだろうけど実行できないしもうどうでもいい
                    return result;
                }
            }

            return new[] { "*.exe", "*.bat", "*.com" };

        }

        public IReadOnlyList<SystemExecuteItem> GetPathExecuteFiles()
        {
            var path = Environment.GetEnvironmentVariable("PATH");
            if(string.IsNullOrWhiteSpace(path)) {
                return new List<SystemExecuteItem>();
            }

            var rawExts = GetSystemExecuteExtensions()
                .Select(i => i.Split('.', 2, StringSplitOptions.None).Last())
                .Select(i => $"({i})")
            ;

            var extRegex = new Regex(@".*\." + string.Join("|", rawExts) + "$");
            var dirPaths = path.Split(';');
            var result = new List<SystemExecuteItem>();
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
                            var item = new SystemExecuteItem(dir, file);
                            result.Add(item);
                        }
                    }
                } catch(Exception ex) {
                    Logger.LogWarning(ex, ex.Message);
                }
            }

            return result;
        }

        #endregion
    }
}
