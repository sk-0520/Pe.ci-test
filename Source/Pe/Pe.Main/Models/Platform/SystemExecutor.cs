using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Platform
{
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

        public void OpenUri(Uri uri)
        {
            var process = new Process();
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.FileName = uri.ToString();
            process.Start();
        }

        public Process? OpenFileInDirectory(FileInfo file)
        {
            try {
                var process = new Process();
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.FileName = "explorer.exe";
                process.StartInfo.Arguments = $"/select,\"{file.FullName}\"";
                if(process.Start()) {
                    return process;
                } else {
                    Logger.LogWarning($"fail: {nameof(process)}.{process.Start()}");
                }
            } catch(Exception ex) {
                Logger.LogWarning(ex, ex.Message);
            }

            return null;
        }

        public Process ExecuteFile(FileSystemInfo fileSystemInfo)
        {
            var process = new Process();
            var startInfo = process.StartInfo;

            // 実行パス
            startInfo.FileName = fileSystemInfo.FullName;
            startInfo.UseShellExecute = true;

            process.Start();

            return process;
        }

        public void ShowProperty(FileSystemInfo fileSystemInfo)
        {
            NativeMethods.SHObjectProperties(IntPtr.Zero, SHOP.SHOP_FILEPATH, fileSystemInfo.FullName, string.Empty);
        }

        #endregion
    }

}
