using System;
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

        protected override Task<bool> LoadContentAsync()
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

        public override void SearchContent(string value, bool toNext)
        {
            Logger.LogDebug("text: {value}, {toNext}", value, toNext);
        }

        #endregion
    }
}
