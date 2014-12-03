/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/09/07
 * 時刻: 17:53
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using ContentTypeTextNet.Pe.PeMain.Logic;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	partial class AboutForm
	{
		void ApplyLanguage()
		{
			UIUtility.SetDefaultText(this, CommonData.Language);
			
			this.commandExecuteDir.SetLanguage(CommonData.Language);
			this.commandDataDir.SetLanguage(CommonData.Language);
			this.commandBackupDir.SetLanguage(CommonData.Language);
			this.commandChangelog.SetLanguage(CommonData.Language);
			this.commandUpdate.SetLanguage(CommonData.Language);
			
			this.labelUserenv.SetLanguage(CommonData.Language);
			this.linkCopyShort.SetLanguage(CommonData.Language);
			this.linkCopyLong.SetLanguage(CommonData.Language);
			
			this.gridComponents_columnName.SetLanguage(CommonData.Language);
			this.gridComponents_columnType.SetLanguage(CommonData.Language);
			this.gridComponents_columnLicense.SetLanguage(CommonData.Language);
		}
	}
}
