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
using PeUtility;

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
	
	public class PeHotkeyControl: ExHotkeyControl
	{
		private Language _language;
		
		public Language Language
		{
			get
			{
				return this._language;
			}
			set
			{
				this._language = value;
				Redraw();
			}
		}
	}
}
