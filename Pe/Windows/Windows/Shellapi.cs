/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/18
 * 時刻: 13:40
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Runtime.InteropServices;

namespace Windows
{
	public enum ABM: int
	{
		/// <summary>
		/// http://msdn.microsoft.com/en-us/library/windows/desktop/bb787951%28v=vs.85%29.aspx
		/// </summary>
		ABM_NEW = 0,
		ABM_REMOVE = 1,
		ABM_QUERYPOS = 2,
		ABM_SETPOS = 3,
		ABM_GETSTATE = 4,
		ABM_GETTASKBARPOS = 5,
		ABM_ACTIVATE = 6,
		ABM_GETAUTOHIDEBAR = 7,
		ABM_SETAUTOHIDEBAR = 8,
		ABM_WINDOWPOSCHANGED = 9,
		ABM_SETSTATE = 10,
	}
	
	public enum ABN: int
	{
		ABN_STATECHANGE = 0,
		ABN_POSCHANGED = 1,
		ABN_FULLSCREENAPP = 2,
		ABN_WINDOWARRANGE = 3,
	}
	
	public enum ABE: int
	{
		ABE_LEFT = 0,
		ABE_TOP = 1,
		ABE_RIGHT = 2,
		ABE_BOTTOM = 3,
	}
	
	/// <summary>
	/// http://www.pinvoke.net/default.aspx/shell32/APPBARDATA%20.html
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct APPBARDATA
	{
		//  typedef struct _AppBarData
		//      {
		//        DWORD cbSize;
		//        HWND hWnd;
		//        UINT uCallbackMessage;
		//        UINT uEdge;
		//        RECT rc;
		//        LPARAM lParam;
		//      } APPBARDATA, *PAPPBARDATA;
		public static readonly int cbSize = Marshal.SizeOf(typeof(APPBARDATA));
		public IntPtr hWnd;
		public uint uCallbackMessage;
		public ABE uEdge;
		public RECT rc;
		public int lParam;
	}
	
	public partial class API
	{
		/// <summary>
		/// http://www.pinvoke.net/default.aspx/shell32/SHAppBarMessage.html
		/// </summary>
		/// <param name="dwMessage"></param>
		/// <param name="pData"></param>
		/// <returns></returns>
		[DllImport("shell32.dll")]
		public static extern IntPtr SHAppBarMessage(ABM dwMessage, ref APPBARDATA pData);
	}
}
