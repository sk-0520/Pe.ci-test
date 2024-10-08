using System;
using System.Diagnostics;
using System.IO;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using ContentTypeTextNet.Pe.Library.Base;

namespace ContentTypeTextNet.Pe.Main.Models.Platform
{
    /// <summary>
    /// システム上処理の実行。
    /// </summary>
    /// <remarks>
    /// <para>OS上での実行を行う(ファイルなら開いてEXEなら起動的な)</para>
    /// </remarks>
    public class SystemExecutor
    {
        #region property
        #endregion

        #region function

        /// <summary>
        /// rundll を実行。
        /// </summary>
        /// <param name="command">コマンド</param>
        /// <returns></returns>
        public Process? RunDLL(string command)
        {
            var rundll = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "rundll32.exe");
            var startupInfo = new ProcessStartInfo(rundll, command);

            return Process.Start(startupInfo);
        }

        /// <summary>
        /// タスクトレイ通知領域履歴を開く。
        /// </summary>
        /// <param name="appNonProcess"></param>
        /// <seealso cref="RunDLL(string)"/>
        public void OpenNotificationAreaHistory()
        {
            RunDLL("shell32.dll,Options_RunDLL 5");
        }

        /// <summary>
        /// URIを開く。
        /// </summary>
        /// <param name="uri"></param>
        public void OpenUri(Uri uri)
        {
            var process = new Process();
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.FileName = uri.ToString();
            process.Start();
        }

        /// <summary>
        /// ファイル配置されているディレクトリを開く。
        /// </summary>
        /// <remarks>
        /// <para>まぁ Explorer で開く。</para>
        /// </remarks>
        /// <param name="filePath">ファイルパス。</param>
        /// <returns></returns>
        public Process OpenDirectoryWithFileSelect(string filePath)
        {
            var process = new Process();
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.FileName = "explorer.exe";
            process.StartInfo.Arguments = $"/select,{CommandLine.Escape(filePath)}";
            process.Start();
            return process;
        }

        /// <summary>
        /// ファイルを実行。
        /// </summary>
        /// <param name="filePath">ファイルパス。</param>
        /// <returns></returns>
        public Process ExecuteFile(string filePath)
        {
            var process = new Process();
            var startInfo = process.StartInfo;

            // 実行パス
            startInfo.FileName = filePath;
            startInfo.UseShellExecute = true;

            process.Start();

            return process;
        }

        /// <summary>
        /// ファイルを実行。
        /// </summary>
        /// <param name="filePath">ファイルパス。</param>
        /// <param name="argument">引数。</param>
        /// <returns></returns>
        public Process ExecuteFile(string filePath, string argument)
        {
            var process = new Process();
            var startInfo = process.StartInfo;

            // 実行パス
            startInfo.FileName = filePath;
            startInfo.Arguments = argument;
            startInfo.UseShellExecute = true;

            process.Start();

            return process;
        }

        /// <summary>
        /// ファイルのプロパティを表示。
        /// </summary>
        /// <param name="filePath">ファイルパス。</param>
        public void ShowProperty(string filePath)
        {
            NativeMethods.SHObjectProperties(IntPtr.Zero, SHOP.SHOP_FILEPATH, filePath, string.Empty);
        }

        #endregion
    }
}
