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
using System.Windows.Forms;

using PeMain.Data;
using PeUtility;
using PInvoke.Windows;

namespace PeMain.Logic
{
	/// <summary>
	/// 列挙体メンバ名を言語に合わせて文字列化する。
	/// </summary>
	public static class EnumToText
	{
		public static string ToText(this LauncherType value, Language language)
		{
			var key = "enum/launcher-type/" + new Dictionary<LauncherType, string>() {
				{ LauncherType.File,      "file"},
				{ LauncherType.Directory, "directory"},
				{ LauncherType.URI,       "uri"},
				{ LauncherType.Embedded,  "embedded"},
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
		
		public static string ToText(this MOD value, Language language)
		{
			if(value == MOD.None) {
				return string.Empty;
			}
			
			const string preKey = "enum/key/mod/";
			var map = new Dictionary<MOD, string>() {
				{ MOD.MOD_ALT,     "alt" },
				{ MOD.MOD_CONTROL, "control" },
				{ MOD.MOD_SHIFT,   "shift" },
				{ MOD.MOD_WIN,     "windows" },
			};
			
			var modTextList = new List<string>();
			
			foreach(var pair in map) {
				if((value & pair.Key) == pair.Key) {
					var key = preKey + pair.Value;
					modTextList.Add(language[key]);
				}
			}
			
			var keySeparator = language["enum/key/separator"];
			return string.Join(keySeparator, modTextList);
		}
		
		public static string ToText(this Keys value, Language language)
		{
			const string preKey = "enum/key/keys/"; 
			var map = new Dictionary<Keys, string>() {
				{ Keys.D1,      "k-1" }, { Keys.D2,      "k-2" }, { Keys.D3,      "k-3" }, { Keys.D4,      "k-4" }, { Keys.D5,      "k-5" }, { Keys.D6,      "k-6" }, { Keys.D7,      "k-7" }, { Keys.D8,      "k-8" }, { Keys.D9,      "k-9" }, { Keys.D0,      "k-0" },
				{ Keys.NumPad1, "n-1" }, { Keys.NumPad2, "n-2" }, { Keys.NumPad3, "n-3" }, { Keys.NumPad4, "n-4" }, { Keys.NumPad5, "n-5" }, { Keys.NumPad6, "n-6" }, { Keys.NumPad7, "n-7" }, { Keys.NumPad8, "n-8" }, { Keys.NumPad9, "n-9" }, { Keys.NumPad0, "n-0" },
				{ Keys.Add,      "n-add" },
				{ Keys.Subtract, "n-sub" },
				{ Keys.Multiply, "n-mul" },
				{ Keys.Divide,   "n-div" },
				{ Keys.Decimal,  "n-dec" },
					
					
				{ Keys.PageUp,        "page-up" },
				{ Keys.PageDown,      "page-down" },
				{ Keys.Scroll,        "scr-lk" },
				
				{ Keys.IMEConvert,    "ime-convert" },
				{ Keys.IMENonconvert, "ime-nonconvert" },
				
				{ Keys.OemBackslash,  "oem-backslash" },
				{ Keys.Oemplus,       "oem-plus" },
				{ Keys.OemMinus,      "oem-minus" },
				{ Keys.OemSemicolon,  "oem-semicolon" },
				{ Keys.Oemcomma,      "oem-comma" },
				{ Keys.OemPeriod,     "oem-period" },
				{ Keys.Oemtilde,      "oem-tilde" },
				{ Keys.OemOpenBrackets, "oem-open-brackets" },
				{ Keys.Oem6,            "oem-6" },
				{ Keys.Oem5,            "oem-5" },
				{ Keys.Oem7,            "oem-7" },
				{ Keys.OemQuestion,     "oem-question" },
				
			};
			
			string name;
			if(map.TryGetValue(value, out name)) {
				var key = preKey + name;
				return language[key];
			} else {
				return value.ToString();
			}
		}
	}
}
