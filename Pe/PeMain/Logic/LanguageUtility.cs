/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/02/22
 * 時刻: 0:30
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Windows.Forms;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.Library.PInvoke.Windows;

namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	/// <summary>
	/// 言語共通処理。
	/// </summary>
	public static class LanguageUtility
	{
		/// <summary>
		///ホットキーの組み合わせを文字列化。
		/// </summary>
		/// <param name="language"></param>
		/// <param name="modifiers"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static string HotkeyToDisplayText(Language language, MOD modifiers, Keys key)
		{
			if (modifiers == MOD.None) {
				return key.ToText(language);
			}
			
			var keySeparator = language["enum/key/separator"];
			
			return modifiers.ToText(language) + keySeparator + key.ToText(language);
		}

		public static string HotkeySettingToDisplayText(Language language, HotKeySetting hotkeySetting)
		{
			return HotkeyToDisplayText(language, hotkeySetting.Modifiers, hotkeySetting.Key);
		}
		
		/*
		public static string HotKeySettingToMenuText(Language language, string menuText, HotKeySetting hotkeySetting)
		{
			if(hotkeySetting.Enabled) {
				return string.Format("{0}\t{1}", menuText, HotkeyToDisplayText(language, hotkeySetting.Modifiers, hotkeySetting.Key));
			} else {
				return menuText;
			}
		}
		*/
		
		public static string FontSettingToDisplayText(Language language, FontSetting font)
		{
			string viewText = language["common/command/default-font"];
			if(!font.IsDefault) {
				viewText = string.Format("{0} {1}", font.Family, font.Height);
			}
			
			return viewText;
		}

		
	}
}
