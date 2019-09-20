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
    public class CustomizeLauncherEnvironmentVariableViewModel : CustomizeLauncherDetailViewModelBase
    {
        #region variable

        TextDocument _updateTextDocument;
        TextDocument _removeTextDocument;

        #endregion

        public CustomizeLauncherEnvironmentVariableViewModel(CustomizeLauncherItemElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property
        #endregion

        #region command

        public TextDocument UpdateTextDocument
        {
            get => this._updateTextDocument;
            set => SetProperty(ref this._updateTextDocument, value);
        }
        public TextDocument RemoveTextDocument
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
            var envItems = Model.LoadEnvironmentVariableItems();

            var updateItems = envItems
                .Where(i => i.Kind == LauncherEnvironmentVariableKind.Update)
                .Select(i => $"{i.Name}={i.Value}")
            ;

            var removeItems = envItems
                .Where(i => i.Kind == LauncherEnvironmentVariableKind.Remove)
                .Select(i => i.Name)
            ;

            UpdateTextDocument = new TextDocument(string.Join(Environment.NewLine, updateItems));
            RemoveTextDocument = new TextDocument(string.Join(Environment.NewLine, removeItems));
        }

        #endregion
    }
}
