/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/23
 * 時刻: 18:47
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Runtime.InteropServices;

namespace SC.Windows
{
	/// <summary>
	/// http://pinvoke.net/default.aspx/Structures/IMAGELISTDRAWPARAMS.html
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct IMAGELISTDRAWPARAMS
	{
		public int cbSize;
		public IntPtr himl;
		public int i;
		public IntPtr hdcDst;
		public int x;
		public int y;
		public int cx;
		public int cy;
		public int xBitmap;
		public int yBitmap;
		public int rgbBk;
		public int rgbFg;
		public int fStyle;
		public int dwRop;
		public int fState;
		public int Frame;
		public int crEffect;
	}
	
	/// <summary>
	/// http://pinvoke.net/default.aspx/Structures/IMAGEINFO.html
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct IMAGEINFO
	{
		public IntPtr hbmImage;
		public IntPtr hbmMask;
		public int Unused1;
		public int Unused2;
		public RECT rcImage;
	}

}
