namespace ContentTypeTextNet.Library.PInvoke.Windows
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.InteropServices;
	using System.Text;
	using System.Threading.Tasks;

	public static class WindowsUtility
	{
		public static SC ConvertSCFromWParam(IntPtr wParam)
		{
			return (SC)(wParam.ToInt32() & 0xfff0);
		}

		public static bool ConvertBoolFromLParam(IntPtr lParam)
		{
			return Convert.ToBoolean(lParam.ToInt32());
		}

		public static IntPtr GetWindowLong(IntPtr hWnd, int nIndex)
		{
			if (Environment.Is64BitProcess) {
				return NativeMethods.GetWindowLongPtr(hWnd, nIndex);
			} else {
				return new IntPtr(NativeMethods.GetWindowLong(hWnd, nIndex));
			}
		}

		public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
		{
			int error = 0;
			var result = IntPtr.Zero;

			NativeMethods.SetLastError(0);

			if (Environment.Is64BitProcess) {
				result = NativeMethods.SetWindowLongPtr(hWnd, nIndex, dwNewLong);
				error = Marshal.GetLastWin32Error();
			} else {
				Int32 tempResult = NativeMethods.SetWindowLong(hWnd, nIndex, dwNewLong.ToInt32());
				error = Marshal.GetLastWin32Error();
				result = new IntPtr(tempResult);
			}

			if ((result == IntPtr.Zero) && (error != 0)) {
				throw new System.ComponentModel.Win32Exception(error);
			}

			return result;
		}

		public static void Reload(IntPtr handle)
		{
			NativeMethods.SetWindowPos(handle, IntPtr.Zero, 0, 0, 0, 0, SWP.SWP_FRAMECHANGED | SWP.SWP_NOSIZE | SWP.SWP_NOMOVE | SWP.SWP_NOZORDER | SWP.SWP_NOOWNERZORDER | SWP.SWP_NOACTIVATE);
		}
	}
}
