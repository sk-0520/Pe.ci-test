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
		bool UsingAeroGlass { get; set; }

		#endregion

		#region WindowsViewExtendBase

		public override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			switch (msg) {
				case (int)WM.WM_DESTROY:
					UnsetStyle();
					break;

				case (int)WM.WM_DWMCOMPOSITIONCHANGED:
					SetStyle();
					handled = true;
					break;

				case (int)WM.WM_DWMCOLORIZATIONCOLORCHANGED:
					SetWindowColor();
					handled = true;
					break;

				//case (int)WM.WM_NCHITTEST:
				//	IntPtr result = new IntPtr();
				//	handled = NativeMethods.DwmDefWindowProc(hwnd, msg, wParam, lParam, ref result);
				//	handled = true;
				//	break;

				//case (int)WM.WM_WINDOWPOSCHANGED:
				//	SetStyle();
				//	break;

				default:
					break;
			}

			return base.WndProc(hwnd, msg, wParam, lParam, ref handled);
		}

		#endregion

		#region function

		static bool SupportAeroGlass()
		{
			var osVersion = Environment.OSVersion.Version;
			if(osVersion.Major == 6) {
				// vista, 7
				if(osVersion.Minor == 0 || osVersion.Minor == 1) {
					return true;
				}
			}

			return false;
		}

		public  void UnsetStyle()
		{
			if(RestrictionViewModel.EnabledVisualStyle && UsingAeroGlass) {
				var blurHehind = new DWM_BLURBEHIND();
				blurHehind.fEnable = false;
				//blurHehind.hRgnBlur = IntPtr.Zero;
				blurHehind.dwFlags = DWM_BB.DWM_BB_ENABLE;// | DWM_BB.DWM_BB_BLURREGION;
				NativeMethods.DwmEnableBlurBehindWindow(Handle, ref blurHehind);
				UsingAeroGlass = false;
			}
			View.Background = StockBackground;

			RestrictionViewModel.EnabledVisualStyle = false;
		}

		public void SetStyle()
		{
			//if(RestrictionViewModel.EnabledVisualStyle) {
			//	var blurHehind = new DWM_BLURBEHIND();
			//	blurHehind.fEnable = false;
			//	//blurHehind.hRgnBlur = IntPtr.Zero;
			//	blurHehind.dwFlags = DWM_BB.DWM_BB_ENABLE;// | DWM_BB.DWM_BB_BLURREGION;
			//	NativeMethods.DwmEnableBlurBehindWindow(Handle, ref blurHehind);
			//	View.Background = StockBackground;
			//	WindowsUtility.Reload(Handle);
			//	//if(RestrictionViewModel.EnabledVisualStyle) {
			//	//	var margin = new MARGINS();
			//	//	margin.leftWidth = 0;
			//	//	margin.rightWidth = 0;
			//	//	margin.topHeight = 0;
			//	//	margin.bottomHeight = 0;
			//	//	NativeMethods.DwmExtendFrameIntoClientArea(Handle, ref margin); 

			//	//	DWM_BLURBEHIND blurHehind = new DWM_BLURBEHIND();
			//	//	blurHehind.dwFlags = DWM_BB.DWM_BB_ENABLE;
			//	//	blurHehind.fEnable = false;
				
			//	//	NativeMethods.DwmEnableBlurBehindWindow(Handle, ref blurHehind);
			//	//}
			//	return;
			//}
			bool aeroSupport;
			NativeMethods.DwmIsCompositionEnabled(out aeroSupport);
			RestrictionViewModel.EnabledVisualStyle = aeroSupport;

			SetWindowStyle();
			SetWindowColor();
		}

		protected void SetWindowStyle()
		{
			if (RestrictionViewModel.EnabledVisualStyle && SupportAeroGlass()) {
				if(!UsingAeroGlass) {
					// Aero Glass
					var blurHehind = new DWM_BLURBEHIND();
					blurHehind.fEnable = true;
					blurHehind.hRgnBlur = IntPtr.Zero;
					blurHehind.dwFlags = DWM_BB.DWM_BB_ENABLE;
					NativeMethods.DwmEnableBlurBehindWindow(Handle, ref blurHehind);
					//var margins = new MARGINS() {
					//	leftWidth = -1,
					//	rightWidth = -1,
					//	topHeight = -1,
					//	bottomHeight = -1,
					//};
					HwndSource.CompositionTarget.BackgroundColor = Colors.Transparent;
					View.Background = new SolidColorBrush(Color.FromArgb(1, 255, 255, 255));
					//NativeMethods.DwmExtendFrameIntoClientArea(Handle, ref margins);
				}
				UsingAeroGlass = true;
			}
			WindowsUtility.Reload(Handle);
		}

		protected void SetWindowColor()
		{
			if (RestrictionViewModel.EnabledVisualStyle) {
				if(UsingAeroGlass) {
					return;
				}

				// 色を取得
				uint rawColor;
				bool blend;
				NativeMethods.DwmGetColorizationColor(out rawColor, out blend);
				//VisualColor = Color.FromArgb((int)(rawColor & 0x00ffffff));
				var a = (byte)((rawColor & 0xff000000) >> 24);
				var r = (byte)((rawColor & 0x00ff0000) >> 16);
				var g = (byte)((rawColor & 0x0000ff00) >> 8);
				var b = (byte)((rawColor & 0x000000ff) >> 0);

				RestrictionViewModel.VisualAlphaColor = Color.FromArgb(a, r, g, b);
				RestrictionViewModel.VisualPlainColor = Color.FromRgb(r, g, b);

				View.Background = new SolidColorBrush(RestrictionViewModel.VisualAlphaColor);
			} else {
				View.Background = StockBackground;
			}
		}

		#endregion

	}
}
