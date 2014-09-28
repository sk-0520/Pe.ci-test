/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/09/28
 * 時刻: 21:21
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using PeMain.Logic;

namespace PeMain.UI
{
	partial class UpdateForm
	{
		void ApplyLanguage()
		{
			UIUtility.SetDefaultText(this, CommonData.Language);
			
			this.labelRcNote.SetLanguage(CommonData.Language);
			
			var map = new Dictionary<string, string>() {
				{ SystemLanguageName.versionNow,  Literal.PeVersion },
				{ SystemLanguageName.versionNext, UpdateInfo.Version },
				{ SystemLanguageName.versionType, UpdateInfo.IsRcVersion ? "${version-rc}": "${version-release}" },
			};
			var version = CommonData.Language[this.labelVersion.Text.Substring(1)];
			version = CommonData.Language[version, map];
			version = CommonData.Language[version, map];
			version = CommonData.Language[version, map];
			this.labelVersion.Text = version;
		}
	}
}
