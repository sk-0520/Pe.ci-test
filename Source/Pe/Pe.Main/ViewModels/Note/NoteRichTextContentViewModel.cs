using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        public NoteRichTextContentViewModel(NoteContentElement model, IClipboardManager clipboardManager, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, clipboardManager, dispatcherWrapper, loggerFactory)
        {
            TextChangeLazyAction = new LazyAction("RTF変更抑制", TimeSpan.FromSeconds(2), LoggerFactory);
        }

        #region property

        //Xceed.Wpf.Toolkit.RichTextBox Control { get; set; }
        RichTextBox? RichTextBox { get; set; }
        FlowDocument Document => RichTextBox?.Document ?? throw new NullReferenceException(nameof(RichTextBox));

        LazyAction TextChangeLazyAction { get; }

        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region NoteContentViewModelBase

        protected override Task LoadContentAsync(Control control)
        {
            //Control = (Xceed.Wpf.Toolkit.RichTextBox)control;
            RichTextBox = (RichTextBox)control;

            RichTextBox.TextChanged += Control_TextChanged;

            return Task.Run(() => {
                var content = Model.LoadRichTextContent();
                //RtfContent = content;

                var noteContentConverter = new NoteContentConverter(LoggerFactory);
                var stream = noteContentConverter.ToRtfStream(content);
                DispatcherWrapper.Begin(() => {
                    var range = new TextRange(Document.ContentStart, Document.ContentEnd);
                    range.Load(stream, DataFormats.Rtf);
                    stream.Dispose();
                });
            });
        }

        protected override void UnloadContent()
        {
            Flush();

            if(RichTextBox == null) {
                return;
            }

            RichTextBox.TextChanged -= Control_TextChanged;
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
                var noteContentConverter = new NoteContentConverter(LoggerFactory);
                DispatcherWrapper.Begin(() => {
                    if(!IsDisposed) {
                        var content = noteContentConverter.ToRtfString(Document);
                        Model?.ChangeRichTextContent(content);
                    }
                });
            }
        }

        #endregion

        private void Control_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextChangeLazyAction.DelayAction(ChangedText);
        }

        #region IFlushable
        public void Flush()
        {
            TextChangeLazyAction.SafeFlush();
        }

        #endregion
    }
}
