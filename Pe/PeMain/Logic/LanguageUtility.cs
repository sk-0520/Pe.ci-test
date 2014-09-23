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
using PeMain.Data;
using PInvoke.Windows;

namespace PeMain.Logic
{
	/// <summary>
	/// Description of ISetLanguage.
	/// </summary>
	public interface ISetLanguage
	{
		void SetLanguage(Language language);
	}
	
	public static class LanguageUtility
	{
		/// <summary>
		///ホットキーの組み合わせを文字列化。
		/// </summary>
		/// <param name="language"></param>
		/// <param name="modifiers"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static string ToDisplayText(this Language language, MOD modifiers, Keys key)
		{
			if (modifiers == MOD.None) {
				return key.ToText(language);
			}
			
			var keySeparator = language["enum/key/separator"];
			
			return modifiers.ToText(language) + keySeparator + key.ToText(language);
		}
		
		public static string ToMenuText(this Language language, string menuText, HotKeySetting hotkeySetting)
		{
			if(hotkeySetting.Enabled) {
				return string.Format("{0}\t{1}", menuText, ToDisplayText(language, hotkeySetting.Modifiers, hotkeySetting.Key));
			} else {
				return menuText;
			}
		}
	}
}
