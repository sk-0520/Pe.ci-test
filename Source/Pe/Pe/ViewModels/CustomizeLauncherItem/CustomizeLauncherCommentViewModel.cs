using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models.Element.CustomizeLauncherItem;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.CustomizeLauncherItem
{
    public class CustomizeLauncherCommentViewModel : CustomizeLauncherDetailViewModelBase
    {
        #region variable

        //TextDocument _commentDocument;

        #endregion

        public CustomizeLauncherCommentViewModel(CustomizeLauncherItemElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property

        //public TextDocument CommentDocument
        //{
        //    get => this._commentDocument;
        //    set => SetProperty(ref this._commentDocument, value);
        //}


        #endregion

        #region command
        #endregion

        #region function

        #endregion

        #region CustomizeLauncherDetailViewModelBase

        protected override void InitializeImpl()
        {
            //CommentDocument = new TextDocument(Model.Comment);
        }

        #endregion
    }
}
