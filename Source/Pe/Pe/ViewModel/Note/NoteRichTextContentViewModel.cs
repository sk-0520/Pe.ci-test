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

        string _rtfContent;

        #endregion

        public NoteRichTextContentViewModel(NoteContentElement model, IDispatcherWapper dispatcherWapper, ILogger logger)
            : base(model, dispatcherWapper, logger)
        { }

        public NoteRichTextContentViewModel(NoteContentElement model, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWapper, loggerFactory)
        { }

        #region property

        Xceed.Wpf.Toolkit.RichTextBox Control { get; set; }

        public string RtfContent
        {
            get => this._rtfContent;
            set
            {
                if(SetProperty(ref this._rtfContent, value)) {
                    if(CanVisible) {
                        Model.ChangeRichTextContent(RtfContent);
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
            Control = (Xceed.Wpf.Toolkit.RichTextBox)control;

            return Task.Run(() => {
                var content = Model.LoadRichTextContent();
                RtfContent = content;
            });
        }
        #endregion
    }
}
