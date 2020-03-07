using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.Note;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Note;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Note
{
    public class NoteRichTextContentViewModel : NoteContentViewModelBase, IFlushable
    {
        #region variable

        bool _isOpenToolbar;

        #endregion

        public NoteRichTextContentViewModel(NoteContentElement model, IClipboardManager clipboardManager, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, clipboardManager, dispatcherWrapper, loggerFactory)
        {
            TextChangeLazyAction = new LazyAction("RTF変更抑制", TimeSpan.FromSeconds(2), LoggerFactory);
        }

        #region property

        //Xceed.Wpf.Toolkit.RichTextBox Control { get; set; }
        RichTextBox? RichTextBox { get; set; }
        FlowDocument Document => RichTextBox?.Document ?? throw new NullReferenceException(nameof(RichTextBox));
        Popup? Popup { get; set; }

        LazyAction TextChangeLazyAction { get; }

        public bool IsOpenToolbar
        {
            get => this._isOpenToolbar;
            set => SetProperty(ref this._isOpenToolbar, value);
        }

        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region NoteContentViewModelBase

        protected override Task LoadContentAsync(FrameworkElement baseElement)
        {
            //Control = (Xceed.Wpf.Toolkit.RichTextBox)control;
            RichTextBox = (RichTextBox)baseElement.FindName("content");
            Popup = (Popup)baseElement.FindName("popup");

            RichTextBox.TextChanged += Control_TextChanged;
            RichTextBox.SelectionChanged += RichTextBox_SelectionChanged;

            return Task.Run(() => {
                var content = Model.LoadRichTextContent();
                //RtfContent = content;

                var noteContentConverter = new NoteContentConverter(LoggerFactory);
                var stream = noteContentConverter.ToRtfStream(content);
                DispatcherWrapper.Begin(arg => {
                    if(arg.@this.IsDisposed) {
                        return;
                    }

                    var range = new TextRange(arg.@this.Document.ContentStart, arg.@this.Document.ContentEnd);
                    range.Load(arg.stream, DataFormats.Rtf);
                    stream.Dispose();
                }, (@this: this, stream), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            });
        }

        protected override void UnloadContent()
        {
            Flush();

            if(RichTextBox == null) {
                return;
            }

            RichTextBox.TextChanged -= Control_TextChanged;
            RichTextBox.SelectionChanged -= RichTextBox_SelectionChanged;

            Popup = null;
            RichTextBox = null;
            IsOpenToolbar = false;
        }

        protected override IDataObject GetClipbordContentData()
        {
            var data = new DataObject();
            if(CanVisible) {
                var noteContentConverter = new NoteContentConverter(LoggerFactory);
                data.SetText(noteContentConverter.ToRtfString(Document), TextDataFormat.Rtf);
            } else {
                var value = Model.LoadRichTextContent();
                data.SetText(value, TextDataFormat.Rtf);
            }

            return data;
        }

        void ChangedText()
        {
            if(CanVisible && EnabledUpdate) {
                DispatcherWrapper.Begin(vm => {
                    if(vm.IsDisposed) {
                        return;
                    }
                    var noteContentConverter = new NoteContentConverter(vm.LoggerFactory);
                    var content = noteContentConverter.ToRtfString(vm.Document);
                    vm.Model?.ChangeRichTextContent(content);
                }, this, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Flush();
            }

            base.Dispose(disposing);
        }

        #endregion

        #region IFlushable
        public void Flush()
        {
            TextChangeLazyAction.SafeFlush();
        }

        #endregion


        private void Control_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextChangeLazyAction.DelayAction(ChangedText);
        }

        private void RichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            // ツールバー的なの何かできればいいなぁと
            Debug.Assert(RichTextBox != null);
            Debug.Assert(Popup != null);

            if(RichTextBox.Selection.IsEmpty) {
                IsOpenToolbar = false;
                return;
            }

            // Popup.Placement = PlacementMode.AbsolutePoint;
            var startRect = RichTextBox.Selection.Start.GetCharacterRect(LogicalDirection.Forward);
            var endRect = RichTextBox.Selection.End.GetCharacterRect(LogicalDirection.Forward);
            var rect = new Rect(
                RichTextBox.PointToScreen(new Point(startRect.X, startRect.Y + startRect.Height)),
                new Size(endRect.Width, endRect.Height)
            );

            Popup.PlacementRectangle = rect;
            Logger.LogInformation(ObjectDumper.GetDumpString(rect));

            IsOpenToolbar = true;
        }

    }
}
