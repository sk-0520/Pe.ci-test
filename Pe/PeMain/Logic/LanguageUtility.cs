namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Text.RegularExpressions;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Kind;

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

		static string ClipboardTypeToDisplayText(Language language, ClipboardType clipboardType)
		{
			return string.Format("<{0}>", clipboardType.ToText(language));
		}
		public static string ClipboardItemToDisplayText(Language language, ClipboardItem clipboardItem, ILogger logger)
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
							result = ClipboardTypeToDisplayText(language, type);
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
								result = ClipboardTypeToDisplayText(language, type);
							} else {
								result = text;
							}
						}
					}
					break;

				case ClipboardType.Html:
					{
						var takeCount = 64;
						var converted = false;
						var lines = clipboardItem.Html.SplitLines().Take(takeCount);
						var text = string.Join("", lines);
						//var text = clipboardItem.Html.Replace('\r', ' ').Replace('\n', ' ');

						var timeTitle = TimeSpan.FromMilliseconds(500);
						var timeHeader = TimeSpan.FromMilliseconds(500);

						// タイトル
						try {
							var regTitle = new Regex("<title>(.+)</title>", RegexOptions.IgnoreCase | RegexOptions.Multiline, timeTitle);
							var matchTitle = regTitle.Match(text);
							if(!converted && matchTitle.Success && matchTitle.Groups.Count > 1) {
								text = matchTitle.Groups[1].Value.Trim();
								converted = true;
							}
						} catch(RegexMatchTimeoutException ex) {
							logger.Puts(LogType.Warning, "title:" + ex.Message, ex);
						}

						// h1
						try {
							var regHeader = new Regex("<h1(?:.*)?>(.+)</h1>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
							var matchHeader = regHeader.Match(text);
							if(!converted && matchHeader.Success && matchHeader.Groups.Count > 1) {
								text = matchHeader.Groups[1].Value.Trim();
								Debug.WriteLine(text);
								converted = true;
							}
						} catch(RegexMatchTimeoutException ex) {
							logger.Puts(LogType.Warning, "header:" + ex.Message, ex);
						}

						if(!converted || string.IsNullOrWhiteSpace(text)) {
							result = ClipboardTypeToDisplayText(language, type);
						} else {
							result = text;
						}
					}
					break;

				case ClipboardType.Image:
					{
						var map = new Dictionary<string,string>() {
							{ AppLanguageName.imageType, ClipboardTypeToDisplayText(language, type) },
							{ AppLanguageName.imageWidth, clipboardItem.Image.Width.ToString() },
							{ AppLanguageName.imageHeight, clipboardItem.Image.Height.ToString() },
						};

						result = language["clipboard/title/image", map];
					}
					break;

				case ClipboardType.File:
					{
						var map = new Dictionary<string, string>() {
							{ AppLanguageName.fileType, ClipboardTypeToDisplayText(language, type) },
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

		enum ApplicationItemText
		{
			Title,
			Comment,
		}
		static string GetApplicationItemText(ApplicationItem item, ApplicationItemText type)
		{
			string typeKey;
			switch(type) {
				case ApplicationItemText.Title:
					typeKey = "title";
					break;
				case ApplicationItemText.Comment:
					typeKey = "comment";
					break;
				default:
					throw new NotImplementedException();
			}

			return string.Format("applications/{0}/{1}", item.LanguageKey, typeKey);
		}
		public static string ApplicationItemToTitle(Language language, ApplicationItem item)
		{
			return language[GetApplicationItemText(item, ApplicationItemText.Title)];
		}
		public static string ApplicationItemToComment(Language language, ApplicationItem item)
		{
			return language[GetApplicationItemText(item, ApplicationItemText.Comment)];
		}
	}
}
