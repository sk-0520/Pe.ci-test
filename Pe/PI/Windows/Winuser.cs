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
using System.Text;

namespace PI.Windows
{
	public enum WM: uint
	{
		WM_ACTIVATE = 0x06,
		WM_WINDOWPOSCHANGED = 0x47,
		WM_HOTKEY = 0x0312,
		WM_MOUSEACTIVATE = 0x21,
		WM_NCLBUTTONDOWN = 0xa1,
		WM_NCHITTEST = 0x84,
		WM_WINDOWPOSCHANGING = 0x0046,
		WM_MOVING = 0x0216,
		WM_COMMAND = 0x0111,
		WM_SETTINGCHANGE = 0x001a,
		WM_NCPAINT = 0x0085
	}
	
	public enum WM_COMMAND_SUB
	{
		Refresh = 0x7103,
	}
	
	public enum MA: int
	{
		MA_ACTIVATE = 1,
		MA_ACTIVATEANDEAT = 2,
		MA_NOACTIVATE = 3,
		MA_NOACTIVATEANDEAT = 4,
	}

	[Flags]
	public enum MOD: uint
	{
		MOD_ALT = 0x0001,
		MOD_CONTROL = 0x0002,
		MOD_SHIFT = 0x0004,
		MOD_WIN = 0x0008,
		MOD_NOREPEAT = 0x4000
	}
	
	public enum HT
	{
		HTNOWHERE = 0,
		HTCAPTION = 0x02,
		HTBORDER = 18,
		HTBOTTOM = 15,
		HTLEFT = 10,
		HTRIGHT = 11,
	}
	
	[Flags]
	public enum SWP: int
	{
		SWP_ASYNCWINDOWPOS = 0x4000,
		SWP_DEFERERASE = 0x2000,
		SWP_DRAWFRAME = 0x0020,
		SWP_FRAMECHANGED = 0x0020,
		SWP_HIDEWINDOW = 0x0080,
		SWP_NOACTIVATE = 0x0010,
		SWP_NOCOPYBITS = 0x0100,
		SWP_NOMOVE = 0x0002,
		SWP_NOOWNERZORDER = 0x0200,
		SWP_NOREDRAW = 0x0008,
		SWP_NOREPOSITION = 0x0200,
		SWP_NOSENDCHANGING = 0x0400,
		SWP_NOSIZE = 0x0001,
		SWP_NOZORDER = 0x0004,
		SWP_SHOWWINDOW = 0x0040,
	}
	
	public enum HWND
	{
		HWND_TOP = 0,
		HWND_BOTTOM = 1,
		HWND_TOPMOST = -1,
		HWND_NOTOPMOST = -2,
		HWND_BROADCAST = 0xffff
	}

	/// <summary>
	/// 
	/// </summary>
	[Flags]
	public enum SMTO : uint
	{
		SMTO_NORMAL             = 0x0,
		SMTO_BLOCK              = 0x1,
		SMTO_ABORTIFHUNG        = 0x2,
		SMTO_NOTIMEOUTIFNOTHUNG = 0x8
	}

	
	/// <summary>
	/// http://pinvoke.net/default.aspx/Structures.WINDOWPOS
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct WINDOWPOS
	{
		public IntPtr hwnd;
		public IntPtr hwndInsertAfter;
		public int x;
		public int y;
		public int cx;
		public int cy;
		public SWP flags;
	}
	
	public static partial class API
	{
		/// <summary>
		/// http://www.pinvoke.net/default.aspx/user32.sendmessage
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="Msg"></param>
		/// <param name="wParam"></param>
		/// <param name="lParam"></param>
		/// <returns></returns>
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam);
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr PostMessage(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam);
		
		/// <summary>
		/// http://www.pinvoke.net/default.aspx/user32.registerwindowmessage
		/// </summary>
		/// <param name="lpString"></param>
		/// <returns></returns>
		[DllImport("user32.dll", SetLastError=true, CharSet=CharSet.Auto)]
		public static extern uint RegisterWindowMessage(string lpString);
		
		/// <summary>
		/// http://pinvoke.net/default.aspx/user32/DestroyIcon.html
		/// </summary>
		/// <param name="hIcon"></param>
		/// <returns></returns>
		[DllImport("user32.dll", SetLastError=true)]
		public static extern bool DestroyIcon(IntPtr hIcon);

		[DllImport("user32.dll")]
		public static extern bool ReleaseCapture();
		
		[DllImport("user32.dll", SetLastError=true, CharSet=CharSet.Auto)]
		public static extern IntPtr SendMessageTimeout(IntPtr hWnd, uint Msg, UIntPtr wParam, IntPtr lParam, SMTO fuFlags, uint uTimeout, out UIntPtr lpdwResult);
		
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
		
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
		
		[DllImport("user32.dll")]
		public static extern IntPtr GetWindowDC(IntPtr hWnd);
		
		[DllImport("user32.dll")]
		public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);
	}

}
