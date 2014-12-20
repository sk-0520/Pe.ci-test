/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/02/22
 * 時刻: 0:30
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ObjectDumper;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.Library.Utility;
using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;
using System.Diagnostics;
using System.Text.RegularExpressions;

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

		public static string ClipboardItemToDisplayText(Language language, ClipboardItem clipboardItem)
		{
			var type = clipboardItem.GetSingleClipboardType();
			Debug.Assert(type != ClipboardType.None);

			string result;
			result = type.ToText(language);

			switch(type) {
				case ClipboardType.Text:
					{
						var text = clipboardItem.Text
							.SplitLines()
							.Where(s => !string.IsNullOrWhiteSpace(s))
							.Select(s => s.Trim())
							.FirstOrDefault()
						;

						if(string.IsNullOrWhiteSpace(text)) {
							result = type.ToText(language);
						} else {
							result = text;
						}
					}
					break;

				case ClipboardType.Rtf:
					{
						using(var rt = new RichTextBox()) {
							rt.Rtf = clipboardItem.Rtf;
							var text = rt.Text
								.SplitLines()
								.Where(s => !string.IsNullOrWhiteSpace(s))
								.Select(s => s.Trim())
								.FirstOrDefault()
							;

							if(string.IsNullOrWhiteSpace(text)) {
								result = type.ToText(language);
							} else {
								result = text;
							}
						}
					}
					break;

				case ClipboardType.Html:
					{
						var converted = false;
						var text = string.Join("", clipboardItem.Html.SplitLines());

						// タイトル
						var regTitle = new Regex("<title>(.+)</title>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
						var matchTitle = regTitle.Match(text);
						if(!converted && matchTitle.Success && matchTitle.Groups.Count > 1) {
							text = matchTitle.Groups[1].Value.Trim();
							converted = true;
						}

						// h1
						var regHeader = new Regex("<h1(?:.*)?>(.+)</h1>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
						var matchHeader = regHeader.Match(text);
						if(!converted && matchHeader.Success && matchHeader.Groups.Count > 1) {
							text = matchHeader.Groups[1].Value.Trim();
							Debug.WriteLine(text);
							converted = true;
						}

						if(!converted || string.IsNullOrWhiteSpace(text)) {
							result = type.ToText(language);
						} else {
							result = text;
						}
					}
					break;

				case ClipboardType.Image:
					{
						var map = new Dictionary<string,string>() {
							{ AppLanguageName.imageType, type.ToText(language) },
							{ AppLanguageName.imageWidth, clipboardItem.Image.Width.ToString() },
							{ AppLanguageName.imageHeight, clipboardItem.Image.Height.ToString() },
						};

						result = language["clipboard/title/image", map];
					}
					break;

				case ClipboardType.File:
					{
						var map = new Dictionary<string, string>() {
							{ AppLanguageName.fileType, type.ToText(language) },
							{ AppLanguageName.fileCount, clipboardItem.Files.Count().ToString() },
						};

						result = language["clipboard/title/file", map];
					}
					break;

				default:
					throw new NotImplementedException();
			}

			return result.SplitLines().FirstOrDefault();
		}

		
	}
}
