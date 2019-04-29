using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Element.Note;

namespace ContentTypeTextNet.Pe.Main.ViewModel.Note
{
    public class NotePlainContentViewModel : NoteContentViewModelBase
    {
        #region variable

        string _content;

        #endregion

        public NotePlainContentViewModel(NoteContentElement model, ILogger logger)
            : base(model, logger)
        { }

        public NotePlainContentViewModel(NoteContentElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property

        public string Content
        {
            get => this._content;
            set
            {
                if(SetProperty(ref this._content, value)) {
                    Model.ChangePlainContent(Content);
                }
            }
        }

        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region NoteContentViewModelBase

        protected override Task LoadContentAsync()
        {
            return Task.Run(() => {
                var content = Model.LoadPlainContent();
                Content = content;
            });
        }

        #endregion
    }
}
