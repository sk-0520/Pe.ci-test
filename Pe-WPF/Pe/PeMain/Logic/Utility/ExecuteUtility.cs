namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
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

	public static class ExecuteUtility
	{
		static Process RunFileItem(LauncherItemModel launcherItem, INonProcess nonProcess, IAppSender appSender)
		{
			Debug.Assert(launcherItem.LauncherKind == LauncherKind.File);

			var process = new Process();
			var startInfo = process.StartInfo;
			startInfo.FileName = Environment.ExpandEnvironmentVariables(launcherItem.Command);
			var streamWatch = false;

			startInfo.Arguments = launcherItem.Option;

			if (launcherItem.Administrator) {
				startInfo.Verb = "runas";
			}

			// 作業ディレクトリ
			if (!string.IsNullOrWhiteSpace(launcherItem.WorkDirectoryPath)) {
				startInfo.WorkingDirectory = Environment.ExpandEnvironmentVariables(launcherItem.WorkDirectoryPath);
			} else if (Path.IsPathRooted(startInfo.FileName) && FileUtility.Exists(startInfo.FileName)) {
				startInfo.WorkingDirectory = Path.GetDirectoryName(startInfo.FileName);
			}

			// 環境変数
			if (launcherItem.EnvironmentVariables.Edit) {
				startInfo.UseShellExecute = false;
				var envs = startInfo.EnvironmentVariables;
				// 追加・更新
				foreach (var pair in launcherItem.EnvironmentVariables.Update) {
					envs[pair.Id] = pair.Value;
				}
				// 削除
				var removeList = launcherItem.EnvironmentVariables.Remove.Where(envs.ContainsKey);
				foreach (var key in removeList) {
					envs.Remove(key);
				}
			}

			// 出力取得
			//StreamForm streamForm = null;
			if (launcherItem.StdStream.OutputWatch) {
				streamWatch = true;
				startInfo.CreateNoWindow = true;
				startInfo.UseShellExecute = false;
				startInfo.RedirectStandardOutput = launcherItem.StdStream.OutputWatch;
				startInfo.RedirectStandardError = launcherItem.StdStream.OutputWatch;
				startInfo.RedirectStandardInput = launcherItem.StdStream.InputUsing;
			}

			LauncherItemStreamWindow streamWindow = null;
			try {
				if (streamWatch) {
					var streamData = new StreamData() {
						Process = process,
						LauncherItem = launcherItem,
					};
					streamWindow = (LauncherItemStreamWindow)appSender.SendCreateWindow(WindowKind.LauncherStream, streamData, null);
					//streamForm = new StreamForm();
					//streamForm.SetParameter(process, launcherItem);
					//streamForm.SetCommonData(commonData);
					//commonData.RootSender.AppendWindow(streamForm);
				}

				if (streamWatch) {
					streamWindow.ViewModel.Start();
					//streamWindow.Show();
				} else {
					process.Start();
				}

			} catch (Win32Exception ex) {
				nonProcess.Logger.Error(ex);
				//if (streamForm != null) {
				//	streamForm.Dispose();
				//}
				throw;
			}

			return process;
		}

		/// <summary>
		/// URIアイテム実行。
		/// </summary>
		/// <param name="launcherItem">URIアイテム</param>
		/// <param name="commonData">共通データ</param>
		/// <param name="parentForm">親ウィンドウ</param>
		private static Process RunCommandItem(LauncherItemModel launcherItem, INonProcess nonProcess, IAppSender appSender)
		{
			Debug.Assert(launcherItem.LauncherKind == LauncherKind.Command);

			//return RunCommand(launcherItem.Command, launcherItem.Option, commonData);
			var fileLauncherItem = (LauncherItemModel)launcherItem.DeepClone();
			// ファイルアイテムに変換
			fileLauncherItem.LauncherKind = LauncherKind.File;
			// 管理者権限はどうにも効かなさそう
			fileLauncherItem.Administrator = false;

			return RunFileItem(fileLauncherItem, nonProcess, appSender);
		}
		public static Process RunItem(LauncherItemModel launcherItem, ScreenModel screen, INonProcess nonProcess, IAppSender appSender)
		{
			nonProcess.Logger.Information(launcherItem.ToString());

			switch(launcherItem.LauncherKind) {
				case LauncherKind.File:
					return RunFileItem(launcherItem, nonProcess, appSender);

				case LauncherKind.Command:
					return RunCommandItem(launcherItem, nonProcess, appSender);

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

			if (string.IsNullOrWhiteSpace(arguments)) {
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
			if (FileUtility.Exists(expandedFilePath)) {
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
