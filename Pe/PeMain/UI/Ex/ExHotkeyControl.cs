/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/26
 * 時刻: 19:36
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using PeMain.Data;
using PeMain.IF;
using PeMain.Logic;
using ContentTypeTextNet.Pe.Library.Utility;

namespace PeMain.UI
{
	/// <summary>
	/// Description of ExHotkeyControl.
	/// </summary>
	public abstract class ExHotkeyControl: Hot_Key_Settings.HotKeyControl
	{ }
	
	public class PeHotkeyControl: ExHotkeyControl, ISetLanguage
	{
		public Language Language { get; private set; }
		public bool Registered { get; set; }
		
		public HotKeySetting HotKeySetting
		{
			get
			{
				if(DesignMode) {
					return null;
				}
				var result = new HotKeySetting();
				
				result.Key = Hotkey;
				result.Modifiers = Modifiers;
				result.IsRegistered = Registered;
				
				return result;
			}
			set
			{
				if(value == null) {
					return;
				}
				Hotkey = value.Key;
				Modifiers = value.Modifiers;
				Registered = value.IsRegistered;
			}
		}
		
		protected override string ToValueString()
		{
			if(DesignMode) {
				return base.ToValueString();
			}
			
			return LanguageUtility.HotkeyToDisplayText(Language, Modifiers, Hotkey);
		}
		
		public void SetLanguage(Language language)
		{
			Language = language;
			Redraw();
		}
	}
}
