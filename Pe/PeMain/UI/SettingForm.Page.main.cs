/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/12
 * 時刻: 0:10
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PeMain.Data;

namespace PeMain.UI
{
	/// <summary>
	/// Description of SettingForm_Page_main.
	/// </summary>
	public partial class SettingForm
	{
		void LogExportSetting(LogSetting logSetting)
		{
			logSetting.Visible = this.selectLogVisible.Checked;
			logSetting.AddShow = this.selectLogAddShow.Checked;
			
			var trigger = new Dictionary<CheckBox, LogType>() {
				{ this.selectLogTrigger_information, LogType.Information },
				{ this.selectLogTrigger_warning,     LogType.Warning },
				{ this.selectLogTrigger_error,       LogType.Error },
			};
			var logType = LogType.None;
			foreach(var t in trigger) {
				if(t.Key.Checked) {
					logType |= t.Value;
				}
			}
			logSetting.AddShowTrigger = logType;
		}
		
		void SystemEnvExportSetting(SystemEnvSetting systemEnvSetting)
		{
			systemEnvSetting.HiddenFileShowHotKey.Key = this.inputSystemEnvHiddenFile.Hotkey;
			systemEnvSetting.HiddenFileShowHotKey.Modifiers = this.inputSystemEnvHiddenFile.Modifiers;
			
			systemEnvSetting.ExtensionShowHotKey.Key = this.inputSystemEnvExt.Hotkey;
			systemEnvSetting.ExtensionShowHotKey.Modifiers = this.inputSystemEnvExt.Modifiers;
		}
		
		void MainExportSetting(MainSetting mainSetting)
		{
			LogExportSetting(mainSetting.Log);
			SystemEnvExportSetting(mainSetting.SystemEnv);
		}
	
	}
}
