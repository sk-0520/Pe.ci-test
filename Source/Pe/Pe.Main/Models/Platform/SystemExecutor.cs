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


        #endregion
    }

}
