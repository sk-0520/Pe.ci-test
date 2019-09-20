using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Element.CustomizeLauncherItem;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherItem;
using ICSharpCode.AvalonEdit.Document;

namespace ContentTypeTextNet.Pe.Main.ViewModel.CustomizeLauncherItem
{
    public class CustomizeLauncherTagViewModel : CustomizeLauncherDetailViewModelBase
    {
        #region variable

        TextDocument _tagDocument;

        #endregion

        public CustomizeLauncherTagViewModel(CustomizeLauncherItemElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property
        public TextDocument TagDocument
        {
            get => this._tagDocument;
            set => SetProperty(ref this._tagDocument, value);
        }

        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region CustomizeLauncherDetailViewModelBase

        protected override void InitializeImpl()
        {
            var tags = Model.LoadTags();
            TagDocument = new TextDocument(string.Join(Environment.NewLine, tags));
        }

        #endregion
    }
}
