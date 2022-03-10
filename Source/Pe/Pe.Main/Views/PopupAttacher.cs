using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Main.Views
{
    public class PopupAttacher: DisposerBase
    {
        public PopupAttacher(Window window, Popup popup)
        {
            Window = window;
            Popup = popup;

            Window.LocationChanged += Window_LocationChanged!;
            Window.SizeChanged += Window_SizeChanged;
            Window.Closed += Window_Closed!;
        }

        #region property

        private Window Window { get; }
        private Popup Popup { get; }

        #endregion

        #region function

        private void ResetPopupPosition()
        {
            Popup.HorizontalOffset += 1;
            Popup.HorizontalOffset -= 1;
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Window.LocationChanged -= Window_LocationChanged!;
                Window.SizeChanged -= Window_SizeChanged!;
                Window.Closed -= Window_Closed!;
            }

            base.Dispose(disposing);
        }

        #endregion

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            ResetPopupPosition();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResetPopupPosition();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
