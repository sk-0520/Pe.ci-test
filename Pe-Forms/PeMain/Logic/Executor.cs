namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Threading;
	using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Kind;
	using ContentTypeTextNet.Pe.PeMain.UI;

	/// <summary>
	/// 実行処理共通化。
	/// </summary>
	public static class Executor
	{
		/// <summary>
		/// ファイルアイテムの実行。
		/// </summary>
		/// <param name="launcherItem">ファイルアイテム</param>
		/// <param name="commonData">共通データ</param>
		/// <param name="parentForm">親ウィンドウ</param>
		/// <returns></returns>
		private static Process RunFileItem(LauncherItem launcherItem, CommonData commonData)
		{
			Debug.Assert(launcherItem.LauncherType == LauncherType.File);
			
			var process = new Process();
			var startInfo = process.StartInfo;
			startInfo.FileName = Environment.ExpandEnvironmentVariables(launcherItem.Command);
			var getOutput = false;

			startInfo.Arguments = launcherItem.Option;

			if(launcherItem.Administrator) {
				startInfo.Verb = "runas";
			}
			
			// 作業ディレクトリ
			if(!string.IsNullOrWhiteSpace(launcherItem.WorkDirPath)) {
				startInfo.WorkingDirectory = Environment.ExpandEnvironmentVariables(launcherItem.WorkDirPath);
			} else if(Path.IsPathRooted(startInfo.FileName) && FileUtility.Exists(startInfo.FileName)) {
				startInfo.WorkingDirectory = Path.GetDirectoryName(startInfo.FileName);
			}
			
			// 環境変数
			if(launcherItem.EnvironmentSetting.EditEnvironment) {
				startInfo.UseShellExecute = false;
				var envs = startInfo.EnvironmentVariables;
				// 追加・更新
				foreach(var pair in launcherItem.EnvironmentSetting.Update) {
					envs[pair.First] = pair.Second;
				}
				// 削除
				var removeList = launcherItem.EnvironmentSetting.Remove.Where(envs.ContainsKey);
				foreach(var key in removeList) {
					envs.Remove(key);
				}
			}
			
			// 出力取得
			StreamForm streamForm = null;
			startInfo.CreateNoWindow = launcherItem.StdOutputWatch;
			if(launcherItem.StdOutputWatch) {
				getOutput = true;
				startInfo.UseShellExecute = false;
				startInfo.RedirectStandardOutput = true;
				startInfo.RedirectStandardError = true;
				startInfo.RedirectStandardInput = true;
			}
			
			try {
				if(getOutput) {
					streamForm = new StreamForm();
					streamForm.SetParameter(process, launcherItem);
					streamForm.SetCommonData(commonData);
					commonData.RootSender.AppendWindow(streamForm);
				}

				process.Start();

				if(getOutput) {
					streamForm.StartStream();
					streamForm.Show();
				}
			} catch(Win32Exception) {
				if(streamForm != null) {
					streamForm.Dispose();
				}
				throw;
			}
			
			return process;
		}
		
		/// <summary>
		/// ディレクトリアイテム実行。
		/// </summary>
		/// <param name="launcherItem">ディレクトリアイテム</param>
		/// <param name="commonData"></param>
		/// <param name="parentForm"></param>
		private static Process RunDirectoryItem(LauncherItem launcherItem, CommonData commonData)
		{
			Debug.Assert(launcherItem.LauncherType == LauncherType.Directory);
			
			var expandPath = Environment.ExpandEnvironmentVariables(launcherItem.Command);
			return OpenDirectory(expandPath, commonData, null);
		}
		
		/// <summary>
		/// URIアイテム実行。
		/// </summary>
		/// <param name="launcherItem">URIアイテム</param>
		/// <param name="commonData">共通データ</param>
		/// <param name="parentForm">親ウィンドウ</param>
		private static Process RunCommandItem(LauncherItem launcherItem, CommonData commonData)
		{
			Debug.Assert(launcherItem.LauncherType == LauncherType.URI || launcherItem.LauncherType == LauncherType.Command);

			//return RunCommand(launcherItem.Command, launcherItem.Option, commonData);
			var fileLauncherItem = (LauncherItem)launcherItem.DeepClone();
			// ファイルアイテムに変換
			fileLauncherItem.LauncherType = LauncherType.File;
			// 管理者権限はどうにも効かなさそう
			fileLauncherItem.Administrator = false;

			return RunFileItem(fileLauncherItem, commonData);
		}
		
		/// <summary>
		/// 組み込みアイテムの実行。
		/// </summary>
		/// <param name="launcherItem"></param>
		/// <param name="commonData"></param>
		/// <returns></returns>
		private static Process RunEmbeddedItem(LauncherItem launcherItem, CommonData commonData)
		{
			Debug.Assert(launcherItem.LauncherType == LauncherType.Embedded);

			var applicationItem = commonData.ApplicationSetting.GetApplicationItem(launcherItem);
			if(commonData.ApplicationSetting.ExecutingItems.Any(i => i.ApplicationItem == applicationItem || i.Name == applicationItem.Name)) {
				throw new WarningException(launcherItem.Name + " - " + launcherItem.Command);
			}

			var executeItem = new LauncherItem();
			executeItem.Name = applicationItem.Name;
			executeItem.LauncherType = LauncherType.File;
			executeItem.WorkDirPath = applicationItem.DirectoryPath;
			executeItem.Command = applicationItem.FilePath;
			executeItem.EnvironmentSetting.EditEnvironment = true;
			var ev = applicationItem.CreateExecuterEV();
			foreach(var pair in ev) {
				var pairItem = TPair<string, string>.Create(pair.Key, pair.Value);
				executeItem.EnvironmentSetting.Update.Add(pairItem);
			}
			executeItem.Administrator = applicationItem.Administrator;
			var args = new List<string>();
			foreach(var param in applicationItem.Parameters) {
				var value = UIUtility.GetLanguage(param.Value, commonData.Language);
				string arg;
				// type
				arg = string.Format("/{0}={1}", param.Name, TextUtility.WhitespaceToQuotation(value));
				args.Add(arg);
			}
			executeItem.Option = string.Join(" ", args);

			var applicationExecuteItem = new ApplicationExecuteItem(applicationItem);

			switch(applicationItem.Communication) {
				case ApplicationCommunication.Event:
					{
						var name = ev[EVLiteral.communicationEventName];
						applicationExecuteItem.Event = new EventWaitHandle(false, EventResetMode.AutoReset, name);
					}
					break;
				case ApplicationCommunication.ClientServer:
				default:
					throw new NotImplementedException();
			}

			commonData.ApplicationSetting.ExecutingItems.Add(applicationExecuteItem);
			var process = RunFileItem(executeItem, commonData);
			applicationExecuteItem.Process = process;
			applicationExecuteItem.Process.EnableRaisingEvents = true;
			applicationExecuteItem.Process.Exited += (object sender, EventArgs e) => {
				commonData.ApplicationSetting.ExecutingItems.Remove(applicationExecuteItem);
				applicationExecuteItem.ToDispose();
			};

			return process;
		}

		/// <summary>
		/// ランチャーアイテム実行。
		/// </summary>
		/// <param name="launcherItem">ランチャーアイテム</param>
		/// <param name="commonData">共通データ</param>
		/// <param name="parentForm">親ウィンドウ</param>
		public static Process RunItem(LauncherItem launcherItem, CommonData commonData)
		{
			commonData.Logger.Puts(LogType.Information, commonData.Language["log/exec/run-item"], launcherItem);
			
			switch(launcherItem.LauncherType) {
				case LauncherType.File:
					return RunFileItem(launcherItem, commonData);
					
				case LauncherType.Directory:
					return RunDirectoryItem(launcherItem, commonData);

				case LauncherType.URI:
				case LauncherType.Command:
					return RunCommandItem(launcherItem, commonData);

				case LauncherType.Embedded:
					return RunEmbeddedItem(launcherItem, commonData);
					
				default:
					throw new NotImplementedException();
			}
		}

		public static Process RunCommand(string expandPath, CommonData commonData)
		{
			return RunCommand(expandPath, null, commonData);
		}

		/// <summary>
		/// コマンド文字列の実行。
		/// </summary>
		/// <param name="expandPath">環境変数展開済みコマンド文字列。</param>
		/// <param name="commonData"></param>
		/// <returns></returns>
		public static Process RunCommand(string expandPath, string arguments, CommonData commonData)
		{
			string exCommand = expandPath;

			if(string.IsNullOrWhiteSpace(arguments)) {
				return Process.Start(exCommand);
			} else {
				return Process.Start(exCommand, arguments);
			}
		}
		
		/// <summary>
		/// ファイルパスを規定プログラムで開く。
		/// </summary>
		/// <param name="expandPath">展開済みファイルパス</param>
		/// <param name="commonData"></param>
		/// <returns></returns>
		public static Process OpenFile(string expandPath, CommonData commonData)
		{
			return Process.Start(expandPath);
		}
		
		/// <summary>
		/// ディレクトリを開く。
		/// </summary>
		/// <param name="expandPath">展開済みディレクトリパス</param>
		/// <param name="commonData"></param>
		/// <param name="openItem"></param>
		public static Process OpenDirectory(string expandPath, CommonData commonData, LauncherItem openItem)
		{
			return Process.Start(expandPath);
		}

		public static Process OpenDirectoryWithFileSelect(string expandPath, CommonData commonData, LauncherItem openItem)
		{
			if(FileUtility.Exists(expandPath)) {
				var processName = "explorer.exe";
				var argument = string.Format("/select, {0}", expandPath);
				return Process.Start(processName, argument);
			} else {
				var dirPath = Path.GetDirectoryName(expandPath);
				return OpenDirectory(dirPath, commonData, openItem);
			}
		}

		/// <summary>
		/// プロパティを表示。
		/// </summary>
		/// <param name="expandPath">展開済みパス</param>
		/// <param name="hWnd"></param>
		public static void OpenProperty(string expandPath, IntPtr hWnd)
		{
			NativeMethods.SHObjectProperties(hWnd, SHOP.SHOP_FILEPATH, expandPath, string.Empty);
		}
	}

	public static class SystemExecutor
	{
		public static Process RunDLL(string command, CommonData commonData)
		{
			var rundll = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "rundll32.exe");
			var startupInfo = new ProcessStartInfo(rundll, command);
			
			return Process.Start(startupInfo);
		}
		
		/// <summary>
		/// タスクトレイ通知領域履歴を開く。
		/// </summary>
		/// <param name="commonData"></param>
		public static void OpenNotificationAreaHistory(CommonData commonData)
		{
			RunDLL("shell32.dll,Options_RunDLL 5", commonData);
		}
	}
}
