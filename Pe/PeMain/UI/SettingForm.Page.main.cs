/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/12
 * 時刻: 0:10
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
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
		}
		
		void MainExportSetting(MainSetting mainSetting)
		{
			LogExportSetting(mainSetting.Log);
		}
	
	}
}
