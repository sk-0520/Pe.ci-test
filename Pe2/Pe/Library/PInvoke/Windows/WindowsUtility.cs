namespace ContentTypeTextNet.Library.PInvoke.Windows
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
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
	}
}
