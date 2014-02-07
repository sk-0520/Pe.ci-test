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

namespace PeMain.Logic
{
	/// <summary>
	/// Description of Executer.
	/// </summary>
	public static class Executer
	{
		public static void RunFileItem(ILogger logger, Language language, MainSetting mainSetting, ISkin skin, LauncherItem launcherItem, Form parentForm)
		{
			Debug.Assert(launcherItem.LauncherType == LauncherType.File);
			
			var process = new Process();
			var startInfo = process.StartInfo;
			startInfo.FileName = launcherItem.Command;
			if(launcherItem.IsExecteFile) {
				startInfo.UseShellExecute = false;
				
				startInfo.WorkingDirectory = launcherItem.WorkDirPath;
				startInfo.Arguments = launcherItem.Option;
				
				// 環境変数
				if(launcherItem.EnvironmentSetting.EditEnvironment) {
					var env = startInfo.EnvironmentVariables;
					// 追加・更新
					foreach(var pair in launcherItem.EnvironmentSetting.Update) {
						env[pair.Key] = pair.Value;
					}
					// 削除
					launcherItem.EnvironmentSetting.Remove
						.Where(s => env.ContainsKey(s))
						.ForEach(s => env.Remove(s))
					;
				}
				
				// 出力取得
				startInfo.CreateNoWindow = launcherItem.StdOutputWatch;
				if(launcherItem.StdOutputWatch) {
					startInfo.RedirectStandardOutput = true;
					startInfo.RedirectStandardError = true;
					var streamForm = new StreamForm();
					streamForm.Logger = logger;
					streamForm.SetParameter(process, launcherItem);
					streamForm.SetSettingData(language, mainSetting, skin);
					streamForm.Show(parentForm);
				}
			}
			
			process.Start();
			
			if(launcherItem.StdOutputWatch) {
				process.BeginOutputReadLine();
				process.BeginErrorReadLine();
			}
		}
		
		public static void OpenDirectory(string path, ILogger logger, Language language, LauncherItem openItem)
		{
			Process.Start(path);
		}
	}
}
