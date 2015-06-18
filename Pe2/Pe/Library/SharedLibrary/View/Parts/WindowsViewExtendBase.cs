namespace ContentTypeTextNet.Library.SharedLibrary.View.Parts
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Interop;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.IF.Marker;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;

	/// <summary>
	/// ウィンドウとかに何かしら機能拡張する実装。
	/// </summary>
	/// <typeparam name="TViewModel"></typeparam>
	public abstract class WindowsViewExtendBase<TViewModel> : DisposeFinalizeBase
		where TViewModel : class, IWindowsViewExtendRestrictionViewModelMarker
	{
		public WindowsViewExtendBase(Window view, TViewModel restrictionViewModel)
			: base()
		{
			CheckUtility.EnforceNotNull(view);
			CheckUtility.EnforceNotNull(restrictionViewModel);
			CheckUtility.Enforce(view.IsLoaded);

			View = view;
			RestrictionViewModel = restrictionViewModel;


			Handle = HandleUtility.GetWindowHandle(View);
			HwndSource = HwndSource.FromHwnd(Handle);
		}

		#region property

		protected TViewModel RestrictionViewModel { get; private set; }
		protected Window View { get; private set; }
		protected IntPtr Handle { get; private set; }
		protected HwndSource HwndSource { get; private set; }

		#endregion

		#region DisposeFinalizeBase

		protected override void Dispose(bool disposing)
		{
			if (!IsDisposed) {
				if (HwndSource != null) {
					HwndSource.Dispose();
					HwndSource = null;
				}
				Handle = IntPtr.Zero;
				RestrictionViewModel = null;
				View = null;
			}
			base.Dispose(disposing);
		}

		#endregion

		#region function

		public virtual IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			return IntPtr.Zero;
		}

		#endregion

	}
}
