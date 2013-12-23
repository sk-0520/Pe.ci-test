/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/18
 * 時刻: 15:59
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Runtime.InteropServices;

namespace Windows
{
	public enum WM: uint
	{
		WM_ACTIVATE = 0x06,
		WM_WINDOWPOSCHANGED = 0x47,
		WM_HOTKEY = 0x0312,
	}

	public enum MOD: uint
	{
		MOD_ALT = 0x0001,
		MOD_CONTROL = 0x0002,
		MOD_SHIFT = 0x0004,
		MOD_WIN = 0x0008,
		MOD_NOREPEAT = 0x4000
	}
	
	
	public static partial class API
	{
		/// <summary>
		/// http://www.pinvoke.net/default.aspx/user32.registerwindowmessage
		/// </summary>
		/// <param name="lpString"></param>
		/// <returns></returns>
		[DllImport("user32.dll", SetLastError=true, CharSet=CharSet.Auto)]
		public static extern uint RegisterWindowMessage(string lpString);
	}
}
