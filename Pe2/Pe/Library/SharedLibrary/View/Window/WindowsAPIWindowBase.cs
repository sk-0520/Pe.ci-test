namespace ContentTypeTextNet.Library.SharedLibrary.View.Window
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Interop;
	using ContentTypeTextNet.Library.SharedLibrary.IF;

	public abstract class WindowsAPIWindowBase: OnLoadedWindowBase, IWindowsHandle
	{
		public WindowsAPIWindowBase()
			:base()
		{ }

		#region property

		protected WindowInteropHelper WindowInteropHelper { get; private set; }

		#region IWindowsHandle

		public IntPtr Handle { get { return WindowInteropHelper.Handle; } }

		#endregion

		#endregion

		#region OnLoadedWindowBase

		protected override void OnSourceInitialized(EventArgs e)
		{
			WindowInteropHelper = new WindowInteropHelper(this);
			
			base.OnSourceInitialized(e);
		}

		#endregion
	}
}
