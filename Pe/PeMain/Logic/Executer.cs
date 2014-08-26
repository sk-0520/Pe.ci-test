/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/02/05
 * 時刻: 21:38
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

using PeMain.Data;
using PeMain.UI;
using PeUtility;
using PInvoke.Windows;

namespace PeMain.Logic
{
	/// <summary>
	/// Description of Executer.
	/// </summary>
	public static class Executer
	{
		public static void RunFileItem(CommonData commonData, LauncherItem launcherItem, Form parentForm)
		{
			Debug.Assert(launcherItem.LauncherType == LauncherType.File);
			
			commonData.Logger.Puts(LogType.Information, commonData.Language["log/exec/run-item"], launcherItem);
			
			var process = new Process();
			var startInfo = process.StartInfo;
			startInfo.FileName = Environment.ExpandEnvironmentVariables(launcherItem.Command);
			var getOutput = false;
			if(launcherItem.IsExecteFile) {
				startInfo.Arguments = launcherItem.Option;
				
				if(launcherItem.Administrator) {
					startInfo.Verb = "runas";
				} else {
					startInfo.UseShellExecute = false;
					
					if(!string.IsNullOrWhiteSpace(launcherItem.WorkDirPath)) {
						startInfo.WorkingDirectory = Environment.ExpandEnvironmentVariables(launcherItem.WorkDirPath);
					}
					
					// 環境変数
					if(launcherItem.EnvironmentSetting.EditEnvironment) {
						var envs = startInfo.EnvironmentVariables;
						// 追加・更新
						foreach(var pair in launcherItem.EnvironmentSetting.Update) {
							envs[pair.First] = pair.Second;
						}
						// 削除
						var removeList = launcherItem.EnvironmentSetting.Remove.Where(s => envs.ContainsKey(s));
						foreach(var key in removeList) {
							envs.Remove(key);
						}
					}
					
					// 出力取得
					startInfo.CreateNoWindow = launcherItem.StdOutputWatch;
					if(launcherItem.StdOutputWatch) {
						getOutput = true;
						startInfo.RedirectStandardOutput = true;
						startInfo.RedirectStandardError = true;
						var streamForm = new StreamForm();
						streamForm.SetParameter(process, launcherItem);
						streamForm.SetCommonData(commonData);
						streamForm.Show(parentForm);
					}
				}
			}
			
			process.Start();
			
			if(getOutput) {
				process.BeginOutputReadLine();
				process.BeginErrorReadLine();
			}
		}
		
		public static void OpenDirectory(string path, ILogger logger, Language language, LauncherItem openItem)
		{
			Process.Start(path);
		}
		
		public static void OpenProperty(string path, IntPtr hWnd)
		{
			API.SHObjectProperties(hWnd, SHOP.SHOP_FILEPATH, path, string.Empty);
		}
		
	}
}
