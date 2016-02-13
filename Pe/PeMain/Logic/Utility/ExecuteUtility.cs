/**
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
using ContentTypeTextNet.Pe.PeMain.Define;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.View;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
    public static class ExecuteUtility
    {
        static Process RunExecutableItem(LauncherItemModel launcherItem, ScreenModel screen, INonProcess nonProcess, IAppSender appSender)
        {
            Debug.Assert(launcherItem.LauncherKind == LauncherKind.File || launcherItem.LauncherKind == LauncherKind.Command);

            var process = new Process();
            var startInfo = process.StartInfo;
            startInfo.FileName = Environment.ExpandEnvironmentVariables(launcherItem.Command);
            var streamWatch = false;

            startInfo.Arguments = launcherItem.Option;

            if(launcherItem.Administrator) {
                startInfo.Verb = "runas";
            }

            // 作業ディレクトリ
            if(!string.IsNullOrWhiteSpace(launcherItem.WorkDirectoryPath)) {
                startInfo.WorkingDirectory = Environment.ExpandEnvironmentVariables(launcherItem.WorkDirectoryPath);
            } else if(Path.IsPathRooted(startInfo.FileName) && FileUtility.Exists(startInfo.FileName)) {
                startInfo.WorkingDirectory = Path.GetDirectoryName(startInfo.FileName);
            }

            // 環境変数
            if(launcherItem.EnvironmentVariables.Edit) {
                startInfo.UseShellExecute = false;
                var envs = startInfo.EnvironmentVariables;
                // 追加・更新
                foreach(var pair in launcherItem.EnvironmentVariables.Update) {
                    envs[pair.Id] = pair.Value;
                }
                // 削除
                var removeList = launcherItem.EnvironmentVariables.Remove.Where(envs.ContainsKey);
                foreach(var key in removeList) {
                    envs.Remove(key);
                }
            }

            // 出力取得
            //StreamForm streamForm = null;
            if(launcherItem.StdStream.OutputWatch) {
                streamWatch = true;
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = launcherItem.StdStream.OutputWatch;
                startInfo.RedirectStandardError = launcherItem.StdStream.OutputWatch;
                startInfo.RedirectStandardInput = launcherItem.StdStream.InputUsing;
            }

            LauncherItemStreamWindow streamWindow = null;
            //try {
            if(streamWatch) {
                //streamForm = new StreamForm();
                //streamForm.SetParameter(process, launcherItem);
                //streamForm.SetCommonData(commonData);
                //commonData.RootSender.AppendWindow(streamForm);
            }

            process.Start();
            if(streamWatch) {
                var streamData = new StreamData(launcherItem, screen, process);
                streamWindow = (LauncherItemStreamWindow)appSender.SendCreateWindow(WindowKind.LauncherStream, streamData, null);
                streamWindow.ViewModel.Start();
            }

            //} catch(Win32Exception ex) {
            //	nonProcess.Logger.Error(ex);
            //	//if (streamForm != null) {
            //	//	streamForm.Dispose();
            //	//}
            //	throw;
            //}

            return process;
        }

        static Process RunFileItem(LauncherItemModel launcherItem, ScreenModel screen, INonProcess nonProcess, IAppSender appSender)
        {
            Debug.Assert(launcherItem.LauncherKind == LauncherKind.File);

            return RunExecutableItem(launcherItem, screen, nonProcess, appSender);
        }

        /// <summary>
        /// URIアイテム実行。
        /// </summary>
        /// <param name="launcherItem">URIアイテム</param>
        /// <param name="commonData">共通データ</param>
        /// <param name="parentForm">親ウィンドウ</param>
        private static Process RunCommandItem(LauncherItemModel launcherItem, ScreenModel screen, INonProcess nonProcess, IAppSender appSender)
        {
            Debug.Assert(launcherItem.LauncherKind == LauncherKind.Command);

            return RunExecutableItem(launcherItem, screen, nonProcess, appSender);
        }
        public static Process RunItem(LauncherItemModel launcherItem, ScreenModel screen, INonProcess nonProcess, IAppSender appSender)
        {
            var map = new Dictionary<string, string>() {
                {  LanguageKey.logExecuteItemName, DisplayTextUtility.GetDisplayName( launcherItem) },
            };
            nonProcess.Logger.Information(nonProcess.Language["log/execute/item", map], launcherItem);

            switch(launcherItem.LauncherKind) {
                case LauncherKind.File:
                    return RunFileItem(launcherItem, screen, nonProcess, appSender);

                case LauncherKind.Command:
                    return RunCommandItem(launcherItem, screen, nonProcess, appSender);

                default:
                    throw new NotImplementedException();
            }
        }


        /// <summary>
        /// コマンド文字列の実行。
        /// </summary>
        /// <param name="expandedPath">環境変数展開済みコマンド文字列。</param>
        /// <param name="nonProcess"></param>
        /// <returns></returns>
        public static Process ExecuteCommand(string expandedPath, INonProcess nonProcess)
        {
            return ExecuteCommand(expandedPath, null, nonProcess);
        }

        /// <summary>
        /// コマンド文字列の実行。
        /// </summary>
        /// <param name="expandedPath">環境変数展開済みコマンド文字列。</param>
        /// <param name="nonProcess"></param>
        /// <returns></returns>
        public static Process ExecuteCommand(string expandedPath, string arguments, INonProcess nonProcess)
        {
            string exCommand = expandedPath;

            if(string.IsNullOrWhiteSpace(arguments)) {
                return Process.Start(exCommand);
            } else {
                return Process.Start(exCommand, arguments);
            }
        }

        /// <summary>
        /// ファイルパスを規定プログラムで開く。
        /// </summary>
        /// <param name="expandedFilePath">展開済みファイルパス</param>
        /// <param name="nonProcess"></param>
        /// <returns></returns>
        public static Process OpenFile(string expandedFilePath, INonProcess nonProcess)
        {
            return Process.Start(expandedFilePath);
        }

        /// <summary>
        /// ディレクトリを開く。
        /// </summary>
        /// <param name="expandedDirPath">展開済みディレクトリパス</param>
        /// <param name="nonProcess"></param>
        /// <param name="openItem"></param>
        public static Process OpenDirectory(string expandedDirPath, INonProcess nonProcess, LauncherItemModel openItem)
        {
            return Process.Start(expandedDirPath);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expandedFilePath"></param>
        /// <param name="nonProcess"></param>
        /// <param name="openItem"></param>
        /// <returns></returns>
        public static Process OpenDirectoryWithFileSelect(string expandedFilePath, INonProcess nonProcess, LauncherItemModel openItem)
        {
            if(FileUtility.Exists(expandedFilePath)) {
                var processName = "explorer.exe";
                var argument = string.Format("/select, {0}", expandedFilePath);
                return Process.Start(processName, argument);
            } else {
                var dirPath = Path.GetDirectoryName(expandedFilePath);
                return OpenDirectory(dirPath, nonProcess, openItem);
            }
        }

        /// <summary>
        /// プロパティを表示。
        /// </summary>
        /// <param name="expandedPath">展開済みパス</param>
        /// <param name="hWnd"></param>
        public static void OpenProperty(string expandedPath, IntPtr hWnd)
        {
            NativeMethods.SHObjectProperties(hWnd, SHOP.SHOP_FILEPATH, expandedPath, string.Empty);
        }

    }
}
