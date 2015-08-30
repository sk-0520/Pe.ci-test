namespace ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Interop;
	using ContentTypeTextNet.Library.SharedLibrary.IF;

	public static class HandleUtility
	{
		public static IntPtr GetWindowHandle(Window view)
		{
			var windowHandle = view as IWindowsHandle;
			if (windowHandle != null) {
				return windowHandle.Handle;
			} else {
				var helper = new WindowInteropHelper(view);
				return helper.Handle;
			}
		}
	}
}
