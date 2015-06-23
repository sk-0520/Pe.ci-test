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

	/// <summary>
	/// Windowsのウィンドウプロシージャを持つWindow。
	/// </summary>
	public abstract class WndProcWindowBase: WindowsAPIWindowBase
	{
		#region variable

		HwndSource _hWndSource;

		#endregion

		public WndProcWindowBase()
			: base()
		{ }

		#region property

		protected HwndSource HandleSource 
		{ 
			get 
			{
				if(this._hWndSource == null) {
					this._hWndSource = HwndSource.FromHwnd(Handle);
				}

				return this._hWndSource;
			} 
		}

		#endregion

		#region WindowsAPIWindowBase

		protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);

			HandleSource.AddHook(WndProc);
			Closed += WndProcWindowBase_Closed;
		}

		#endregion

		#region function

		protected virtual IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			return IntPtr.Zero;
		}

		#endregion

		void WndProcWindowBase_Closed(object sender, EventArgs e)
		{
			Closed -= WndProcWindowBase_Closed;
			HandleSource.RemoveHook(WndProc);
		}
	}
}
