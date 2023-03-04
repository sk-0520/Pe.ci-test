using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Interop;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Views
{
    public abstract class WindowExtendBase<TWindow, TExtendData>: ViewExtendBase<TWindow, TExtendData>
        where TWindow : Window
        where TExtendData : IExtendData
    {
        protected WindowExtendBase(TWindow view, TExtendData extendData, ILoggerFactory loggerFactory)
            : base(view, extendData, loggerFactory)
        { }

        #region property
        #endregion

        #region function

        public IntPtr GetWindowHandle() => HandleUtility.GetWindowHandle(View);

        #endregion

        #region DisposerBase
        #endregion
    }

    public abstract class WndProcExtendBase<TWindow, TExtendData>: WindowExtendBase<TWindow, TExtendData>
        where TWindow : Window
        where TExtendData : IExtendData
    {
        protected WndProcExtendBase(TWindow view, TExtendData extendData, ILoggerFactory loggerFactory)
            : base(view, extendData, loggerFactory)
        {
            if(View.IsLoaded) {
                AttachHwndSource();
            } else {
                View.SourceInitialized += View_SourceInitialized;
            }
        }

        #region property

        protected bool IsEnabledWindowHandle { get; private set; }

        protected HwndSource? HwndSource { get; private set; }

        protected IntPtr WindowHandle { get; private set; }

        #endregion

        #region function

        protected virtual void InitializedWindowHandleImpl()
        { }

        private void InitializedWindowHandle()
        {
            Debug.Assert(WindowHandle.ToInt32() != 0);

            InitializedWindowHandleImpl();
        }

        private void AttachHwndSource()
        {
            WindowHandle = GetWindowHandle();
            IsEnabledWindowHandle = true;
            HwndSource = HwndSource.FromHwnd(WindowHandle);
            HwndSource.AddHook(WndProc);

            InitializedWindowHandle();
        }

        protected virtual IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if(msg == (int)WM.WM_DESTROY) {
                IsEnabledWindowHandle = false;
            }

            return IntPtr.Zero;
        }

        #endregion

        #region WindowExtendBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    HwndSource?.Dispose();
                    HwndSource = null!;
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        private void View_SourceInitialized(object? sender, EventArgs e)
        {
            View.SourceInitialized -= View_SourceInitialized;
            AttachHwndSource();
        }
    }
}
