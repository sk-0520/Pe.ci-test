using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Core.Models;
using ICSharpCode.AvalonEdit;

namespace ContentTypeTextNet.Pe.Main.Views
{
    /// <summary>
    /// マウスホイールでスクロールする際にいろんな要因で吸い取られるイベントを自然に処理する。
    /// <para>WPFが標準で用意してるやつは基本的に大丈夫そうだけどサードパーティ製のやつとか自前のやつとかに特化。</para>
    /// </summary>
    public class ScrollTuner : DisposerBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="view">親ウィンドウ。</param>
        /// <param name="attachOnLoaded">ロード時に存在するコントロール全てを対象にするか。</param>
        public ScrollTuner(Window view, bool attachOnLoaded)
        {
            View = view;

            AttachOnLoaded = attachOnLoaded;

            if(AttachOnLoaded) {
                if(View.IsLoaded) {
                    AttachView();
                } else {
                    View.Loaded += View_Loaded;
                }
            }
        }

        #region property

        bool AttachOnLoaded { get; }
        Window View { get; }

        IList<UIElement> Elements { get; } = new List<UIElement>();

        public int ScrollNotch { get; set; } = 120;
        public int ScrollLines { get; set; } = SystemParameters.WheelScrollLines;

        #endregion

        #region function

        void AttachView()
        {
            View.Closed += View_Closed;

            // TextEditor に対する特殊処理を追加する
            var avalonEditors = UIUtility.FindVisualChildren<TextEditor>(View);
            foreach(var avalonEditor in avalonEditors) {
                AddCore(avalonEditor);
            }
        }

        void DetachView()
        {
            View.Loaded -= View_Loaded;
            View.Closed -= View_Closed;

            foreach(var element in Elements.ToList()) {
                RemoveCore(element);
            }
        }

        void AttachAvalonEditor(TextEditor textEditor)
        {
            textEditor.PreviewMouseWheel += TextEditor_PreviewMouseWheel;
        }

        void DetachAvalonEditor(TextEditor textEditor)
        {
            textEditor.PreviewMouseWheel -= TextEditor_PreviewMouseWheel;
        }

        void AddCore(UIElement element)
        {
            Elements.Add(element);
            switch(element) {
                case TextEditor textEditor:
                    AttachAvalonEditor(textEditor);
                    break;

                default:
                    break;
            }
        }

        bool RemoveCore(UIElement element)
        {
            if(!Elements.Remove(element)) {
                return false;
            }

            switch(element) {
                case TextEditor textEditor:
                    DetachAvalonEditor(textEditor);
                    break;

                default:
                    break;
            }

            return true;
        }

        public void Add(UIElement element)
        {
            AddCore(element);
        }

        public bool Remove(UIElement element)
        {
            return RemoveCore(element);
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

        private void View_Loaded(object? sender, EventArgs e)
        {
            View.Loaded -= View_Loaded;
            AttachView();
        }
        private void View_Closed(object? sender, EventArgs e)
        {
            View.Closed -= View_Closed;
            DetachView();
        }


        private void TextEditor_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            var textEditor = (TextEditor)sender;

            var parentScrollViewer = UIUtility.GetVisualClosest<ScrollViewer>(textEditor);
            if(parentScrollViewer == null) {
                return;
            }

            var childScrollViewer = UIUtility.FindVisualChildren<ScrollViewer>(textEditor).FirstOrDefault();
            if(childScrollViewer == null) {
                return;
            }

            var scrollLineCount = (Math.Abs(e.Delta) / ScrollNotch) * ScrollLines;
            if(e.Delta > 0) { // ↑
                if(textEditor.VerticalOffset == 0) {
                    // 一番上なので親側をスクロールさせる
                    foreach(var counter in new Counter(scrollLineCount)) {
                        parentScrollViewer.LineUp();
                    }
                    e.Handled = true;
                }
            } else if(e.Delta < 0) { // ↓
                if(childScrollViewer.VerticalOffset == childScrollViewer.ScrollableHeight) {
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
