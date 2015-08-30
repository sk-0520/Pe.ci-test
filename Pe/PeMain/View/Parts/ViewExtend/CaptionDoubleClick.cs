namespace ContentTypeTextNet.Pe.PeMain.View.Parts.ViewExtend
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Windows;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.View.ViewExtend;
	using ContentTypeTextNet.Pe.PeMain.IF.ViewExtend;
	using ContentTypeTextNet.Library.PInvoke.Windows;
	using System.ComponentModel;

	public class CaptionDoubleClick: WindowsViewExtendBase<ICaptionDoubleClickData>
	{
		public CaptionDoubleClick(Window view, ICaptionDoubleClickData restrictionViewModel, INonProcess nonProcess)
			: base(view, restrictionViewModel, nonProcess)
		{ }

		#region function

		void OnCaptionDoubleClick(CancelEventArgs e)
		{
			//RestrictionViewModel.CaptionDoubleClick(this, e);
		}

		#endregion

		#region WindowsViewExtendBase

		public override IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if(msg == (int)WM.WM_SYSCOMMAND) {
				var sc = WindowsUtility.ConvertSCFromWParam(wParam);
				if(sc == SC.SC_MAXIMIZE) {
					var e = new CancelEventArgs();
					RestrictionViewModel.OnCaptionDoubleClick(this, e);
					if(!e.Cancel) {
						handled = true;
						return IntPtr.Zero;
					}
				}
			}
			return base.WndProc(hWnd, msg, wParam, lParam, ref handled);
		}

		#endregion
	}
}
