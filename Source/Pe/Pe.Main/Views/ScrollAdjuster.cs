using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.Base;
using ICSharpCode.AvalonEdit;

namespace ContentTypeTextNet.Pe.Main.Views
{
    /// <summary>
    /// マウスホイールでスクロールする際にいろんな要因で吸い取られるイベントを自然に処理する。
    /// </summary>
    /// <remarks>
    /// <para>WPFが標準で用意してるやつは基本的に大丈夫そうだけどサードパーティ製のやつとか自前のやつとかに特化。</para>
    /// </remarks>
    public sealed class ScrollAdjuster: DisposerBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="view">親ウィンドウ。</param>
        public ScrollAdjuster(Window view)
        {
            View = view;
            View.PreviewMouseWheel -= View_PreviewMouseWheel;
            View.PreviewMouseWheel += View_PreviewMouseWheel;
        }

        #region property

        /// <summary>
        /// 対象ウィンドウ。
        /// </summary>
        private Window? View { get; set; }

        public int ScrollNotch { get; set; } = 120;
        //public int ScrollLines { get; set; } = SystemParameters.WheelScrollLines;

        #endregion

        #region function

        private void DetachView()
        {
            if(View is not null) {
                View.PreviewMouseWheel -= View_PreviewMouseWheel;
                View.Closed -= View_Closed;
                View = null;
            }
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

        private void View_Closed(object? sender, EventArgs e)
        {
            DetachView();
        }

        private void View_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            var element = e.OriginalSource as Visual;
            if(element == null) {
                return;
            }

            var textEditor = UIUtility.GetVisualClosest<TextEditor>(element);
            if(textEditor != null) {
                var parentScrollViewer = UIUtility.GetVisualClosest<ScrollViewer>(textEditor);
                if(parentScrollViewer == null) {
                    return;
                }

                var childScrollViewer = UIUtility.FindVisualChildren<ScrollViewer>(textEditor).FirstOrDefault();
                if(childScrollViewer == null) {
                    return;
                }

                var scrollLineCount = (Math.Abs(e.Delta) / ScrollNotch) * SystemParameters.WheelScrollLines;
                if(e.Delta > 0) { // ↑
                    if((int)textEditor.VerticalOffset == 0) {
                        // 一番上なので親側をスクロールさせる
                        foreach(var counter in new Counter(scrollLineCount)) {
                            parentScrollViewer.LineUp();
                        }
                        e.Handled = true;
                    }
                } else if(e.Delta < 0) { // ↓
                    if((int)childScrollViewer.VerticalOffset == (int)childScrollViewer.ScrollableHeight) {
                        // 一番下なので親側をスクロールさせる
                        foreach(var counter in new Counter(scrollLineCount)) {
                            parentScrollViewer.LineDown();
                        }
                        e.Handled = true;
                    }
                }
            }
        }
    }
}
