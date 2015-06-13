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

	/// <summary>
	/// ToolbarWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class LauncherToolbarWindow : ViewModelCommonDataWindow<LauncherToolbarViewModel>
	{
		#region event

		/// <summary>
		/// フルスクリーンイベント。
		/// </summary>
		public event EventHandler<AppbarFullScreenEventArgs> AppbarFullScreen;
		/// <summary>
		/// 位置変更時に発生。
		/// </summary>
		public event EventHandler<AppbarPosChangedEventArgs> AppbarPosChanged = delegate { };
		/// <summary>
		/// ステータス変更。
		/// </summary>
		public event EventHandler<AppbarStateChangeEventArgs> AppbarStateChange = delegate { };

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
			ViewModel.DockScreen = Screen;
			// 以降Viewの保持するスクリーン情報は使用しない
			Screen = null;
		}

		protected override void ApplyViewModel()
		{
			base.ApplyViewModel();

			DataContext = ViewModel;
			Appbar = ViewModel;
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);
			// Closedだとハンドルがないのでこっちで対応
			if(!e.Cancel && Appbar != null && Appbar.IsDocking) {
				UnresistAppbar();
			}
		}

		protected override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if(Appbar.IsDocking) {
				switch((int)msg) {
					case (int)WM.WM_ACTIVATE: 
						{
							var appBar = new APPBARDATA(Handle);
							NativeMethods.SHAppBarMessage(ABM.ABM_ACTIVATE, ref appBar);
						}
						break;

					case (int)WM.WM_WINDOWPOSCHANGED: 
						{
							//DockingFromProperty();
							var appBar = new APPBARDATA(Handle);
							NativeMethods.SHAppBarMessage(ABM.ABM_WINDOWPOSCHANGED, ref appBar);
						}
						break;

					case (int)WM.WM_EXITSIZEMOVE:
						{
							// 到達した試しがない
							OnResizeEnd();
						}
						break;

					default:
						if(Appbar.CallbackMessage != 0 && msg == Appbar.CallbackMessage) {
							switch(wParam.ToInt32()) {
								case (int)ABN.ABN_FULLSCREENAPP:
									// フルスクリーン
									OnAppbarFullScreen(WindowsUtility.ConvertBoolFromLParam(lParam));
									break;

								case (int)ABN.ABN_POSCHANGED:
									// 他のバーの位置が変更されたので再設定
									DockingFromProperty();
									OnAppbarPosChanged();
									break;

								case (int)ABN.ABN_STATECHANGE:
									// タスクバーの [常に手前に表示] または [自動的に隠す] が変化したとき
									// 特に何もする必要なし
									OnAppbarStateChange();
									break;

								default:
									break;
							}
						}
						break;
				}
			}

			return base.WndProc(hwnd, msg, wParam, lParam, ref handled);
		}

		#endregion


		#region function

		protected void OnAppbarFullScreen(bool fullScreen)
		{
			var e = new AppbarFullScreenEventArgs(fullScreen);
			Appbar.NowFullScreen = e.FullScreen;
			AppbarFullScreen(this, e);
		}

		protected virtual void OnAppbarPosChanged()
		{
			var e = new AppbarPosChangedEventArgs();
			AppbarPosChanged(this, e);
		}

		protected virtual void OnAppbarStateChange()
		{
			DockingFromProperty();

			var e = new AppbarStateChangeEventArgs();
			AppbarStateChange(this, e);
		}
		bool RegistAppbar()
		{
			Appbar.CallbackMessage = NativeMethods.RegisterWindowMessage(Appbar.MessageString);

			var appBar = new APPBARDATA(Handle);
			appBar.uCallbackMessage = Appbar.CallbackMessage;

			var registResult = NativeMethods.SHAppBarMessage(ABM.ABM_NEW, ref appBar);

			return registResult.ToInt32() != 0;
		}

		bool UnresistAppbar()
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

			var deviceDesktopArea = Appbar.DockScreen.DeviceBounds;

			var deviceBarSize = UIUtility.ToDevicePixel(this, Appbar.BarSize);

			double top, left, width, height;

			// 設定値からバー領域取得
			if (dockType == DockType.Left || dockType == DockType.Right) {
				top = deviceDesktopArea.Top;
				width = deviceBarSize.Width;
				height = deviceDesktopArea.Height;
				if (dockType == DockType.Left) {
					left = deviceDesktopArea.Left;
				} else {
					left = deviceDesktopArea.Right - width;
				}
			} else {
				Debug.Assert(dockType == DockType.Top || dockType == DockType.Bottom);
				left = deviceDesktopArea.Left;
				width = deviceDesktopArea.Width;
				height = deviceBarSize.Height;
				if (dockType == DockType.Top) {
					top = deviceDesktopArea.Top;
				} else {
					top = deviceDesktopArea.Bottom - height;
				}
			}

			return new Rect(left, top, width, height);
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

			Appbar.AutoHide = autoHideResult;
			Appbar.DockType = dockType;

			var deviceWindowBounds = PodStructUtility.Convert(appBar.rc);
			var logicalWindowBounds = UIUtility.ToLogicalPixel(this, deviceWindowBounds);

			if (!autoHideResult) {
				var appbarResult = NativeMethods.SHAppBarMessage(ABM.ABM_SETPOS, ref appBar);
			}

			NativeMethods.MoveWindow(Handle, appBar.rc.X, appBar.rc.Y, appBar.rc.Width, appBar.rc.Height, false);
			Appbar.ShowDeviceBarArea = PodStructUtility.Convert(appBar.rc);

			if (Appbar.AutoHide) {
				//WaitHidden();
			}

			Dispatcher.BeginInvoke(
				DispatcherPriority.ApplicationIdle,
				new Action(() => ResizeShowDeviceBarArea())
			);
		}

		void ResizeShowDeviceBarArea()
		{
			var dviceArea = Appbar.ShowDeviceBarArea;
			NativeMethods.MoveWindow(Handle, (int)dviceArea.X, (int)dviceArea.Y, (int)dviceArea.Width, (int)dviceArea.Height, true);
		}

		public void DockingFromProperty()
		{
			DockingFromParameter(Appbar.DockType, Appbar.AutoHide);
		}

		/// <summary>
		/// ドッキングの実行
		/// 
		/// すでにドッキングされている場合はドッキングを再度実行する
		/// </summary>
		public void Docking(DockType dockType)
		{
			//if(this.timerAutoHidden.Enabled) {
			//	this.timerAutoHidden.Stop();
			//}

			// 登録済みであればいったん解除
			var needResist = true;
			if(Appbar.IsDocking) {
				if(Appbar.DockType != dockType || Appbar.AutoHide) {
					UnresistAppbar();
					needResist = true;
				} else {
					needResist = false;
				}
			}

			if(dockType == DockType.None) {
				// NOTE: もっかしフルスクリーン通知拾えるかもなんで登録すべきかも。
				return;
			}

			// 登録
			if(needResist) {
				RegistAppbar();
			}

			DockingFromParameter(dockType, Appbar.AutoHide);
		}

		void OnResizeEnd()
		{
			// AppBar のサイズを更新。
			switch(Appbar.DockType) {
				case DockType.Left:
				case DockType.Right:
					Appbar.BarSize = new Size(Width, Appbar.BarSize.Height);
					break;
				case DockType.Top:
				case DockType.Bottom:
					Appbar.BarSize = new Size(Appbar.BarSize.Width, Height);
					break;
				default:
					throw new NotImplementedException();
			}

			DockingFromProperty();
		}
		#endregion
	}
}
