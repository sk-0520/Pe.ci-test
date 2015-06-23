namespace ContentTypeTextNet.Library.SharedLibrary.View.Window
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Library.PInvoke.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.IF;

	/// <summary>
	/// × とか Alt + F4 で閉じたことを検知できる Window。
	/// </summary>
	public abstract class UserClosableWindowWindowBase: WndProcWindowBase, IUserClosableWindow
	{
		#region event

		public event CancelEventHandler UserClosing = delegate { };

		#endregion

		public UserClosableWindowWindowBase()
			: base()
		{ }

		#region WndProcWindowBase

		protected override IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if(msg == (int)WM.WM_SYSCOMMAND) {
				if(WindowsUtility.ConvertSCFromWParam(wParam) == SC.SC_CLOSE) {
					var e = new CancelEventArgs(false);
					OnUserClosing(e);
					if(e.Cancel) {
						handled = true;
					}
				}
			}
			return base.WndProc(hWnd, msg, wParam, lParam, ref handled);
		}

		#endregion

		#region IUserClosableWindow

		public void UserClose()
		{
			var e = new CancelEventArgs(false);
			OnUserClosing(e);
			if(!e.Cancel) {
				Close();
			}
		}

		#endregion

		#region function

		protected virtual void OnUserClosing(CancelEventArgs e)
		{
			UserClosing(this, e);
		}

		#endregion
	}
}
