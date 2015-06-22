﻿namespace ContentTypeTextNet.Library.SharedLibrary.View.ViewExtend
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Windows;
	using System.Windows.Input;
	using System.Windows.Interop;
	using System.Windows.Threading;
	using ContentTypeTextNet.Library.PInvoke.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.Attribute;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.Event;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Pe.PeMain.Logic.Extension;

	/// <summary>
	/// Windowにアプリケーションデスクトップツールバー機能を付与する。
	/// </summary>
	public class ApplicationDesktopToolbar : WindowsViewExtendBase<IApplicationDesktopToolbarData>
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

		public ApplicationDesktopToolbar(Window view, IApplicationDesktopToolbarData restrictionViewModel)
			: base(view, restrictionViewModel)
		{
			View.IsVisibleChanged += View_IsVisibleChanged;

			AutoHideTimer = new DispatcherTimer();
			AutoHideTimer.Tick += TimerAutoHide_Tick;

			if (view.Visibility == Visibility.Visible) {
				Docking(RestrictionViewModel.DockType);
			}
		}

		#region property

		protected virtual string MessageString { get { return "appbar"; } }
		protected virtual DispatcherTimer AutoHideTimer { get; private set; }

		#endregion

		#region DisposeFinalizeBase

		protected override void Dispose(bool disposing)
		{
			if (!IsDisposed) {
				View.IsVisibleChanged -= View_IsVisibleChanged;
			}
			base.Dispose(disposing);
		}

		#endregion

		#region WindowsViewExtendBase

		public override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (RestrictionViewModel.IsDocking) {
				switch ((int)msg) {
					case (int)WM.WM_DESTROY: {
							UnresistAppbar();
							handled = true;
						}
						break;

					case (int)WM.WM_ACTIVATE: {
							var appBar = new APPBARDATA(Handle);
							NativeMethods.SHAppBarMessage(ABM.ABM_ACTIVATE, ref appBar);
						}
						break;

					case (int)WM.WM_WINDOWPOSCHANGED: {
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
						if (RestrictionViewModel.CallbackMessage != 0 && msg == RestrictionViewModel.CallbackMessage) {
							switch (wParam.ToInt32()) {
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

			return base.WndProc(hwnd, msg, wParam, lParam, ref handled);
		}

		#endregion

		#region function

		protected void OnAppbarFullScreen(bool fullScreen)
		{
			var e = new AppbarFullScreenEventArgs(fullScreen);
			RestrictionViewModel.NowFullScreen = e.FullScreen;
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

		void OnResizeEnd()
		{
			// AppBar のサイズを更新。
			switch (RestrictionViewModel.DockType) {
				case DockType.Left:
				case DockType.Right:
					RestrictionViewModel.BarSize = new Size(View.Width, RestrictionViewModel.BarSize.Height);
					break;
				case DockType.Top:
				case DockType.Bottom:
					RestrictionViewModel.BarSize = new Size(RestrictionViewModel.BarSize.Width, View.Height);
					break;
				default:
					throw new NotImplementedException();
			}

			DockingFromProperty();
		}

		bool RegistAppbar()
		{
			RestrictionViewModel.CallbackMessage = NativeMethods.RegisterWindowMessage(MessageString);

			var appBar = new APPBARDATA(Handle);
			appBar.uCallbackMessage = RestrictionViewModel.CallbackMessage;

			var registResult = NativeMethods.SHAppBarMessage(ABM.ABM_NEW, ref appBar);

			return RestrictionViewModel.IsDocking = registResult.ToInt32() != 0;
		}

		public bool UnresistAppbar()
		{
			var appBar = new APPBARDATA(Handle);
			var unregistResult = NativeMethods.SHAppBarMessage(ABM.ABM_REMOVE, ref appBar);
			RestrictionViewModel.CallbackMessage = 0;
			RestrictionViewModel.IsDocking = false;
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

			var deviceDesktopArea = RestrictionViewModel.DockScreen.DeviceBounds;

			var deviceBarSize = UIUtility.ToDevicePixel(View, RestrictionViewModel.BarSize);

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
			var deviceBarSize = UIUtility.ToDevicePixel(View, RestrictionViewModel.BarSize);
			NativeMethods.SHAppBarMessage(ABM.ABM_QUERYPOS, ref appBar);

			switch (appBar.uEdge) {
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
			appBar.uEdge = DockTypeUtility.ToABE(dockType);
			var nowWnd = NativeMethods.SHAppBarMessage(ABM.ABM_GETAUTOHIDEBAR, ref appBar);

			return nowWnd;
		}

		void DockingFromParameter(DockType dockType, bool autoHide)
		{
			Debug.Assert(dockType != DockType.None);

			var appBar = new APPBARDATA(Handle);
			appBar.uEdge = DockTypeUtility.ToABE(dockType);
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

			RestrictionViewModel.AutoHide = autoHideResult;
			RestrictionViewModel.DockType = dockType;

			var deviceWindowBounds = PodStructUtility.Convert(appBar.rc);
			var logicalWindowBounds = UIUtility.ToLogicalPixel(View, deviceWindowBounds);

			if (!autoHideResult) {
				var appbarResult = NativeMethods.SHAppBarMessage(ABM.ABM_SETPOS, ref appBar);
			}

			NativeMethods.MoveWindow(Handle, appBar.rc.X, appBar.rc.Y, appBar.rc.Width, appBar.rc.Height, false);
			RestrictionViewModel.ShowDeviceBarArea = PodStructUtility.Convert(appBar.rc);

			if (RestrictionViewModel.AutoHide) {
				WaitHidden();
			}

			View.Dispatcher.BeginInvoke(
				DispatcherPriority.ApplicationIdle,
				new Action(() => ResizeShowDeviceBarArea())
			);
		}

		void ResizeShowDeviceBarArea()
		{
			var deviceArea = RestrictionViewModel.ShowDeviceBarArea;
			NativeMethods.MoveWindow(Handle, (int)deviceArea.X, (int)deviceArea.Y, (int)deviceArea.Width, (int)deviceArea.Height, true);
		}

		public void DockingFromProperty()
		{
			DockingFromParameter(RestrictionViewModel.DockType, RestrictionViewModel.AutoHide);
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
			if (AutoHideTimer.IsEnabled) {
				AutoHideTimer.Stop();
			}

			// 登録済みであればいったん解除
			var needResist = true;
			if (RestrictionViewModel.IsDocking) {
				if (RestrictionViewModel.DockType != dockType || RestrictionViewModel.AutoHide) {
					UnresistAppbar();
					needResist = true;
				} else {
					needResist = false;
				}
			}
			RestrictionViewModel.IsHidden = false;

			if (dockType == DockType.None) {
				RestrictionViewModel.DockType = dockType;
				RestrictionViewModel.ChangingWindowMode();
				// NOTE: もっかしフルスクリーン通知拾えるかもなんで登録すべきかも。
				return;
			}

			// 登録
			if (needResist) {
				RegistAppbar();
			}

			DockingFromParameter(dockType, RestrictionViewModel.AutoHide);
		}

		/// <summary>
		/// 非表示状態への待ちを取りやめ。
		/// </summary>
		void StopHidden()
		{
			Debug.Assert(RestrictionViewModel.AutoHide);
			if (AutoHideTimer.IsEnabled) {
				AutoHideTimer.Stop();
			}
			ToShow();
		}

		/// <summary>
		/// 非表示状態への待ちを開始。
		/// </summary>
		void WaitHidden()
		{
			Debug.Assert(RestrictionViewModel.AutoHide);

			if (!AutoHideTimer.IsEnabled) {
				AutoHideTimer.Interval = RestrictionViewModel.HiddenWaitTime;
				AutoHideTimer.Start();
			}
		}

		/// <summary>
		/// 自動的に隠す状態から復帰
		/// </summary>
		protected virtual void ToShow()
		{
			Debug.Assert(RestrictionViewModel.DockType != DockType.None);
			Debug.Assert(RestrictionViewModel.AutoHide);

			
			//var screeanPos = DockScreen.WorkingArea.Location;
			//var screeanSize = DockScreen.WorkingArea.Size;
			//var size = ShowBarSize;
			//var pos = Location;
			//switch (DesktopDockType) {
			//	case DesktopDockType.Top:
			//		//size.Width = screeanSize.Width;
			//		//size.Height = HiddenSize.Top;
			//		pos.X = screeanPos.X;
			//		pos.Y = screeanPos.Y;
			//		break;

			//	case DesktopDockType.Bottom:
			//		//size.Width = screeanSize.Width;
			//		//size.Height = HiddenSize.Bottom;
			//		pos.X = screeanPos.X;
			//		pos.Y = screeanPos.Y + screeanSize.Height - size.Height;
			//		break;

			//	case DesktopDockType.Left:
			//		//size.Width = HiddenSize.Left;
			//		//size.Height = screeanSize.Height;
			//		pos.X = screeanPos.X;
			//		pos.Y = screeanPos.Y;
			//		break;

			//	case DesktopDockType.Right:
			//		//size.Width = HiddenSize.Right;
			//		//size.Height = screeanSize.Height;
			//		pos.X = screeanPos.X + screeanSize.Width - size.Width;
			//		pos.Y = screeanPos.Y;
			//		break;

			//	default:
			//		throw new NotImplementedException();
			//}

			//Bounds = new Rectangle(pos, size);
			//IsHidden = false;
			ResizeShowDeviceBarArea();
		}

		/// <summary>
		/// 非表示状態へ遷移。
		/// </summary>
		/// <param name="force">強制的に遷移するか。</param>
		protected void ToHidden(bool force)
		{
			Debug.Assert(RestrictionViewModel.DockType != DockType.None);
			Debug.Assert(RestrictionViewModel.AutoHide);

			AutoHideTimer.Stop();

			var deviceCursolPosition = new POINT();
			NativeMethods.GetCursorPos(out deviceCursolPosition);

			if (!force && RestrictionViewModel.ShowDeviceBarArea.Contains(PodStructUtility.Convert(deviceCursolPosition))) {
				return;
			}

			//RestrictionViewModel.HideSize
			//RestrictionViewModel.DockScreen.DeviceBounds.Location

			Size deviceSize;
			Point deviceLocation;

			switch (RestrictionViewModel.DockType) {
				case DockType.Top:
					deviceSize = new Size(RestrictionViewModel.DockScreen.DeviceBounds.Width, RestrictionViewModel.HideSize.Height);
					deviceLocation = RestrictionViewModel.DockScreen.DeviceBounds.Location;
					break;

				case DockType.Bottom:
					deviceSize = new Size(RestrictionViewModel.DockScreen.DeviceBounds.Width, RestrictionViewModel.HideSize.Height);
					deviceLocation = new Point(RestrictionViewModel.DockScreen.DeviceBounds.X, RestrictionViewModel.DockScreen.DeviceBounds.Y - deviceSize.Height);
					break;

				case DockType.Left:
					deviceSize = new Size(RestrictionViewModel.HideSize.Width, RestrictionViewModel.DockScreen.DeviceBounds.Height);
					deviceLocation = RestrictionViewModel.DockScreen.DeviceBounds.Location;
					break;

				case DockType.Right:
					deviceSize = new Size(RestrictionViewModel.HideSize.Width, RestrictionViewModel.DockScreen.DeviceBounds.Height);
					deviceLocation = new Point(RestrictionViewModel.DockScreen.DeviceBounds.Right - deviceSize.Width, RestrictionViewModel.DockScreen.DeviceBounds.Y);
					break;

				default:
					throw new NotImplementedException();
			}

			HiddenView(!force, new Rect(deviceLocation, deviceSize));
		}

		static AW ToAW(DockType type, bool show)
		{
			var result = new Dictionary<DockType, AW>() {
				{ DockType .Top,    show ? AW.AW_VER_POSITIVE: AW.AW_VER_NEGATIVE },
				{ DockType .Bottom, show ? AW.AW_VER_NEGATIVE: AW.AW_VER_POSITIVE },
				{ DockType .Left,   show ? AW.AW_HOR_POSITIVE: AW.AW_HOR_NEGATIVE },
				{ DockType .Right,  show ? AW.AW_HOR_NEGATIVE: AW.AW_HOR_POSITIVE },
			}[type];

			if (!show) {
				result |= AW.AW_HIDE;
			}

			return result;
		}

		/// <summary>
		/// 自動的に隠すの実際の処理。
		/// </summary>
		/// <param name="animation"></param>
		/// <param name="deviceHiddenArea"></param>
		protected virtual void HiddenView(bool animation, Rect deviceHiddenArea)
		{
			var prevVisibility = RestrictionViewModel.Visibility;

			if (RestrictionViewModel.Visibility == Visibility.Visible) {
				if (animation) {
					var animateTime = (int)RestrictionViewModel.HiddenAnimateTime.TotalMilliseconds;
					var animateFlag = ToAW(RestrictionViewModel.DockType, false);
					//NativeMethods.AnimateWindow(Handle, animateTime, animateFlag);
				}
				RestrictionViewModel.IsHidden = true;
				RestrictionViewModel.HideLogicalBarArea = UIUtility.ToLogicalPixel(View, deviceHiddenArea);
				
				RestrictionViewModel.Visibility = prevVisibility;
				//View.Left = deviceHiddenArea.X;
				//View.Top = deviceHiddenArea.Y;
				//View.Width = 10;
				//View.Height = deviceHiddenArea.Height;
				NativeMethods.MoveWindow(Handle, (int)deviceHiddenArea.X, (int)deviceHiddenArea.Y, (int)deviceHiddenArea.Width, (int)deviceHiddenArea.Height, true);
			}
		}

		#endregion

		void View_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue != e.OldValue) {
				var isVisible = (bool)e.NewValue;
				if (isVisible) {
					Docking(RestrictionViewModel.DockType);
				} else {
					if (RestrictionViewModel.IsDocking) {
						UnresistAppbar();
					}
				}
			}
		}

		void TimerAutoHide_Tick(object sender, EventArgs e)
		{
			if (RestrictionViewModel.IsDocking) {
				ToHidden(false);
			} else {
				AutoHideTimer.Stop();
			}
		}

	}
}
