/*
This file is part of SharedLibrary.

SharedLibrary is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharedLibrary is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SharedLibrary.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.Event;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.IF.WindowsViewExtend;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility.UI;

namespace ContentTypeTextNet.Library.SharedLibrary.View.ViewExtend
{
    /// <summary>
    /// Windowにアプリケーションデスクトップツールバー機能を付与する。
    /// </summary>
    public class ApplicationDesktopToolbar: WindowsViewExtendBase<IApplicationDesktopToolbarData>
    {
        #region event

        ///// <summary>
        ///// フルスクリーンイベント。
        ///// </summary>
        //public event EventHandler<AppbarFullScreenEventArgs> AppbarFullScreen = delegate { };
        /// <summary>
        /// 位置変更時に発生。
        /// </summary>
        public event EventHandler<AppbarPosChangedEventArgs> AppbarPosChanged = delegate { };
        /// <summary>
        /// ステータス変更。
        /// </summary>
        public event EventHandler<AppbarStateChangeEventArgs> AppbarStateChange = delegate { };

        #endregion

        public ApplicationDesktopToolbar(System.Windows.Window view, IApplicationDesktopToolbarData restrictionViewModel, INonProcess nonProcess)
            : base(view, restrictionViewModel, nonProcess)
        {
            View.IsVisibleChanged += View_IsVisibleChanged;
            View.MouseEnter += View_MouseEnter;
            View.MouseLeave += View_MouseLeave;

            AutoHideTimer = new DispatcherTimer();
            AutoHideTimer.Tick += TimerAutoHide_Tick;

            if(view.Visibility == Visibility.Visible) {
                Docking(RestrictionViewModel.DockType, RestrictionViewModel.AutoHide);
            }
        }

        #region property

        protected virtual string MessageString { get { return "appbar"; } }
        protected virtual DispatcherTimer AutoHideTimer { get; private set; }
        protected DispatcherOperation DockingDispatcherOperation { get; set; }
        protected DispatcherOperation HiddenDispatcherOperation { get; set; }

        #endregion

        #region function

        protected void OnAppbarFullScreen(bool fullScreen)
        {
            //var e = new AppbarFullScreenEventArgs(fullScreen);
            //RestrictionViewModel.NowFullScreen = e.FullScreen;
            RestrictionViewModel.NowFullScreen = fullScreen;
            //e.Handled = true;
            //AppbarFullScreen(View, e);
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
            switch(RestrictionViewModel.DockType) {
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
            if(DockingDispatcherOperation != null) {
                DockingDispatcherOperation.Abort();
                DockingDispatcherOperation = null;
            }
            var appBar = new APPBARDATA(Handle);
            var unregistResult = NativeMethods.SHAppBarMessage(ABM.ABM_REMOVE, ref appBar);
            RestrictionViewModel.CallbackMessage = 0;
            RestrictionViewModel.IsDocking = false;
            return unregistResult.ToInt32() != 0;
        }

        /// <summary>
        /// 設定値からバー領域取得
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
            var deviceBarSize = UIUtility.ToDevicePixel(View, RestrictionViewModel.BarSize);
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
            appBar.uEdge = DockTypeUtility.ToABE(dockType);
            var nowWnd = NativeMethods.SHAppBarMessage(ABM.ABM_GETAUTOHIDEBAR, ref appBar);

            return nowWnd;
        }

        void DockingFromParameter(DockType dockType, bool autoHide)
        {
            Debug.Assert(dockType != DockType.None);

            RestrictionViewModel.DockType = dockType;

            var appBar = new APPBARDATA(Handle);
            appBar.uEdge = DockTypeUtility.ToABE(dockType);
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

            RestrictionViewModel.AutoHide = autoHideResult;

            var deviceWindowBounds = PodStructUtility.Convert(appBar.rc);
            var logicalWindowBounds = UIUtility.ToLogicalPixel(View, deviceWindowBounds);

            if(!autoHideResult) {
                var appbarResult = NativeMethods.SHAppBarMessage(ABM.ABM_SETPOS, ref appBar);
            }

            NativeMethods.MoveWindow(Handle, appBar.rc.X, appBar.rc.Y, appBar.rc.Width, appBar.rc.Height, false);
            RestrictionViewModel.ShowLogicalBarArea = logicalWindowBounds;//PodStructUtility.Convert(appBar.rc);

            if(RestrictionViewModel.AutoHide) {
                StartHideWait();
            }

            DockingDispatcherOperation = View.Dispatcher.BeginInvoke(
                new Action(ResizeShowDeviceBarArea),
                DispatcherPriority.ApplicationIdle
            );
        }

        void ResizeShowDeviceBarArea()
        {
            if(View != null && RestrictionViewModel != null) {
                var deviceArea = UIUtility.ToDevicePixel(View, RestrictionViewModel.ShowLogicalBarArea);
                NativeMethods.MoveWindow(Handle, (int)deviceArea.X, (int)deviceArea.Y, (int)deviceArea.Width, (int)deviceArea.Height, true);
                RestrictionViewModel.IsHidden = false;
                WindowsUtility.ShowNoActiveForeground(Handle);
            }
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
        public void Docking(DockType dockType, bool autoHide)
        {
            //if(this.timerAutoHidden.Enabled) {
            //	this.timerAutoHidden.Stop();
            //}
            if(AutoHideTimer.IsEnabled) {
                AutoHideTimer.Stop();
            }

            // 登録済みであればいったん解除
            var needResist = true;
            if(RestrictionViewModel.IsDocking) {
                if(RestrictionViewModel.DockType != dockType || RestrictionViewModel.AutoHide) {
                    UnresistAppbar();
                    needResist = true;
                } else {
                    needResist = false;
                }
            }
            RestrictionViewModel.IsHidden = false;

            if(dockType == DockType.None) {
                //RestrictionViewModel.DockType = dockType;
                //RestrictionViewModel.DockType = dockType;
                RestrictionViewModel.ChangingWindowMode(dockType);
                // NOTE: もっかしフルスクリーン通知拾えるかもなんで登録すべきかも。
                return;
            }

            // 登録
            if(needResist) {
                RegistAppbar();
            }

            DockingFromParameter(dockType, autoHide);
        }

        /// <summary>
        /// 非表示状態への待ちを取りやめ。
        /// </summary>
        void StopHideWait()
        {
            Debug.Assert(RestrictionViewModel.AutoHide);
            if(AutoHideTimer.IsEnabled) {
                AutoHideTimer.Stop();
            }
            if(RestrictionViewModel.IsDocking) {
                AutoHideToShow();
            }
        }

        /// <summary>
        /// 非表示状態への待ちを開始。
        /// </summary>
        void StartHideWait()
        {
            Debug.Assert(RestrictionViewModel.AutoHide);

            if(!AutoHideTimer.IsEnabled) {
                AutoHideTimer.Interval = RestrictionViewModel.HideWaitTime;
                AutoHideTimer.Start();
            }
        }

        /// <summary>
        /// 自動的に隠す状態から復帰
        /// </summary>
        protected virtual void AutoHideToShow()
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
            //RestrictionViewModel.IsHidden = false;
            ResizeShowDeviceBarArea();
        }

        /// <summary>
        /// 非表示状態へ遷移。
        /// </summary>
        /// <param name="force">強制的に遷移するか。</param>
        public void HideView(bool force)
        {
            if(RestrictionViewModel.DockType == DockType.None) {
                return;
            }
            if(!RestrictionViewModel.AutoHide) {
                return;
            }

            AutoHideTimer.Stop();

            //var deviceCursolPosition = new POINT();
            //NativeMethods.GetCursorPos(out deviceCursolPosition);
            //var logicalCursolPosition = UIUtility.ToLogicalPixel(View, PodStructUtility.Convert(deviceCursolPosition));
            var deviceCursolPosition = MouseUtility.GetDevicePosition();
            var logicalCursolPosition = UIUtility.ToLogicalPixel(View, deviceCursolPosition);

            if(!force && RestrictionViewModel.ShowLogicalBarArea.Contains(logicalCursolPosition)) {
                return;
            }

            //RestrictionViewModel.HideSize
            //RestrictionViewModel.DockScreen.DeviceBounds.Location

            var logicalScreenArea = UIUtility.ToLogicalPixel(View, RestrictionViewModel.DockScreen.DeviceBounds);
            Rect logicalHideArea = new Rect();

            switch(RestrictionViewModel.DockType) {
                case DockType.Top:
                    logicalHideArea.Width = logicalScreenArea.Width;
                    logicalHideArea.Height = RestrictionViewModel.HideWidth;
                    logicalHideArea.X = logicalScreenArea.X;
                    logicalHideArea.Y = logicalScreenArea.Y;
                    break;

                case DockType.Bottom:
                    logicalHideArea.Width = logicalScreenArea.Width;
                    logicalHideArea.Height = RestrictionViewModel.HideWidth;
                    logicalHideArea.X = logicalScreenArea.X;
                    logicalHideArea.Y = logicalScreenArea.Height - logicalHideArea.Height;
                    break;

                case DockType.Left:
                    logicalHideArea.Width = RestrictionViewModel.HideWidth;
                    logicalHideArea.Height = logicalScreenArea.Height;
                    logicalHideArea.X = logicalScreenArea.X;
                    logicalHideArea.Y = logicalScreenArea.Y;
                    break;

                case DockType.Right:
                    logicalHideArea.Width = RestrictionViewModel.HideWidth;
                    logicalHideArea.Height = logicalScreenArea.Height;
                    logicalHideArea.X = logicalScreenArea.Right - logicalHideArea.Width;
                    logicalHideArea.Y = logicalScreenArea.Y;
                    break;

                default:
                    throw new NotImplementedException();
            }

            RestrictionViewModel.IsHidden = true;
            HideViewCore(!force, logicalHideArea);
        }

        //static AW ToAW(DockType type, bool show)
        //{
        //	var result = new Dictionary<DockType, AW>() {
        //		{ DockType .Top,    show ? AW.AW_VER_POSITIVE: AW.AW_VER_NEGATIVE },
        //		{ DockType .Bottom, show ? AW.AW_VER_NEGATIVE: AW.AW_VER_POSITIVE },
        //		{ DockType .Left,   show ? AW.AW_HOR_POSITIVE: AW.AW_HOR_NEGATIVE },
        //		{ DockType .Right,  show ? AW.AW_HOR_NEGATIVE: AW.AW_HOR_POSITIVE },
        //	}[type];

        //	if (!show) {
        //		result |= AW.AW_HIDE;
        //	}

        //	return result;
        //}

        /// <summary>
        /// 自動的に隠すの実際の処理。
        /// </summary>
        /// <param name="animation"></param>
        /// <param name="logicalHideArea"></param>
        protected virtual void HideViewCore(bool animation, Rect logicalHideArea)
        {
            Debug.Assert(RestrictionViewModel.DockType != DockType.None);
            Debug.Assert(RestrictionViewModel.AutoHide);

            var prevVisibility = RestrictionViewModel.Visibility;

            if(RestrictionViewModel.Visibility == Visibility.Visible) {

                RestrictionViewModel.IsHidden = true;
                RestrictionViewModel.HideLogicalBarArea = UIUtility.ToLogicalPixel(View, logicalHideArea);

                var deviceHideArea = UIUtility.ToDevicePixel(View, logicalHideArea);
                if(animation) {
                    //var animateTime = (int)RestrictionViewModel.HiddenAnimateTime.TotalMilliseconds;
                    //var animateFlag = ToAW(RestrictionViewModel.DockType, false);
                    //NativeMethods.AnimateWindow(Handle, animateTime, animateFlag);
                    // TODO: アニメーションしねぇ…
                    //NativeMethods.MoveWindow(Handle, (int)deviceHideArea.X, (int)deviceHideArea.Y, (int)deviceHideArea.Width, (int)deviceHideArea.Height, true);
                } else {
                    //NativeMethods.MoveWindow(Handle, (int)deviceHideArea.X, (int)deviceHideArea.Y, (int)deviceHideArea.Width, (int)deviceHideArea.Height, true);
                }
                NativeMethods.MoveWindow(Handle, (int)deviceHideArea.X, (int)deviceHideArea.Y, (int)deviceHideArea.Width, (int)deviceHideArea.Height, true);
                //View.Width = logicalHideArea.Width;
                //View.Measure(logicalHideArea.Size);
                //View.Arrange(new Rect(0, 0, logicalHideArea.Width, logicalHideArea.Height));
            }
        }

        #endregion


        #region DisposeFinalizeBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                View.IsVisibleChanged -= View_IsVisibleChanged;
                View.MouseEnter -= View_MouseEnter;
                View.MouseLeave -= View_MouseLeave;
            }
            base.Dispose(disposing);
        }

        #endregion

        #region WindowsViewExtendBase

        public override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if(RestrictionViewModel.IsDocking) {
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

                    case (int)WM.WM_EXITSIZEMOVE:
                        {
                            // 到達した試しがない
                            OnResizeEnd();
                        }
                        break;

                    default:
                        if(RestrictionViewModel.CallbackMessage != 0 && msg == RestrictionViewModel.CallbackMessage) {
                            switch(wParam.ToInt32()) {
                                case (int)ABN.ABN_FULLSCREENAPP:
                                    // フルスクリーン
                                    OnAppbarFullScreen(WindowsUtility.ConvertBoolFromLParam(lParam));
                                    //return IntPtr.Zero;
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

        void View_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(e.NewValue != e.OldValue) {
                var isVisible = (bool)e.NewValue;
                if(isVisible) {
                    Docking(RestrictionViewModel.DockType, RestrictionViewModel.AutoHide);
                } else {
                    if(RestrictionViewModel.IsDocking) {
                        UnresistAppbar();
                    }
                }
            }
        }

        void TimerAutoHide_Tick(object sender, EventArgs e)
        {
            if(View != null && RestrictionViewModel != null) {
                if(RestrictionViewModel.IsDocking) {
                    HideView(false);
                } else {
                    AutoHideTimer.Stop();
                }
            }
        }

        void View_MouseEnter(object sender, MouseEventArgs e)
        {
            if(RestrictionViewModel.AutoHide) {
                StopHideWait();
            }
        }

        void View_MouseLeave(object sender, MouseEventArgs e)
        {
            if(RestrictionViewModel.AutoHide) {
                StartHideWait();
            }
        }
    }
}
