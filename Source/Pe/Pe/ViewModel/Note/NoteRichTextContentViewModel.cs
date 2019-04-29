using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Element.Note;

namespace ContentTypeTextNet.Pe.Main.ViewModel.Note
{
    public class NoteRichTextContentViewModel: NoteContentViewModelBase
    {
        public NoteRichTextContentViewModel(NoteContentElement model, ILogger logger)
            : base(model, logger)
        { }

        public NoteRichTextContentViewModel(NoteContentElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property
        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region NoteContentViewModelBase
        protected override Task LoadContentAsync()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
