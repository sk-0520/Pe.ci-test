/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/02/05
 * 時刻: 22:20
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;

namespace PeMain.Data
{
	/// <summary>
	/// Description of ShortcutKeySetting.
	/// </summary>
	[Serializable]
	public class SystemEnvSetting: Item
	{
		public SystemEnvSetting()
		{
			HiddenFileShowHotkey = new HotkeySetting();
			ExtensionShowHotkey = new HotkeySetting();
		}
		
		public HotkeySetting HiddenFileShowHotkey { get; set; }
		public HotkeySetting ExtensionShowHotkey { get; set; }
	}
}
