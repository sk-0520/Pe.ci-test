using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Element.Note;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Note
{
    public class NotePlainContentViewModel: NoteContentViewModelBase<TextBox>
    {
        #region variable

        private string _content = string.Empty;

        #endregion

        public NotePlainContentViewModel(NoteContentElement model, NoteConfiguration noteConfiguration, IClipboardManager clipboardManager, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, noteConfiguration, clipboardManager, dispatcherWrapper, loggerFactory)
        { }

        #region property

        public string Content
        {
            get => this._content;
            set
            {
                if(SetProperty(ref this._content, value)) {
                    if(CanVisible && EnabledUpdate) {
                        Model.ChangePlainContent(Content);
                    }
                }
            }
        }

        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region NoteContentViewModelBase

        protected override Task<bool> LoadContentAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() => {
                try {
                    var content = Model.LoadPlainContent();
                    Content = content;
                    return true;
                } catch(Exception ex) {
                    Content = ex.Message;
                }
                return false;
            });
        }

        protected override void UnloadContent()
        { }

        protected override IDataObject GetClipboardContentData()
        {
            var data = new DataObject();
            if(CanVisible) {
                data.SetText(Content, TextDataFormat.UnicodeText);
            } else {
                var value = Model.LoadPlainContent();
                data.SetText(value, TextDataFormat.UnicodeText);
            }

            return data;
        }

        public override void SearchContent(string searchValue, bool searchNext)
        {
            Logger.LogDebug("text: {SearchValue}, {SearchNext}", searchValue, searchNext);
            Logger.LogDebug("selection: {SelectionStart}, {SelectionLength}", ControlElement.SelectionStart, ControlElement.SelectionLength);

            var content = ControlElement.Text;
            var comparisonType = StringComparison.OrdinalIgnoreCase;

            var selectionIndex = searchNext
                ? content.IndexOf(searchValue, ControlElement.SelectionStart + ControlElement.SelectionLength, comparisonType)
                : content.LastIndexOf(searchValue, ControlElement.SelectionStart - ControlElement.SelectionLength, comparisonType)
                ;

            if(selectionIndex == -1) {
                selectionIndex = searchNext
                    ? content.IndexOf(searchValue, 0, comparisonType)
                    : content.LastIndexOf(searchValue, content.Length, comparisonType)
                ;
            }

            if(selectionIndex == -1) {
                Logger.LogTrace("exit");
                return;
            }

            ControlElement.Focus();
            ControlElement.Select(selectionIndex, searchValue.Length);
        }

        #endregion
    }
}
