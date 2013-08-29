/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/06/30
 * 時刻: 14:09
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Runtime.InteropServices;

namespace ShareLib
{
	public static partial class WindowsAPI
	{
		[DllImport("user32.dll", SetLastError=true)]
 		public static extern bool DestroyIcon(IntPtr hIcon);
	}

}
