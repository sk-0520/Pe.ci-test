/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/29
 * 時刻: 23:04
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using PeMain.Data;
using PeUtility;

namespace PeMain.Logic
{
	/// <summary>
	/// Description of EnumLang.
	/// </summary>
	public static class EnumLang
	{
		public static string ToText(this LauncherType value, Language lang)
		{
			var key = "enum/launcher-type/" + new Dictionary<LauncherType, string>() {
				{ LauncherType.File, "file"},
				{ LauncherType.URI,  "uri"},
				{ LauncherType.Pe,   "pe"},
			}[value];
			
			return lang[key];
		}
		
		public static string ToText(this ToolbarPosition value, Language lang)
		{
			var key = "enum/toolbar-position/" + new Dictionary<ToolbarPosition, string>() {
				{ ToolbarPosition.DesktopFloat,  "desktop/float" },
				{ ToolbarPosition.DesktopLeft,   "desktop/left" },
				{ ToolbarPosition.DesktopTop,    "desktop/top" },
				{ ToolbarPosition.DesktopRight,  "desktop/right" },
				{ ToolbarPosition.DesktopBottom, "desktop/bottom" },
				{ ToolbarPosition.WindowLeft,    "window/left" },
				{ ToolbarPosition.WindowTop,     "window/top" },
				{ ToolbarPosition.WindowRight,   "window/right" },
				{ ToolbarPosition.WindowBottom,  "window/bottom" },
			}[value];
			
			return lang[key];
		}
		
		public static string ToText(this IconSize value, Language lang)
		{
			var key = "enum/icon-size/" + new Dictionary<IconSize, string>() {
				{ IconSize.Small,  "small"},
				{ IconSize.Normal, "normal"},
				{ IconSize.Big,    "big"},
				{ IconSize.Large,  "large"},
			}[value];
			
			return lang[key];
		}
		
	}
}
