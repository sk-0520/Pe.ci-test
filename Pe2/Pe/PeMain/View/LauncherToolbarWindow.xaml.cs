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
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using System.Windows.Interop;
	using ContentTypeTextNet.Library.SharedLibrary.View.ViewExtend;
	using Xceed.Wpf.Toolkit;
	using System.Windows.Controls.Primitives;

	/// <summary>
	/// ToolbarWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class LauncherToolbarWindow : ViewModelCommonDataWindow<LauncherToolbarViewModel>, IApplicationDesktopToolbar
	{
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
		VisualStyle VisualStyle { get; set; }

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

			ViewModel = new LauncherToolbarViewModel(model, this, CommonData);
			ViewModel.DockScreen = Screen;
			ViewModel.LauncherIcons = CommonData.LauncherIcons;
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
			int exStyle = (int)WindowsUtility.GetWindowLong(Handle, (int)GWL.GWL_EXSTYLE);
			exStyle |= (int)WS_EX.WS_EX_TOOLWINDOW;
			WindowsUtility.SetWindowLong(Handle, (int)GWL.GWL_EXSTYLE, (IntPtr)exStyle);

			base.OnLoaded(sender, e);

			Appbar = new ApplicationDesktopToolbar(this, ViewModel);
			VisualStyle = new VisualStyle(this, ViewModel);
		}

		protected override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if(Appbar != null) {
				Appbar.WndProc(hwnd, msg, wParam, lParam, ref handled);
			}
			if (VisualStyle != null) {
				VisualStyle.WndProc(hwnd, msg, wParam, lParam, ref handled);
			}
			if (handled) {
				return IntPtr.Zero;
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

		public void Docking(DockType dockType, bool autoHide)
		{
			if (Appbar != null) {
				Appbar.Docking(dockType, autoHide);
				if(VisualStyle != null) {
					//VisualStyle.UnsetStyle();
					VisualStyle.SetStyle();
				}
				//NativeMethods.UpdateWindow(Handle);
			}
		}

		#endregion

		#region function


		#endregion

		private void Caption_MouseLeftButton(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed) {
				if (ViewModel.CanWindowDrag) {
					DragMove();
				}
			}
		}

		private void Element_Click(object sender, RoutedEventArgs e)
		{
			// ダルイ、全部閉じちゃおう
			foreach(var button in UIUtility.FindVisualChildren<DropDownButton>(this).Where(b => b.IsOpen)) {
				button.IsOpen = false;
			}
		}
	}
}
