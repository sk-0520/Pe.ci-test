/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/23
 * 時刻: 8:25
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using PeMain.Data;
using PeMain.Logic;

namespace PeMain.UI
{
	/// <summary>
	/// Description of LauncherItemSelectControl_language.
	/// </summary>
	partial class LauncherItemSelectControl
	{
		void ApplyLanguage(Language language)
		{
			if(language == null) {
				return;
			}
			
			this.toolLauncherItems_create.SetLanguage(language);
			this.toolLauncherItems_remove.SetLanguage(language);
			this.toolLauncherItems_filter.SetLanguage(language);
			this.toolLauncherItems_type_full.SetLanguage(language);
			this.toolLauncherItems_type_name.SetLanguage(language);
			this.toolLauncherItems_type_tag.SetLanguage(language);
			
			ToolLauncherItems_type_Click(this.toolLauncherItems_type_full, null);
		}
	}
}
