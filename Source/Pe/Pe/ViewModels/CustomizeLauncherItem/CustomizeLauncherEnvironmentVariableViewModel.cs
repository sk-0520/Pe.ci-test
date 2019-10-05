using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.CustomizeLauncherItem;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ICSharpCode.AvalonEdit.Document;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.CustomizeLauncherItem
{
    public class CustomizeLauncherEnvironmentVariableViewModel : CustomizeLauncherDetailViewModelBase
    {
        #region variable

        TextDocument? _updateTextDocument;
        TextDocument? _removeTextDocument;

        #endregion

        public CustomizeLauncherEnvironmentVariableViewModel(CustomizeLauncherItemElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property
        #endregion

        #region command

        public TextDocument? UpdateTextDocument
        {
            get => this._updateTextDocument;
            set => SetProperty(ref this._updateTextDocument, value);
        }
        public TextDocument? RemoveTextDocument
        {
            get => this._removeTextDocument;
            set => SetProperty(ref this._removeTextDocument, value);
        }

        #endregion

        #region function

        #endregion

        #region CustomizeLauncherDetailViewModelBase

        protected override void InitializeImpl()
        {
            var envItems = Model.LoadMergeEnvironmentVariableItems();
            var envItems2 = Model.LoadDeleteEnvironmentVariableItems();

            var updateItems = envItems
                .Select(i => $"{i.Name}={i.Value}")
            ;

            var removeItems = envItems2
                .Select(i => i)
            ;

            UpdateTextDocument = new TextDocument(string.Join(Environment.NewLine, updateItems));
            RemoveTextDocument = new TextDocument(string.Join(Environment.NewLine, removeItems));
        }

        #endregion
    }
}
