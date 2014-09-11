/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/09/07
 * 時刻: 17:53
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using PeMain.Logic;

namespace PeMain.UI
{
	public partial class AboutForm
	{
		void ApplyLanguage()
		{
			UIUtility.SetDefaultText(this, CommonData.Language);
			
			this.commandExecuteDir.SetLanguage(CommonData.Language);
			this.commandDataDir.SetLanguage(CommonData.Language);
			this.commandBackupDir.SetLanguage(CommonData.Language);
		}
	}
}
