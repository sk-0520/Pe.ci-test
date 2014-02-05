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

namespace PeMain.UI
{
	/// <summary>
	/// Description of LauncherItemSelectControl_language.
	/// </summary>
	public partial class LauncherItemSelectControl
	{
		void ApplyLanguage(Language lang)
		{
			if(lang == null) {
				return;
			}
			this.toolLauncherItems_create.Text = lang["item-selecter/command/create"];
			this.toolLauncherItems_remove.Text = lang["item-selecter/command/remove"];
			this.toolLauncherItems_filter.Text = lang["item-selecter/command/filtering"];
			this.toolLauncherItems_type_full.Text = lang["item-selecter/command/type-full"];
			this.toolLauncherItems_type_name.Text = lang["item-selecter/command/type-name"];
			this.toolLauncherItems_type_tag.Text = lang["item-selecter/command/type-tag"];
		}
	}
}
