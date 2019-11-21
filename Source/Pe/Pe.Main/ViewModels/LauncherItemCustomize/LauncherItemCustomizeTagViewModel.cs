using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ICSharpCode.AvalonEdit.Document;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize
{
    public class LauncherItemCustomizeTagViewModel : LauncherItemCustomizeDetailViewModelBase
    {
        #region variable

        TextDocument? _tagDocument;

        #endregion

        public LauncherItemCustomizeTagViewModel(LauncherItemCustomizeElementBase model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property

        public TextDocument? TagDocument
        {
            get => this._tagDocument;
            set => SetProperty(ref this._tagDocument, value);
        }

        #endregion

        #region command
        #endregion

        #region function

        public IReadOnlyCollection<string> GetTagItems()
        {
            return TextUtility.ReadLines(TagDocument!.Text)
                .Where(i => !string.IsNullOrWhiteSpace(i))
                .Select(i => i.Trim())
                .ToList()
            ;
        }

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
