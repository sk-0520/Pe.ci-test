using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Forms;
using ContentTypeTextNet.Pe.Library.Shared.Library.CompatibleWindows;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.View;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.View.Extend
{
    public enum AppDesktopToolbarPosition
    {
        Left,
        Top,
        Right,
        Bottom,
    }

    public static class AppDesktopToolbarPositionUtility
    {
        /// <summary>
        /// <see cref="ABE"/> へ変換。
        /// </summary>
        /// <param name="toolbarPosition"></param>
        /// <returns></returns>
        public static ABE ToABE(AppDesktopToolbarPosition toolbarPosition)
        {
            switch(toolbarPosition) {
                case AppDesktopToolbarPosition.Left: return ABE.ABE_LEFT;
                case AppDesktopToolbarPosition.Top: return ABE.ABE_TOP;
                case AppDesktopToolbarPosition.Bottom: return ABE.ABE_BOTTOM;
                case AppDesktopToolbarPosition.Right: return ABE.ABE_RIGHT;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// <see cref="AppDesktopToolbarPosition"/> へ変換。
        /// </summary>
        /// <param name="abe"></param>
        /// <returns></returns>
        public static AppDesktopToolbarPosition ToToolbarPosition(ABE abe)
        {
            switch(abe) {
                case ABE.ABE_LEFT: return AppDesktopToolbarPosition.Left;
                case ABE.ABE_TOP: return AppDesktopToolbarPosition.Top;
                case ABE.ABE_RIGHT: return AppDesktopToolbarPosition.Right;
                case ABE.ABE_BOTTOM: return AppDesktopToolbarPosition.Bottom;
                default:
                    throw new NotImplementedException();
            }
        }
    }

    public interface IAppDesktopToolbarExtendData : IExtendData
    {
        #region property

        /// <summary>
        /// ツールバー位置。
        /// </summary>
        AppDesktopToolbarPosition ToolbarPosition { get; set; }
        /// <summary>
        /// ドッキング中か。
        /// </summary>
        bool IsDocking { get; set; }
        /// <summary>
        /// 自動的に隠すか。
        /// </summary>
        bool IsAutoHide { get; set; }
        /// <summary>
        /// 隠れているか。
        /// </summary>
        bool IsHiding { get; set; }
        /// <summary>
        /// 自動的に隠れるまでの時間。
        /// </summary>
        TimeSpan AutoHideTimeout { get; }

        /// <summary>
        /// 表示中のサイズ。
        /// <para><see cref="AppDesktopToolbarPosition"/>の各辺に対応</para>
        /// </summary>
        [PixelKind(Px.Logical)]
        Size DisplaySize { get; set; }
        /// <summary>
        /// 表示中の論理バーサイズ。
        /// <para><see cref="AppDesktopToolbarExtend"/>で設定されるためユーザーコードで変更は行わないこと。</para>
        /// </summary>
        [PixelKind(Px.Logical)]
        Rect DisplayBarArea { get; set; }

        /// <summary>
        /// 隠れた状態のバー論理サイズ。
        /// <para><see cref="AppDesktopToolbarPosition"/>の各辺に対応</para>
        /// </summary>
        [PixelKind(Px.Logical)]
        Size HiddenSize { get; }
        /// <summary>
        /// 表示中の隠れたバーの論理領域。
        /// <para><see cref="AppDesktopToolbarExtend"/>で設定されるためユーザーコードで変更は行わないこと。</para>
        /// </summary>
        [PixelKind(Px.Logical)]
        Rect HiddenBarArea { get; set; }


        /// <summary>
        /// フルスクリーンウィンドウが存在するか。
        /// </summary>
        bool ExistsFullScreenWindow { get; set; }

        /// <summary>
        /// 対象ディスプレイ。
        /// </summary>
        Screen DockScreen { get; }

        #endregion
    }

    public abstract class AppDesktopToolbarEventArgs : EventArgs
    { }

    public class AppDesktopToolbarFullScreenEventArgs : AppDesktopToolbarEventArgs
    {
        public AppDesktopToolbarFullScreenEventArgs(bool fullScreen)
        {
            FullScreen = fullScreen;
        }

        public bool FullScreen { get; private set; }
        public bool Handled { get; set; }
    }

    public class AppDesktopToolbarPositionChangedEventArgs : AppDesktopToolbarEventArgs
    { }

    public class AppDesktopToolbarStateChangeEventArgs : AppDesktopToolbarEventArgs
    { }

    public class AppDesktopToolbarExtend : WndProcExtendBase<Window, IAppDesktopToolbarExtendData>
    {
        #region event

        /// <summary>
        /// フルスクリーンイベント。
        /// </summary>
        public event EventHandler<AppDesktopToolbarFullScreenEventArgs> AppDesktopToolbarFullScreen;
        /// <summary>
        /// 位置変更時に発生。
        /// </summary>
        public event EventHandler<AppDesktopToolbarPositionChangedEventArgs> AppDesktopToolbarPositionChanged;
        /// <summary>
        /// ステータス変更。
        /// </summary>
        public event EventHandler<AppDesktopToolbarStateChangeEventArgs> AppDesktopToolbarStateChange;

        #endregion

        public AppDesktopToolbarExtend(Window view, IAppDesktopToolbarExtendData extendData, ILoggerFactory loggerFactory)
            : base(view, extendData, loggerFactory)
        {
            View.MouseEnter += View_MouseEnter;
            View.MouseLeave += View_MouseLeave;

            AutoHideTimer = new DispatcherTimer();
            AutoHideTimer.Tick += TimerAutoHide_Tick;
        }

        #region property

        string MessageString { get { return "appbar"; } }
        uint CallbackMessage { get; set; }
        DispatcherTimer AutoHideTimer { get; set; }
        DispatcherOperation DockingDispatcherOperation { get; set; }
        DispatcherOperation HiddenDispatcherOperation { get; set; }

        bool NowWorking { get; set; }

        ISet<string> DockingTriggerPropertyNames { get; } = new HashSet<string>(new[] {
            nameof(IAppDesktopToolbarExtendData.IsAutoHide),
            nameof(IAppDesktopToolbarExtendData.ToolbarPosition),
        });

        #endregion


        #region function

        protected void OnAppDesktopToolbarFullScreen(bool fullScreen)
        {
            Logger.Trace($"{nameof(fullScreen)}: {fullScreen}");

            if(ExtendData != null) {
                ExtendData.ExistsFullScreenWindow = fullScreen;
            }

            var _ignore_ = false;
            if(_ignore_) {
                if(AppDesktopToolbarFullScreen != null) {
                    var e = new AppDesktopToolbarFullScreenEventArgs(fullScreen);
                    AppDesktopToolbarFullScreen(View, e);
                }
            }
        }

        protected virtual void OnAppDesktopToolbarPositionChanged()
        {
            if(AppDesktopToolbarPositionChanged != null) {
                var e = new AppDesktopToolbarPositionChangedEventArgs();
                AppDesktopToolbarPositionChanged(View, e);
            }
        }

        protected virtual void OnAppDesktopToolbarStateChange()
        {
            DockingFromProperty();

            if(AppDesktopToolbarStateChange != null) {
                var e = new AppDesktopToolbarStateChangeEventArgs();
                AppDesktopToolbarStateChange(View, e);
            }
        }

        void OnResizeEnd()
        {
            // AppBar のサイズを更新。
            switch(ExtendData.ToolbarPosition) {
                case AppDesktopToolbarPosition.Left:
                case AppDesktopToolbarPosition.Right:
                    ExtendData.DisplaySize = new Size(View.Width, ExtendData.DisplaySize.Height);
                    break;

                case AppDesktopToolbarPosition.Top:
                case AppDesktopToolbarPosition.Bottom:
                    ExtendData.DisplaySize = new Size(ExtendData.DisplaySize.Width, View.Height);
                    break;

                default:
                    throw new NotImplementedException();
            }

            DockingFromProperty();
        }

        bool RegisterAppbar()
        {
            Debug.Assert(CallbackMessage == 0);

            CallbackMessage = NativeMethods.RegisterWindowMessage(MessageString);

            var appBar = new APPBARDATA(WindowHandle) {
                uCallbackMessage = CallbackMessage
            };

            var registResult = NativeMethods.SHAppBarMessage(ABM.ABM_NEW, ref appBar);

            return ExtendData.IsDocking = registResult.ToInt32() != 0;
        }

        public bool UnregisterAppbar()
        {
            if(DockingDispatcherOperation != null) {
                DockingDispatcherOperation.Abort();
                DockingDispatcherOperation = null;
            }
            var appBar = new APPBARDATA(WindowHandle);
            var unregistResult = NativeMethods.SHAppBarMessage(ABM.ABM_REMOVE, ref appBar);

            CallbackMessage = 0;
            ExtendData.IsDocking = false;

            return unregistResult.ToInt32() != 0;
        }

        /// <summary>
        /// 設定値からバー領域取得
        /// </summary>
        /// <param name="dockType"></param>
        /// <returns></returns>
        [return: PixelKind(Px.Device)]
        Rect CalcWantBarArea(AppDesktopToolbarPosition dockType)
        {
            var deviceDesktopArea = ExtendData.DockScreen.DeviceBounds;

            var deviceBarSize = UIUtility.ToDevicePixel(View, ExtendData.DisplaySize);

            double top, left, width, height;

            // 設定値からバー領域取得
            if(dockType == AppDesktopToolbarPosition.Left || dockType == AppDesktopToolbarPosition.Right) {
                top = deviceDesktopArea.Top;
                width = deviceBarSize.Width;
                height = deviceDesktopArea.Height;
                if(dockType == AppDesktopToolbarPosition.Left) {
                    left = deviceDesktopArea.Left;
                } else {
                    left = deviceDesktopArea.Right - width;
                }
            } else {
                Debug.Assert(dockType == AppDesktopToolbarPosition.Top || dockType == AppDesktopToolbarPosition.Bottom);
                left = deviceDesktopArea.Left;
                width = deviceDesktopArea.Width;
                height = deviceBarSize.Height;
                if(dockType == AppDesktopToolbarPosition.Top) {
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
            var deviceBarSize = UIUtility.ToDevicePixel(View, ExtendData.DisplaySize);
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

        public IntPtr ExistsHideWindow(AppDesktopToolbarPosition dockType)
        {
            var appBar = new APPBARDATA(WindowHandle) {
                uEdge = AppDesktopToolbarPositionUtility.ToABE(dockType)
            };
            var nowWnd = NativeMethods.SHAppBarMessage(ABM.ABM_GETAUTOHIDEBAR, ref appBar);

            return nowWnd;
        }

        void DockingFromParameter(AppDesktopToolbarPosition dockType, bool autoHide)
        {
            ExtendData.ToolbarPosition = dockType;

            var appBar = new APPBARDATA(WindowHandle) {
                uEdge = AppDesktopToolbarPositionUtility.ToABE(dockType),
                rc = PodStructUtility.Convert(CalcWantBarArea(dockType))
            };
            TuneSystemBarArea(ref appBar);

            bool autoHideResult = false;
            if(autoHide) {
                var hideWnd = ExistsHideWindow(dockType);
                if(hideWnd == IntPtr.Zero || hideWnd == WindowHandle) {
                    // 自動的に隠す
                    var result = NativeMethods.SHAppBarMessage(ABM.ABM_SETAUTOHIDEBAR, ref appBar);
                    autoHideResult = result.ToInt32() != 0;
                    autoHideResult = true;
                }
            }

            ExtendData.IsAutoHide = autoHideResult;

            var deviceWindowBounds = PodStructUtility.Convert(appBar.rc);
            var logicalWindowBounds = UIUtility.ToLogicalPixel(View, deviceWindowBounds);

            if(!autoHideResult) {
                var appbarResult = NativeMethods.SHAppBarMessage(ABM.ABM_SETPOS, ref appBar);
            }

            NativeMethods.MoveWindow(WindowHandle, appBar.rc.X, appBar.rc.Y, appBar.rc.Width, appBar.rc.Height, false);
            ExtendData.DisplayBarArea = logicalWindowBounds;//PodStructUtility.Convert(appBar.rc);

            if(ExtendData.IsAutoHide) {
                StartHideWait();
            }

            DockingDispatcherOperation = View.Dispatcher.BeginInvoke(
                new Action(ResizeShowDeviceBarArea),
                DispatcherPriority.ApplicationIdle
            );
        }

        void ResizeShowDeviceBarArea()
        {
            //if(View != null && ExtendData != null) {
            if(IsEnabledWindowHandle) {
                var deviceArea = UIUtility.ToDevicePixel(View, ExtendData.DisplayBarArea);
                NativeMethods.MoveWindow(WindowHandle, (int)deviceArea.X, (int)deviceArea.Y, (int)deviceArea.Width, (int)deviceArea.Height, true);
                ExtendData.IsHiding = false;
                WindowsUtility.ShowNoActiveForeground(WindowHandle);
            }
        }

        public void DockingFromProperty()
        {
            DockingFromParameter(ExtendData.ToolbarPosition, ExtendData.IsAutoHide);
        }

        /// <summary>
        /// ドッキングの実行
        ///
        /// すでにドッキングされている場合はドッキングを再度実行する
        /// </summary>
        public void Docking(AppDesktopToolbarPosition toolbarPosition, bool autoHide)
        {
            //if(this.timerAutoHidden.Enabled) {
            //	this.timerAutoHidden.Stop();
            //}
            if(AutoHideTimer.IsEnabled) {
                AutoHideTimer.Stop();
            }

            // 登録済みであればいったん解除
            var needResistor = true;
            if(ExtendData.IsDocking) {
                if(ExtendData.ToolbarPosition != toolbarPosition || ExtendData.IsAutoHide) {
                    UnregisterAppbar();
                    needResistor = true;
                } else {
                    needResistor = false;
                }
            }
            ExtendData.IsHiding = false;

            //if(toolbarPosition == DockType.None) {
            //    //ExtendData.DockType = dockType;
            //    //ExtendData.DockType = dockType;
            //    ExtendData.ChangingWindowMode(toolbarPosition);
            //    // NOTE: もっかしフルスクリーン通知拾えるかもなんで登録すべきかも。
            //    return;
            //}

            // 登録
            if(needResistor) {
                RegisterAppbar();
            }

            DockingFromParameter(toolbarPosition, autoHide);
        }

        /// <summary>
        /// 非表示状態への待ちを取りやめ。
        /// </summary>
        void StopHideWait()
        {
            Debug.Assert(ExtendData.IsAutoHide);
            if(AutoHideTimer.IsEnabled) {
                AutoHideTimer.Stop();
            }
            if(ExtendData.IsDocking) {
                AutoHideToShow();
            }
        }

        /// <summary>
        /// 非表示状態への待ちを開始。
        /// </summary>
        void StartHideWait()
        {
            Debug.Assert(ExtendData.IsAutoHide);

            if(!AutoHideTimer.IsEnabled) {
                AutoHideTimer.Interval = ExtendData.AutoHideTimeout;
                AutoHideTimer.Start();
            }
        }

        /// <summary>
        /// 自動的に隠す状態から復帰
        /// </summary>
        protected virtual void AutoHideToShow()
        {
            //Debug.Assert(ExtendData.DockType != DockType.None);
            Debug.Assert(ExtendData.IsAutoHide);


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
            //ExtendData.IsHidden = false;
            ResizeShowDeviceBarArea();
        }

        /// <summary>
        /// 非表示状態へ遷移。
        /// </summary>
        /// <param name="force">強制的に遷移するか。</param>
        public void HideView(bool force)
        {
            //if(ExtendData.DockType == DockType.None) {
            //    return;
            //}
            if(!ExtendData.IsAutoHide) {
                return;
            }

            AutoHideTimer.Stop();

            //var deviceCursolPosition = new POINT();
            //NativeMethods.GetCursorPos(out deviceCursolPosition);
            //var logicalCursolPosition = UIUtility.ToLogicalPixel(View, PodStructUtility.Convert(deviceCursolPosition));
            var deviceCursolPosition = MouseUtility.GetDevicePosition();
            var logicalCursolPosition = UIUtility.ToLogicalPixel(View, deviceCursolPosition);

            if(!force && ExtendData.DisplayBarArea.Contains(logicalCursolPosition)) {
                return;
            }

            //ExtendData.HideSize
            //ExtendData.DockScreen.DeviceBounds.Location

            var logicalScreenArea = UIUtility.ToLogicalPixel(View, ExtendData.DockScreen.DeviceBounds);
            Rect logicalHideArea = new Rect();

            switch(ExtendData.ToolbarPosition) {
                case AppDesktopToolbarPosition.Top:
                    logicalHideArea.Width = logicalScreenArea.Width;
                    logicalHideArea.Height = ExtendData.HiddenSize.Height;
                    logicalHideArea.X = logicalScreenArea.X;
                    logicalHideArea.Y = logicalScreenArea.Y;
                    break;

                case AppDesktopToolbarPosition.Bottom:
                    logicalHideArea.Width = logicalScreenArea.Width;
                    logicalHideArea.Height = ExtendData.HiddenSize.Height;
                    logicalHideArea.X = logicalScreenArea.X;
                    logicalHideArea.Y = logicalScreenArea.Height - logicalHideArea.Height;
                    break;

                case AppDesktopToolbarPosition.Left:
                    logicalHideArea.Width = ExtendData.HiddenSize.Width;
                    logicalHideArea.Height = logicalScreenArea.Height;
                    logicalHideArea.X = logicalScreenArea.X;
                    logicalHideArea.Y = logicalScreenArea.Y;
                    break;

                case AppDesktopToolbarPosition.Right:
                    logicalHideArea.Width = ExtendData.HiddenSize.Width;
                    logicalHideArea.Height = logicalScreenArea.Height;
                    logicalHideArea.X = logicalScreenArea.Right - logicalHideArea.Width;
                    logicalHideArea.Y = logicalScreenArea.Y;
                    break;

                default:
                    throw new NotImplementedException();
            }

            ExtendData.IsHiding = true;
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
            Debug.Assert(ExtendData.IsAutoHide);

            var prevVisibility = View.Visibility;

            if(prevVisibility == Visibility.Visible) {

                ExtendData.IsHiding = true;
                ExtendData.HiddenBarArea = UIUtility.ToLogicalPixel(View, logicalHideArea);

                var deviceHideArea = UIUtility.ToDevicePixel(View, logicalHideArea);
                if(animation) {
                    //var animateTime = (int)ExtendData.HiddenAnimateTime.TotalMilliseconds;
                    //var animateFlag = ToAW(ExtendData.DockType, false);
                    //NativeMethods.AnimateWindow(Handle, animateTime, animateFlag);
                    // TODO: アニメーションしねぇ…
                    //NativeMethods.MoveWindow(Handle, (int)deviceHideArea.X, (int)deviceHideArea.Y, (int)deviceHideArea.Width, (int)deviceHideArea.Height, true);
                } else {
                    //NativeMethods.MoveWindow(Handle, (int)deviceHideArea.X, (int)deviceHideArea.Y, (int)deviceHideArea.Width, (int)deviceHideArea.Height, true);
                }
                NativeMethods.MoveWindow(WindowHandle, (int)deviceHideArea.X, (int)deviceHideArea.Y, (int)deviceHideArea.Width, (int)deviceHideArea.Height, true);
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
                if(disposing) {
                    View.IsVisibleChanged -= View_IsVisibleChanged;
                    View.MouseEnter -= View_MouseEnter;
                    View.MouseLeave -= View_MouseLeave;
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region WindowsViewExtendBase

        protected override void ChangedExtendProperty(string propertyName)
        {
            if(NowWorking) {
                return;
            }
            if(View.IsVisible) {
                return;
            }

            if(DockingTriggerPropertyNames.Contains(propertyName)) {
                NowWorking = true;
                try {
                    DockingFromProperty();
                } finally {
                    NowWorking = false;
                }
            }
        }

        protected override void InitializedWindowHandleImpl()
        {
            base.InitializedWindowHandleImpl();

            View.IsVisibleChanged += View_IsVisibleChanged;

            if(View.IsVisible) {
                Docking(ExtendData.ToolbarPosition, ExtendData.IsAutoHide);
            }

        }

        protected override IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if(ExtendData.IsDocking) {
                switch((int)msg) {
                    case (int)WM.WM_DESTROY: {
                            UnregisterAppbar();
                            handled = true;
                        }
                        break;

                    case (int)WM.WM_ACTIVATE: {
                            var appBar = new APPBARDATA(hWnd);
                            NativeMethods.SHAppBarMessage(ABM.ABM_ACTIVATE, ref appBar);
                        }
                        break;

                    case (int)WM.WM_WINDOWPOSCHANGED: {
                            //DockingFromProperty();
                            var appBar = new APPBARDATA(hWnd);
                            NativeMethods.SHAppBarMessage(ABM.ABM_WINDOWPOSCHANGED, ref appBar);
                        }
                        break;

                    case (int)WM.WM_EXITSIZEMOVE: {
                            // 到達した試しがない
                            OnResizeEnd();
                        }
                        break;

                    default:
                        if(CallbackMessage != 0 && msg == CallbackMessage) {
                            switch(wParam.ToInt32()) {
                                case (int)ABN.ABN_FULLSCREENAPP:
                                    // フルスクリーン
                                    OnAppDesktopToolbarFullScreen(WindowsUtility.ConvertBoolFromLParam(lParam));
                                    //return IntPtr.Zero;
                                    break;

                                case (int)ABN.ABN_POSCHANGED:
                                    // 他のバーの位置が変更されたので再設定
                                    DockingFromProperty();
                                    OnAppDesktopToolbarPositionChanged();
                                    break;

                                case (int)ABN.ABN_STATECHANGE:
                                    // タスクバーの [常に手前に表示] または [自動的に隠す] が変化したとき
                                    // 特に何もする必要なし
                                    DockingFromProperty();
                                    OnAppDesktopToolbarStateChange();
                                    break;

                                default:
                                    break;
                            }
                        }
                        break;
                }
            }

            return base.WndProc(hWnd, msg, wParam, lParam, ref handled);
        }

        #endregion

        void View_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(!IsEnabledWindowHandle) {
                return;
            }

            if(e.NewValue != e.OldValue) {
                var isVisible = (bool)e.NewValue;
                if(isVisible) {
                    Docking(ExtendData.ToolbarPosition, ExtendData.IsAutoHide);
                } else {
                    if(ExtendData.IsDocking) {
                        UnregisterAppbar();
                    }
                }
            }
        }

        void TimerAutoHide_Tick(object sender, EventArgs e)
        {
            if(!IsEnabledWindowHandle) {
                return;
            }

            if(ExtendData.IsDocking) {
                HideView(false);
            } else {
                AutoHideTimer.Stop();
            }
        }

        void View_MouseEnter(object sender, MouseEventArgs e)
        {
            if(ExtendData.IsAutoHide) {
                StopHideWait();
            }
        }

        void View_MouseLeave(object sender, MouseEventArgs e)
        {
            if(ExtendData.IsAutoHide) {
                StartHideWait();
            }
        }
    }
}
