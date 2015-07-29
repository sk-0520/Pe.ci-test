namespace ContentTypeTextNet.Pe.PeMain.View.Parts.ViewExtend
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Windows;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.IF.WindowsViewExtend;
	using ContentTypeTextNet.Library.SharedLibrary.View.ViewExtend;
	using ContentTypeTextNet.Library.PInvoke.Windows;

	public class CaptionCursorHitTest: WindowHitTest
	{
		public CaptionCursorHitTest(Window view, IWindowHitTestData restrictionViewModel, INonProcess appNonProcess)
			: base(view, restrictionViewModel, appNonProcess)
		{ }

		#region property

		protected bool HitCaption { get; private set; }

		#endregion

		#region WindowHitTest

		public override IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			HitCaption = false;

			if(msg == (int)WM.WM_SETCURSOR && RestrictionViewModel.UsingCaptionHitTest) {
				var hitTest = WindowsUtility.ConvertHTFromLParam(lParam);
				if(hitTest == HT.HTCAPTION) {
					NativeMethods.SetCursor(NativeMethods.LoadCursor(IntPtr.Zero, IDC.IDC_SIZEALL));
					handled = true;
					HitCaption = true;
					return new IntPtr(1);
				}
			}

			return base.WndProc(hWnd, msg, wParam, lParam, ref handled);
		}
		#endregion
	}
}
