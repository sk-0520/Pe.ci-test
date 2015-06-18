namespace ContentTypeTextNet.Pe.PeMain.View
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Data;
	using System.Windows.Documents;
	using System.Windows.Input;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using System.Windows.Shapes;
	using ContentTypeTextNet.Library.PInvoke.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.View.Parts.Window;
	using ContentTypeTextNet.Pe.PeMain.ViewModel;
	using ContentTypeTextNet.Pe.PeMain.Logic.Extension;
	using System.Windows.Threading;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Attribute;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data.Event;
	using System.ComponentModel;
	using ContentTypeTextNet.Pe.PeMain.View.Parts;
	using ContentTypeTextNet.Pe.Library.PeData.IF;
	using ContentTypeTextNet.Library.SharedLibrary.View.Parts;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using System.Windows.Interop;

	/// <summary>
	/// ToolbarWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class LauncherToolbarWindow : ViewModelCommonDataWindow<LauncherToolbarViewModel>, IApplicationDesktopToolbar
	{
		#region property

		bool _useVisualStyle = false;
		Color _visualColor;
		
		#endregion

		public LauncherToolbarWindow()
		{
			InitializeComponent();
		}

		public LauncherToolbarWindow(ScreenModel screen)
			: this()
		{
			if (screen == null) {
				throw new ArgumentNullException("screen");
			}
			Screen = screen;
		}

		#region property

		ScreenModel Screen { get; set; }

		ApplicationDesktopToolbar Appbar { get; set; }

		#endregion

		#region ViewModelCommonDataWindow

		protected override void CreateViewModel()
		{
			var model = new LauncherToolbarItemModel() {
				LauncherItems = CommonData.LauncherItemSetting.Items,
				GroupItems = CommonData.LauncherGroupSetting.Groups,
			};

			ToolbarItemModel toolbar;
			if (Screen != null) {
				if(!CommonData.MainSetting.Toolbar.TryGetValue(Screen.DeviceName, out toolbar)) {
					toolbar = new ToolbarItemModel();
					toolbar.Id = Screen.DeviceName;
					CommonData.Logger.Information("create toolbar", Screen);
					CommonData.MainSetting.Toolbar.Add(toolbar);
				}
			} else {
				CommonData.Logger.Debug("dummy toolbar");
				toolbar = new ToolbarItemModel();
			}
			model.Toolbar = toolbar;
			
			ViewModel = new LauncherToolbarViewModel(model, this);
			ViewModel.DockScreen = Screen;
			// 以降Viewの保持するスクリーン情報は使用しない
			Screen = null;
		}

		protected override void ApplyViewModel()
		{
			base.ApplyViewModel();

			DataContext = ViewModel;
		}

		protected override void OnLoaded(object sender, RoutedEventArgs e)
		{
			base.OnLoaded(sender, e);
			Appbar = new ApplicationDesktopToolbar(ViewModel, this);

			SetStyle();
		}

		protected override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if(Appbar != null) {
				Appbar.WndProc(hwnd, msg, wParam, lParam, ref handled);
				if(handled) {
					return IntPtr.Zero;
				}
			}
			if (this._useVisualStyle) {
				if (msg == (int)WM.WM_DWMCOMPOSITIONCHANGED) {
					SetStyleColor();
				}
			}
			return base.WndProc(hwnd, msg, wParam, lParam, ref handled);
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			if(Appbar != null) {
				Appbar.Dispose();
				Appbar = null;
			}
		}

		#endregion

		#region IApplicationDesktopToolbar

		public void Docking(DockType dockType)
		{
			if (Appbar != null) {
				Appbar.Docking(dockType);
			}
		}

		#endregion

		#region function

		void SetStyle()
		{
			bool aeroSupport;
			NativeMethods.DwmIsCompositionEnabled(out aeroSupport);
			if(aeroSupport) {
				this._useVisualStyle = true;

				// Aero Glass
				var blurHehind = new DWM_BLURBEHIND();
				blurHehind.fEnable = true;
				blurHehind.hRgnBlur = IntPtr.Zero;
				blurHehind.dwFlags = DWM_BB.DWM_BB_ENABLE | DWM_BB.DWM_BB_BLURREGION;
				NativeMethods.DwmEnableBlurBehindWindow(Handle, ref blurHehind);
				this.Background = Brushes.Transparent;
				HwndSource.FromHwnd(Handle).CompositionTarget.BackgroundColor = Colors.Transparent;
				var margins = new MARGINS() {
					leftWidth = 0,
					rightWidth = 0,
					topHeight = 0,
					bottomHeight = 0,
				};
				NativeMethods.DwmExtendFrameIntoClientArea(Handle, ref margins);
				// 色を取得
				SetStyleColor();
			} else {
				this._useVisualStyle = false;
			}
		}

		void SetStyleColor()
		{
			uint rawColor;
			bool blend;
			NativeMethods.DwmGetColorizationColor(out rawColor, out blend);
			//VisualColor = Color.FromArgb((int)(rawColor & 0x00ffffff));
			var a = (byte)((rawColor & 0xff000000) >> 24);
			var r = (byte)((rawColor & 0x00ff0000) >> 16);
			var g = (byte)((rawColor & 0x0000ff00) >> 8);
			var b = (byte)((rawColor & 0x000000ff) >> 0);
			this._visualColor = Color.FromArgb(a, r, g, b);

			Background = new SolidColorBrush(this._visualColor);
		}

		#endregion

		private void Caption_MouseLeftButton(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed) {
				if (ViewModel.CanWindowDrag) {
					DragMove();
				}
			}
		}
	}
}
