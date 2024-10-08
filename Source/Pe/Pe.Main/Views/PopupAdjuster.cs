using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.Base;

namespace ContentTypeTextNet.Pe.Main.Views
{
    /// <summary>
    /// <see cref="System.Windows.Controls.Primitives.Popup"/>をいい感じに自動で動かす。
    /// </summary>
    public sealed class PopupAdjuster: DisposerBase
    {
        public PopupAdjuster(Window window, Popup popup)
        {
            Window = window;
            Popup = popup;

            Window.LocationChanged += Window_LocationChanged!;
            Window.SizeChanged += Window_SizeChanged;
            Window.Closed += Window_Closed!;
        }

        #region property

        private Window? Window { get; set; }
        private Popup? Popup { get; set; }

        #endregion

        #region function

        private void ResetPopupPosition()
        {
            if(Popup is not null) {
                Popup.HorizontalOffset += 1;
                Popup.HorizontalOffset -= 1;
            }
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(Window is not null) {
                    Window.LocationChanged -= Window_LocationChanged!;
                    Window.SizeChanged -= Window_SizeChanged!;
                    Window.Closed -= Window_Closed!;
                }
                Window = null;
                Popup = null;
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
