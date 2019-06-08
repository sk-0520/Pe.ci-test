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

        string _comment;

        #endregion

        public CustomizeLauncherCommentViewModel(CustomizeLauncherItemElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property

        public string Comment
        {
            get => this._comment;
            set => SetProperty(ref this._comment, value);
        }


        #endregion

        #region command
        #endregion

        #region function

        #endregion

        #region CustomizeLauncherDetailViewModelBase

        protected override void InitializeImpl()
        {
            Comment = Model.Comment;
        }

        #endregion
    }
}
