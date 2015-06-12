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

	/// <summary>
	/// ToolbarWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class LauncherToolbarWindow : ViewModelCommonDataWindow<LauncherToolbarViewModel>
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
		IApplicationDesktopToolbarData Appbar { get; set; }

		#endregion

		#region ViewModelCommonDataWindow

		protected override void CreateViewModel()
		{
			var model = new LauncherToolbarItemModel() {
				LauncherItems = CommonData.LauncherItemSetting.Items,
				GroupItems = CommonData.LauncherGroupItemSetting.Items,
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

			// 以降Viewの保持するスクリーン情報は使用しない
			Screen = null;
		}

		protected override void ApplyViewModel()
		{
			base.ApplyViewModel();

			DataContext = ViewModel;
		}

		#endregion

		#region function

		bool RegistAppbar()
		{
			Appbar.CallbackMessage = NativeMethods.RegisterWindowMessage(Appbar.MessageString);

			var appBar = new APPBARDATA(Handle);
			appBar.uCallbackMessage = Appbar.CallbackMessage;

			var registResult = NativeMethods.SHAppBarMessage(ABM.ABM_NEW, ref appBar);

			return registResult.ToInt32() != 0;
		}

		bool UnResistAppbar()
		{
			var appBar = new APPBARDATA(Handle);
			var unregistResult = NativeMethods.SHAppBarMessage(ABM.ABM_REMOVE, ref appBar);
			Appbar.CallbackMessage = 0;

			return unregistResult.ToInt32() != 0;
		}

		/// <summary>
		// 設定値からバー領域取得
		/// </summary>
		/// <param name="dockType"></param>
		/// <returns></returns>
		[return: PixelKind(Px.Device)]
		Rect CalcWantBarArea(DockType dockType)
		{
			Debug.Assert(dockType != DockType.None);

			var desktopArea = Appbar.DockScreen.DeviceBounds;

			var deviceBarSize = UIUtility.ToDevicePixel(this, Appbar.BarSize);

			double top, left, width, height;

			// 設定値からバー領域取得
			if (dockType == DockType.Left || dockType == DockType.Right) {
				top = desktopArea.Top;
				width = deviceBarSize.Width;
				height = desktopArea.Height;
				if (dockType == DockType.Left) {
					left = desktopArea.Left;
				} else {
					left = desktopArea.Right - width;
				}
			} else {
				Debug.Assert(dockType == DockType.Top || dockType == DockType.Bottom);
				left = desktopArea.Left;
				width = desktopArea.Width;
				height = deviceBarSize.Height;
				if (dockType == DockType.Top) {
					top = desktopArea.Top;
				} else {
					top = desktopArea.Bottom - height;
				}
			}

			return new Rect(left, width, width, height);
		}

		/// <summary>
		/// 現在の希望するサイズから実際のサイズ要求する
		/// </summary>
		/// <param name="appBar"></param>
		void TuneSystemBarArea(ref APPBARDATA appBar)
		{
			var deviceBarSize = UIUtility.ToDevicePixel(this, Appbar.BarSize);
			NativeMethods.SHAppBarMessage(ABM.ABM_QUERYPOS, ref appBar);
			switch(appBar.uEdge) {
				case ABE.ABE_LEFT:
					appBar.rc.Right = appBar.rc.Left + (int)deviceBarSize.Width;
					break;
					
				case ABE.ABE_RIGHT:
					appBar.rc.Left = appBar.rc.Right - (int)deviceBarSize.Width;
					break;
					
				case ABE.ABE_TOP:
					appBar.rc.Bottom = appBar.rc.Top + (int)deviceBarSize.Height;
					break;
					
				case ABE.ABE_BOTTOM:
					appBar.rc.Top = appBar.rc.Bottom - (int)deviceBarSize.Height;
					break;
					
				default:
					throw new NotImplementedException();
			}
		}

		public IntPtr ExistsHideWindow(DockType dockType)
		{
			Debug.Assert(dockType != DockType.None);

			var appBar = new APPBARDATA(Handle);
			appBar.uEdge = dockType.ToABE();
			var nowWnd = NativeMethods.SHAppBarMessage(ABM.ABM_GETAUTOHIDEBAR, ref appBar);

			return nowWnd;
		}

		private void DockingFromParameter(DockType dockType, bool autoHide)
		{
			Debug.Assert(dockType != DockType.None);

			var appBar = new APPBARDATA(Handle);
			appBar.uEdge = dockType.ToABE();
			appBar.rc = PodStructUtility.Convert(CalcWantBarArea(dockType));
			TuneSystemBarArea(ref appBar);

			bool autoHideResult = false;
			if (autoHide) {
				var hideWnd = ExistsHideWindow(dockType);
				if (hideWnd == IntPtr.Zero || hideWnd == Handle) {
					// 自動的に隠す
					var result = NativeMethods.SHAppBarMessage(ABM.ABM_SETAUTOHIDEBAR, ref appBar);
					autoHideResult = result.ToInt32() != 0;
					autoHideResult = true;
				}
			}
			if (!autoHideResult) {
				var appbarResult = NativeMethods.SHAppBarMessage(ABM.ABM_SETPOS, ref appBar);
			}

			Appbar.AutoHide = autoHideResult;

			var deviceWindowBounds = PodStructUtility.Convert(appBar.rc);
			var logicalWindowBounds = UIUtility.ToLogicalPixel(this, deviceWindowBounds);

			NativeMethods.MoveWindow(Handle, appBar.rc.X, appBar.rc.Y, appBar.rc.Width, appBar.rc.Height, true);
			Appbar.ShowBarSize = logicalWindowBounds.Size;

			if (Appbar.AutoHide) {
				//WaitHidden();
			}
		}

		#endregion
	}
}
