using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Element.CustomizeLauncherItem;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherItem;
using ICSharpCode.AvalonEdit.Document;

namespace ContentTypeTextNet.Pe.Main.ViewModel.CustomizeLauncherItem
{
    public class CustomizeLauncherCommentViewModel : CustomizeLauncherDetailViewModelBase
    {
        #region variable

        TextDocument _commentDocument;

        #endregion

        public CustomizeLauncherCommentViewModel(CustomizeLauncherItemElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property

        public TextDocument CommentDocument
        {
            get => this._commentDocument;
            set => SetProperty(ref this._commentDocument, value);
        }


        #endregion

        #region command
        #endregion

        #region function

        #endregion

        #region CustomizeLauncherDetailViewModelBase

        protected override void InitializeImpl()
        {
            CommentDocument = new TextDocument(Model.Comment);
        }

        #endregion
    }
}
