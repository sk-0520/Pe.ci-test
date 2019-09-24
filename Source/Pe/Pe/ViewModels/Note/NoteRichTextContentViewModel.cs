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
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Note
{
    public class NoteRichTextContentViewModel : NoteContentViewModelBase
    {
        #region variable

        string? _rtfContent;

        #endregion

        public NoteRichTextContentViewModel(NoteContentElement model, IClipboardManager clipboardManager, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(model, clipboardManager, dispatcherWapper, loggerFactory)
        {
            TextChangeLazyAction = new LazyAction("RTF変更抑制", TimeSpan.FromSeconds(2), LoggerFactory);
        }

        #region property

        //Xceed.Wpf.Toolkit.RichTextBox Control { get; set; }
        RichTextBox? Control { get; set; }
        LazyAction TextChangeLazyAction { get; }

        public string? RtfContent
        {
            get => this._rtfContent;
            set
            {
                if(SetProperty(ref this._rtfContent, value)) {
                    if(CanVisible) {
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
                        Model.ChangeRichTextContent(RtfContent);
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
                    }
                }
            }
        }

        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region NoteContentViewModelBase

        protected override Task LoadContentAsync(Control control)
        {
            //Control = (Xceed.Wpf.Toolkit.RichTextBox)control;
            Control = (RichTextBox)control;

            Control.TextChanged += Control_TextChanged;

            return Task.Run(() => {
                var content = Model.LoadRichTextContent();
                //RtfContent = content;

                var doc = Control!.Document;
                var range = new TextRange(doc.ContentStart, doc.ContentEnd);
                var rtf = Encoding.UTF8.GetBytes(content);
                using(var stream = new MemoryStream(rtf)) {
                    DispatcherWapper.Invoke(() => {
                        range.Load(stream, DataFormats.Rtf);
                    });
                }
            });
        }

        protected override void UnloadContent()
        {
            if(Control == null) {
                return;
            }

            Control.TextChanged -= Control_TextChanged;
        }

        protected override IDataObject GetContentData()
        {
            var data = new DataObject();
            if(CanVisible) {
                data.SetText(RtfContent, TextDataFormat.Rtf);
            } else {
                var value = Model.LoadRichTextContent();
                data.SetText(value, TextDataFormat.Rtf);
            }

            return data;
        }

        void ChangedText()
        {
            var doc = Control!.Document;
            var range = new TextRange(doc.ContentStart, doc.ContentEnd);
            using(var stream = new MemoryStream()) {
                DispatcherWapper.Invoke(() => range.Save(stream, DataFormats.Rtf));

                var content = Encoding.UTF8.GetString(stream.ToArray());
                Model.ChangeRichTextContent(content);
            }
        }

        #endregion

        private void Control_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextChangeLazyAction.DelayAction(ChangedText);
        }

    }
}
