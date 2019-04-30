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
    public class NotePlainContentViewModel : NoteContentViewModelBase
    {
        #region variable

        string _content;

        #endregion

        public NotePlainContentViewModel(NoteContentElement model, IDispatcherWapper dispatcherWapper, ILogger logger)
            : base(model, dispatcherWapper, logger)
        { }

        public NotePlainContentViewModel(NoteContentElement model, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWapper, loggerFactory)
        { }

        #region property

        public string Content
        {
            get => this._content;
            set
            {
                if(SetProperty(ref this._content, value)) {
                    if(CanVisible) {
                        Model.ChangePlainContent(Content);
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
            return Task.Run(() => {
                var content = Model.LoadPlainContent();
                Content = content;
            });
        }

        protected override void UnloadContent()
        { }

        #endregion
    }
}
