using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ICSharpCode.AvalonEdit.Document;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize
{
    public class LauncherItemCustomizeCommentViewModel : LauncherItemCustomizeDetailViewModelBase
    {
        #region variable

        TextDocument? _commentDocument;

        #endregion

        public LauncherItemCustomizeCommentViewModel(LauncherItemCustomizeEditorElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property

        public TextDocument? CommentDocument
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
