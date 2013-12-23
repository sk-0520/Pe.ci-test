/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/23
 * 時刻: 13:25
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace SC.Windows
{
	[StructLayout(LayoutKind.Sequential)]
	public struct MARGINS
	{
		public int leftWidth;
		public int rightWidth;
		public int topHeight;
		public int bottomHeight;
	}
	
	public static partial class API
	{
		/// <summary>
		/// http://www.pinvoke.net/default.aspx/dwmapi.dwmiscompositionenabled
		/// </summary>
		/// <param name="enabled"></param>
		/// <returns></returns>
		[DllImport("dwmapi.dll")]
		public static extern int DwmIsCompositionEnabled(out bool enabled);
		
		[DllImport("dwmapi.dll", PreserveSig = true)]
		public static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);
	}
}
