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

	public class LauncherToolbarHitTest : WindowHitTest
	{
		public LauncherToolbarHitTest(Window view, IWindowHitTestData restrictionViewModel, INonProcess nonProcess)
			: base(view, restrictionViewModel, nonProcess)
		{ }

		#region WindowHitTest

		public override IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			var result = base.WndProc(hWnd, msg, wParam, lParam, ref handled);
			if (RestrictionViewModel.UsingHitTest && handled && result != IntPtr.Zero) {
				var hitTest = (HT)result.ToInt32();
				var map = new Dictionary<HT, HT>() {
					{ HT.HTTOP, HT.HTNOWHERE },
					{ HT.HTBOTTOM, HT.HTNOWHERE },

					{ HT.HTTOPLEFT, HT.HTLEFT },
					{ HT.HTBOTTOMLEFT, HT.HTLEFT },

					{ HT.HTTOPRIGHT, HT.HTRIGHT },
					{ HT.HTBOTTOMRIGHT, HT.HTRIGHT },
				};
				HT resultHitTest;
				if (map.TryGetValue(hitTest, out resultHitTest)) {
					return new IntPtr((int)resultHitTest);
				}
			}

			return result;
		}

		#endregion
	}
}
