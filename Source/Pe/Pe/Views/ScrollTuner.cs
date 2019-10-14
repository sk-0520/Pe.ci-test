using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Main.Views
{
    public class ScrollTuner: DisposerBase
    {
        public ScrollTuner(Window view)
        {
            View = view;

            if(View.IsLoaded) {
                AttachView();
            } else {
                View.SourceInitialized += View_SourceInitialized;
            }
        }

        #region property

        Window View { get; }

        #endregion

        #region function

        void AttachView()
        {
        }

        void DetachView()
        {

        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                DetachView();
            }

            base.Dispose(disposing);
        }

        #endregion

        private void View_SourceInitialized(object? sender, EventArgs e)
        {
            View.SourceInitialized -= View_SourceInitialized;
            AttachView();
        }
    }
}
