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
