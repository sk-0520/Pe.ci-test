using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.View
{
    public class WindowExtendBase<TWindow, TExtendData> : ViewExtendBase<TWindow, TExtendData>
        where TWindow : Window
        where TExtendData : INotifyPropertyChanged
    {
        public WindowExtendBase(TWindow view, TExtendData extendData, ILoggerFactory loggerFactory)
            : base(view, extendData, loggerFactory)
        { }

        #region property
        #endregion

        #region function

        public IntPtr GetWindowHandle() => HandleUtility.GetWindowHandle(View);

        #endregion

        #region DisposeFinalizeBase
        #endregion

    }

    public class WndProcExtendBase<TWindow, TExtendData> : WindowExtendBase<TWindow, TExtendData>
        where TWindow : Window
        where TExtendData : INotifyPropertyChanged
    {
        public WndProcExtendBase(TWindow view, TExtendData extendData, ILoggerFactory loggerFactory)
            : base(view, extendData, loggerFactory)
        {
            if(View.IsLoaded) {
                AttachHwndSource();
            } else {
                View.SourceInitialized += View_SourceInitialized;
            }
        }

        #region property

        protected HwndSource HwndSource { get; private set; }

        #endregion

        #region function

        void AttachHwndSource()
        {
            var handle = GetWindowHandle();
            HwndSource = HwndSource.FromHwnd(handle);
            HwndSource.AddHook(WndProc);
        }

        protected virtual IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            return IntPtr.Zero;
        }

        #endregion

        #region WindowExtendBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    HwndSource?.Dispose();
                    HwndSource = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        private void View_SourceInitialized(object sender, EventArgs e)
        {
            View.SourceInitialized -= View_SourceInitialized;
            AttachHwndSource();
        }


    }

}
