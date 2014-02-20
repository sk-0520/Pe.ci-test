﻿/*
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
		public static string ToText(this LauncherType value, Language language)
		{
			var key = "enum/launcher-type/" + new Dictionary<LauncherType, string>() {
				{ LauncherType.File, "file"},
				{ LauncherType.URI,  "uri"},
				{ LauncherType.Pe,   "pe"},
			}[value];
			
			return language[key];
		}
		
		public static string ToText(this ToolbarPosition value, Language language)
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
			
			return language[key];
		}
		
		public static string ToText(this IconScale value, Language language)
		{
			var key = "enum/icon-size/" + new Dictionary<IconScale, string>() {
				{ IconScale.Small,  "small"},
				{ IconScale.Normal, "normal"},
				{ IconScale.Big,    "big"},
				{ IconScale.Large,  "large"},
			}[value];
			
			return language[key];
		}
		
		public static string ToText(this LogType value, Language language)
		{
			var key = "enum/log-type/" + new Dictionary<LogType, string>() {
				{ LogType.Information, "information"},
				{ LogType.Warning,     "warning"},
				{ LogType.Error,       "error"},
			}[value];
			
			return language[key];
		}
		
	}
}
