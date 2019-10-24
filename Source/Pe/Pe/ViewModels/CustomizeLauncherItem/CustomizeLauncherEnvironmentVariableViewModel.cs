using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
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

        TextDocument? _mergeTextDocument;
        TextDocument? _removeTextDocument;

        #endregion

        public CustomizeLauncherEnvironmentVariableViewModel(CustomizeLauncherItemElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property
        #endregion

        #region command

        public TextDocument? MergeTextDocument
        {
            get => this._mergeTextDocument;
            set => SetProperty(ref this._mergeTextDocument, value);
        }
        public TextDocument? RemoveTextDocument
        {
            get => this._removeTextDocument;
            set => SetProperty(ref this._removeTextDocument, value);
        }

        #endregion

        #region function

        public IReadOnlyCollection<LauncherMergeEnvironmentVariableItem> GetMergeItems()
        {
            return TextUtility.ReadLines(MergeTextDocument!.Text)
                .Where(i => !string.IsNullOrWhiteSpace(i))
                .Select(i => i.Split(new char[] { '=' }, 2, StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim()).ToArray())
                .Where(i => i.Length == 2)
                .Select(i => new LauncherMergeEnvironmentVariableItem() { Name = i[0], Value = i[1] })
                .ToList()
            ;
        }

        public IReadOnlyCollection<string> GetRemoveItems()
        {
            return TextUtility.ReadLines(RemoveTextDocument!.Text)
                .Where(i => !string.IsNullOrWhiteSpace(i))
                .Select(i => i.Trim())
                .ToList()
            ;
        }

        #endregion

        #region CustomizeLauncherDetailViewModelBase

        protected override void InitializeImpl()
        {
            var envItems = Model.LoadMergeEnvironmentVariableItems();
            var envItems2 = Model.LoadDeleteEnvironmentVariableItems();

            var mergeItems = envItems
                .Select(i => $"{i.Name}={i.Value}")
            ;

            var removeItems = envItems2
                .Select(i => i)
            ;

            MergeTextDocument = new TextDocument(string.Join(Environment.NewLine, mergeItems));
            RemoveTextDocument = new TextDocument(string.Join(Environment.NewLine, removeItems));
        }

        #endregion
    }
}
