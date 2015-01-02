/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/02/05
 * 時刻: 21:38
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.XPath;

using System.Windows.Forms.VisualStyles;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.UI;
using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;
using System.Threading;
using ContentTypeTextNet.Pe.Library.Utility;
using ContentTypeTextNet.Pe.Applications;

namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	/// <summary>
	/// 実行処理共通化。
	/// </summary>
	public static class Executer
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
				process.Start();
			} catch(Win32Exception) {
				if(streamForm != null) {
					streamForm.Dispose();
				}
				throw;
			}
			
			if(getOutput) {
				streamForm = new StreamForm();
				streamForm.SetParameter(process, launcherItem);
				streamForm.SetCommonData(commonData);
				streamForm.Show();
				commonData.RootSender.AppendWindow(streamForm);
				
				process.BeginOutputReadLine();
				process.BeginErrorReadLine();
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
			
			return RunCommand(launcherItem.Command, commonData);
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
					
				default:
					throw new NotImplementedException();
			}
		}
		
		/// <summary>
		/// コマンド文字列の実行。
		/// </summary>
		/// <param name="expandPath">環境変数展開済みコマンド文字列。</param>
		/// <param name="commonData"></param>
		/// <returns></returns>
		public static Process RunCommand(string expandPath, CommonData commonData)
		{
			string exCommand = expandPath;
			
			return Process.Start(exCommand);
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

	public static class SystemExecuter
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

	public class ApplicationExecuter: IDisposable
	{
		public ApplicationExecuter(string settingPath)
		{
			ApplicationSetting = Serializer.LoadFile<ApplicationSetting>(settingPath, false);
			Items = new List<ApplicationExecuteItem>();
		}

		ApplicationSetting ApplicationSetting { get; set; }

		List<ApplicationExecuteItem> Items { get; set; }

		public IEnumerable<string> Names
		{
			get
			{
				return ApplicationSetting.Items.Select(i => i.Name);
			}
		}

		#region IDisposable

		protected virtual void Dispose(bool disposing)
		{ }

		public void Dispose()
		{
			Dispose(true);
		}

		#endregion

		public ApplicationExecuteItem CreateExecuteItem(string name)
		{
			var item = ApplicationSetting.Items.Single(s => s.Name == name);
			var result = new ApplicationExecuteItem(item);
			return result;
		}

		IDictionary<string, string> CreateExecuterEV(ApplicationItem item)
		{
			var result = new Dictionary<string, string>() {
				{ EVLiteral.systemExecuteFilePath, Literal.ApplicationExecutablePath },
				{ EVLiteral.systemDirectoryPath, Literal.ApplicationRootDirPath },
				{ EVLiteral.systemSettingDirectoryPath, Literal.UserSettingDirPath },
				{ EVLiteral.systemLogDirectoryPath, Literal.LogFileDirPath },
				// ----------------------
				{ EVLiteral.applicationSettingDirectoryPath, Path.Combine(Literal.ApplicationSettingBaseDirectoryPath, item.Name) },
				{ EVLiteral.applicationLogDirectoryPath, Path.Combine(Literal.ApplicationLogBaseDirectoryPath, item.Name) },
				// ----------------------
			};

			var communication = new Dictionary<ApplicationCommunication, TPair<string, string>>() {
				{ ApplicationCommunication.Event, TPair<string,string>.Create(EVLiteral.communicationEventName, string.Format("e-{0}", item.Name)) },
				{ ApplicationCommunication.ClientServer, TPair<string,string>.Create(EVLiteral.communicationServerName, string.Format("s-{0}", item.Name)) },
			}[item.Communication];
			result[communication.First] = communication.Second;

			return result;
		}

		public void Executer(ApplicationExecuteItem item, CommonData commonData)
		{
			if(Items.Any(i => i == item || i.Name == item.Name)) {
				throw new ArgumentException(item.Name);
			}

			var launcherItem = new LauncherItem();
			launcherItem.Name = item.Name;
			launcherItem.LauncherType = LauncherType.File;
			launcherItem.WorkDirPath = Path.Combine(
				Literal.ApplicationBinDirPath,
				item.ApplicationItem.File.Directory
			);
			launcherItem.Command = Path.Combine(
				launcherItem.WorkDirPath,
				item.ApplicationItem.File.Name
			);
			launcherItem.EnvironmentSetting.EditEnvironment = true;
			var ev = CreateExecuterEV(item.ApplicationItem);
			foreach(var pair in ev) {
				var pairItem = TPair<string, string>.Create(pair.Key, pair.Value);
				launcherItem.EnvironmentSetting.Update.Add(pairItem);
			}
			launcherItem.Administrator = item.ApplicationItem.Administrator;

			switch(item.ApplicationItem.Communication) {
				case ApplicationCommunication.Event:
					{
						var name = ev[EVLiteral.communicationEventName];
						item.Event = new EventWaitHandle(false, EventResetMode.AutoReset, name);
					}
					break;
				case ApplicationCommunication.ClientServer:
				default:
					throw new NotImplementedException();
			}

			var process = ContentTypeTextNet.Pe.PeMain.Logic.Executer.RunItem(launcherItem, commonData);

		}
	}

}
