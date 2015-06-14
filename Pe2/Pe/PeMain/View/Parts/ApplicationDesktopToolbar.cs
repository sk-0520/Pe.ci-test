namespace ContentTypeTextNet.Pe.PeMain.View.Parts
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Interop;
	using System.Windows.Threading;
	using ContentTypeTextNet.Library.PInvoke.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.Attribute;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.PeMain.Data.Event;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Extension;

	public class ApplicationDesktopToolbar: DisposeFinalizeBase
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

		#region variable

		EventDisposer<RoutedEventHandler> _eventLoaded = null;

		#endregion variable


		public ApplicationDesktopToolbar(IApplicationDesktopToolbarData viewModel, System.Windows.Window view)
			: base()
		{
			CheckUtility.EnforceNotNull(view);
			CheckUtility.EnforceNotNull(viewModel);
			CheckUtility.Enforce(view.IsLoaded);

			ViewModel = viewModel;
			View = view;

			var windowHandle = View as IWindowsHandle;
			if(windowHandle != null) {
				Handle = windowHandle.Handle;
			} else {
				var helper = new WindowInteropHelper(View);
				Handle = helper.Handle;
			}

			//CheckUtility.EnforceNotNull(Handle);
			Docking(ViewModel.DockType);
		}

		#region property

		protected IApplicationDesktopToolbarData ViewModel { get; private set; }
		protected System.Windows.Window View { get; private set; }
		protected IntPtr Handle { get; private set; }

		protected virtual string MessageString { get { return "appbar"; } }

		#endregion

		#region DisposeFinalizeBase

		protected override void Dispose(bool disposing)
		{
			if(!IsDisposed) {
				if(this._eventLoaded != null) {
					this._eventLoaded.Dispose();
					this._eventLoaded = null;
				}

				Handle = IntPtr.Zero;
				View = null;
				ViewModel = null;
			}
			base.Dispose(disposing);
		}

		#endregion

		#region function

		public IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if(ViewModel.IsDocking) {
				switch((int)msg) {
					case (int)WM.WM_DESTROY: 
						{
							UnresistAppbar();
							handled = true;
						}
						break;

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

					case (int)WM.WM_EXITSIZEMOVE: {
							// 到達した試しがない
							OnResizeEnd();
						}
						break;

					default:
						if(ViewModel.CallbackMessage != 0 && msg == ViewModel.CallbackMessage) {
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
									DockingFromProperty();
									OnAppbarStateChange();
									break;

								default:
									break;
							}
						}
						break;
				}
			}

			return IntPtr.Zero;
		}

		protected void OnAppbarFullScreen(bool fullScreen)
		{
			var e = new AppbarFullScreenEventArgs(fullScreen);
			ViewModel.NowFullScreen = e.FullScreen;
			AppbarFullScreen(View, e);
		}

		protected virtual void OnAppbarPosChanged()
		{
			var e = new AppbarPosChangedEventArgs();
			AppbarPosChanged(View, e);
		}

		protected virtual void OnAppbarStateChange()
		{
			DockingFromProperty();

			var e = new AppbarStateChangeEventArgs();
			AppbarStateChange(View, e);
		}

		bool RegistAppbar()
		{
			ViewModel.CallbackMessage = NativeMethods.RegisterWindowMessage(MessageString);

			var appBar = new APPBARDATA(Handle);
			appBar.uCallbackMessage = ViewModel.CallbackMessage;

			var registResult = NativeMethods.SHAppBarMessage(ABM.ABM_NEW, ref appBar);
			
			return ViewModel.IsDocking = registResult.ToInt32() != 0;
		}

		public bool UnresistAppbar()
		{
			var appBar = new APPBARDATA(Handle);
			var unregistResult = NativeMethods.SHAppBarMessage(ABM.ABM_REMOVE, ref appBar);
			ViewModel.CallbackMessage = 0;
			ViewModel.IsDocking = false;
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

			var deviceDesktopArea = ViewModel.DockScreen.DeviceBounds;

			var deviceBarSize = UIUtility.ToDevicePixel(View, ViewModel.BarSize);

			double top, left, width, height;

			// 設定値からバー領域取得
			if(dockType == DockType.Left || dockType == DockType.Right) {
				top = deviceDesktopArea.Top;
				width = deviceBarSize.Width;
				height = deviceDesktopArea.Height;
				if(dockType == DockType.Left) {
					left = deviceDesktopArea.Left;
				} else {
					left = deviceDesktopArea.Right - width;
				}
			} else {
				Debug.Assert(dockType == DockType.Top || dockType == DockType.Bottom);
				left = deviceDesktopArea.Left;
				width = deviceDesktopArea.Width;
				height = deviceBarSize.Height;
				if(dockType == DockType.Top) {
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
			var deviceBarSize = UIUtility.ToDevicePixel(View, ViewModel.BarSize);
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
			if(autoHide) {
				var hideWnd = ExistsHideWindow(dockType);
				if(hideWnd == IntPtr.Zero || hideWnd == Handle) {
					// 自動的に隠す
					var result = NativeMethods.SHAppBarMessage(ABM.ABM_SETAUTOHIDEBAR, ref appBar);
					autoHideResult = result.ToInt32() != 0;
					autoHideResult = true;
				}
			}

			ViewModel.AutoHide = autoHideResult;
			ViewModel.DockType = dockType;

			var deviceWindowBounds = PodStructUtility.Convert(appBar.rc);
			var logicalWindowBounds = UIUtility.ToLogicalPixel(View, deviceWindowBounds);

			if(!autoHideResult) {
				var appbarResult = NativeMethods.SHAppBarMessage(ABM.ABM_SETPOS, ref appBar);
			}

			NativeMethods.MoveWindow(Handle, appBar.rc.X, appBar.rc.Y, appBar.rc.Width, appBar.rc.Height, false);
			ViewModel.ShowDeviceBarArea = PodStructUtility.Convert(appBar.rc);

			if(ViewModel.AutoHide) {
				//WaitHidden();
			}

			View.Dispatcher.BeginInvoke(
				DispatcherPriority.ApplicationIdle,
				new Action(() => ResizeShowDeviceBarArea())
			);
		}

		void ResizeShowDeviceBarArea()
		{
			var dviceArea = ViewModel.ShowDeviceBarArea;
			NativeMethods.MoveWindow(Handle, (int)dviceArea.X, (int)dviceArea.Y, (int)dviceArea.Width, (int)dviceArea.Height, true);
		}

		public void DockingFromProperty()
		{
			DockingFromParameter(ViewModel.DockType, ViewModel.AutoHide);
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
			if(ViewModel.IsDocking) {
				if(ViewModel.DockType != dockType || ViewModel.AutoHide) {
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

			DockingFromParameter(dockType, ViewModel.AutoHide);
		}

		void OnResizeEnd()
		{
			// AppBar のサイズを更新。
			switch(ViewModel.DockType) {
				case DockType.Left:
				case DockType.Right:
					ViewModel.BarSize = new Size(View.Width, ViewModel.BarSize.Height);
					break;
				case DockType.Top:
				case DockType.Bottom:
					ViewModel.BarSize = new Size(ViewModel.BarSize.Width, View.Height);
					break;
				default:
					throw new NotImplementedException();
			}

			DockingFromProperty();
		}

		#endregion
	}
}
