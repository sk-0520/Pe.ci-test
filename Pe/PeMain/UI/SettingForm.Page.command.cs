/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/04
 * 時刻: 18:13
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using PeMain.Data;

namespace PeMain.UI
{
	/// <summary>
	/// Description of SettingForm_Page_command.
	/// </summary>
	public partial class SettingForm
	{
		void CommandExportSetting(CommandSetting commandSetting)
		{
			var hotKey = this.inputCommandHotkey.Hotkey;
			var modKey = this.inputCommandHotkey.Modifiers;
			commandSetting.HotKey.Key = hotKey;
			commandSetting.HotKey.Modifiers = modKey;
		}
	}
}
