using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Element.Note;

namespace ContentTypeTextNet.Pe.Main.ViewModel.Note
{
    public class NoteRichTextContentViewModel: NoteContentViewModelBase
    {
        #region variable

        string _content;

        #endregion

        public NoteRichTextContentViewModel(NoteContentElement model, IDispatcherWapper dispatcherWapper, ILogger logger)
            : base(model, dispatcherWapper, logger)
        { }

        public NoteRichTextContentViewModel(NoteContentElement model, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWapper, loggerFactory)
        { }

        #region property

        //RichTextBox Control { get; set; }
        public string Content
        {
            get => this._content;
            set
            {
                if(SetProperty(ref this._content, value)) {
                    if(CanVisible) {
                        Model.ChangeRichTextContent(Content);
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
            /*
            Control = (RichTextBox)control;
            return Task.Run(() => {
                var document = Model.LoadRichTextContent();
                DispatcherWapper.Invoke(() => {
                    Control.Document = document;
                });
            });
            */
            return Task.Run(() => {
                var document = Model.LoadRichTextContent();
                DispatcherWapper.Invoke(() => {
                    //Control.Document = document;
                });
                Content = document;
            });
        }
        #endregion
    }
}
