using System;
using System.Drawing;
using System.Windows.Forms;

namespace ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows
{
	public static partial class NativeMethods { }
	//internal static partial class NativeMethods { }

	/// <summary>
	/// PInvoke.Windows.API 関連の便利処理。
	/// </summary>
	public static class WindowsUtility
	{
		public const int classNameLength = 260;
		
		public static int GetIntUnchecked(IntPtr value)
		{
			return IntPtr.Size == 8 ? unchecked((int)value.ToInt64()) : value.ToInt32();
		}
		public static int LOWORD(IntPtr value)
		{
			return unchecked((short)GetIntUnchecked(value));
		}
		public static int HIWORD(IntPtr value)
		{
			return unchecked((short)(((uint)GetIntUnchecked(value)) >> 16));
		}
		
		public static Point ScreenPointFromLParam(IntPtr param)
		{
			/*
			return new Point(
				(int)(lParam.ToInt64() & 0xFFFF),
				(int)((lParam.ToInt64() & 0xFFFF0000) >> 16)
			);
			 */
			/*
			uint xy = unchecked(IntPtr.Size == 8 ? (uint)param.ToInt64() : (uint)param.ToInt32());
			int x = unchecked((short)xy);
			int y = unchecked((short)(xy >> 16));
			*/
			return new Point(LOWORD(param), HIWORD(param));
		}
		public static HT HTFromLParam(IntPtr param)
		{
			return (HT)LOWORD(param);
		}

		public static void ShowNoActive(Form target)
		{
			NativeMethods.SetWindowPos(
				target.Handle,
				IntPtr.Zero,
				0, 0,
				0, 0,
				SWP.SWP_NOACTIVATE | SWP.SWP_NOMOVE | SWP.SWP_NOSIZE | SWP.SWP_SHOWWINDOW
			);
		}

		static readonly IntPtr noDraw = new IntPtr(0);
		static readonly IntPtr onDraw = new IntPtr(1);

		public static void SetRedraw(Control target, bool isDraw)
		{
			var draw = isDraw ? onDraw : noDraw;
			NativeMethods.SendMessage(target.Handle, WM.WM_SETREDRAW, draw, IntPtr.Zero);
		}


	}
}
