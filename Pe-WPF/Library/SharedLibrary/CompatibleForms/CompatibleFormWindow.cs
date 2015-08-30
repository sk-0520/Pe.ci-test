namespace ContentTypeTextNet.Library.SharedLibrary.CompatibleForms
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using Forms = System.Windows.Forms;

	/// <summary>
	/// FormsのFormをウィンドウとして扱う。
	/// <para>要はウィンドウハンドル欲しい。</para>
	/// </summary>
	public class CompatibleFormWindow : Forms.IWin32Window, IWindowsHandle
	{
		public CompatibleFormWindow(Window window)
		{
			Handle = HandleUtility.GetWindowHandle(window);
		}

		#region IWin32Window, IWindowsHandle

		public IntPtr Handle { get; private set; }

		#endregion
	}
}
