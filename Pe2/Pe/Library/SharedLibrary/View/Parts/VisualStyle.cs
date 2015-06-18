namespace ContentTypeTextNet.Library.SharedLibrary.View.Parts
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Interop;
	using System.Windows.Media;
	using ContentTypeTextNet.Library.PInvoke.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

	public class VisualStyle : WindowsViewExtendBase<IVisualStyleData>
	{
		public VisualStyle(Window view, IVisualStyleData restrictionViewModel)
			: base(view, restrictionViewModel)
		{
			StockBackground = view.Background;

			SetStyle();
		}

		#region property

		Brush StockBackground { get; set; }

		#endregion

		#region WindowsViewExtendBase

		public override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			switch (msg) {
				case (int)WM.WM_DWMCOMPOSITIONCHANGED:
					SetStyle();
					break;

				case (int)WM.WM_DWMCOLORIZATIONCOLORCHANGED:
					SetWindowColor();
					break;

				default:
					break;
			}

			return base.WndProc(hwnd, msg, wParam, lParam, ref handled);
		}

		#endregion

		#region function

		public void SetStyle()
		{
			SetWindowStyle();
			SetWindowColor();
		}

		protected void SetWindowStyle()
		{
			bool aeroSupport;
			NativeMethods.DwmIsCompositionEnabled(out aeroSupport);
			RestrictionViewModel.EnabledVisualStyle = aeroSupport;
			if (RestrictionViewModel.EnabledVisualStyle) {
				RestrictionViewModel.EnabledVisualStyle = true;

				// Aero Glass
				var blurHehind = new DWM_BLURBEHIND();
				blurHehind.fEnable = true;
				blurHehind.hRgnBlur = IntPtr.Zero;
				blurHehind.dwFlags = DWM_BB.DWM_BB_ENABLE | DWM_BB.DWM_BB_BLURREGION;
				NativeMethods.DwmEnableBlurBehindWindow(Handle, ref blurHehind);
				View.Background = Brushes.Transparent;
				HwndSource.FromHwnd(Handle).CompositionTarget.BackgroundColor = Colors.Transparent;
				var margins = new MARGINS() {
					leftWidth = 0,
					rightWidth = 0,
					topHeight = 0,
					bottomHeight = 0,
				};
				NativeMethods.DwmExtendFrameIntoClientArea(Handle, ref margins);
			}
		}

		protected void SetWindowColor()
		{
			if (RestrictionViewModel.EnabledVisualStyle) {
				// 色を取得
				uint rawColor;
				bool blend;
				NativeMethods.DwmGetColorizationColor(out rawColor, out blend);
				//VisualColor = Color.FromArgb((int)(rawColor & 0x00ffffff));
				var a = (byte)((rawColor & 0xff000000) >> 24);
				var r = (byte)((rawColor & 0x00ff0000) >> 16);
				var g = (byte)((rawColor & 0x0000ff00) >> 8);
				var b = (byte)((rawColor & 0x000000ff) >> 0);
				var visualColor = Color.FromArgb(a, r, g, b);
				View.Background = new SolidColorBrush(visualColor);
			} else {
				View.Background = StockBackground;
			}
		}

		#endregion

	}
}
