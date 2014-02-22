﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/26
 * 時刻: 19:36
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using PeMain.Data;
using PeMain.Logic;
using PeUtility;
using PI.Windows;

namespace PeMain.UI
{
	/// <summary>
	/// Description of ExHotkeyControl.
	/// </summary>
	public class ExHotkeyControl: HotKeyControl
	{
		public ExHotkeyControl()
		{
		}
	}
	
	public class PeHotkeyControl: ExHotkeyControl, ISetLanguage
	{
		public Language Language { get; private set; }
		public bool Resisted { get; set; }
		
		protected override string ToValueString()
		{
			if (Modifiers == MOD.None) {
				return Hotkey.ToText(Language);
			}
			
			var keySeparator = Language["enum/key/separator"];
			
			return Modifiers.ToText(Language)+ keySeparator + Hotkey.ToText(Language);
		}

		public void SetLanguage(Language language)
		{
			Language = language;
			Redraw();
		}
	}
}
