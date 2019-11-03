using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Platform
{
    public class SystemExecutor
    {
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
    }
}
