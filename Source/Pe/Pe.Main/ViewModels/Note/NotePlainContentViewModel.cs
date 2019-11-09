using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.Note;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Note
{
    public class NotePlainContentViewModel : NoteContentViewModelBase
    {
        #region variable

        string _content = string.Empty;

        #endregion

        public NotePlainContentViewModel(NoteContentElement model, IClipboardManager clipboardManager, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(model, clipboardManager, dispatcherWapper, loggerFactory)
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

        protected override Task LoadContentAsync(Control control)
        {
            return Task.Run(() => {
                try {
                    var content = Model.LoadPlainContent();
                    Content = content;
                } catch(Exception ex) {
                    Content = ex.Message;
                }
            });
        }

        protected override void UnloadContent()
        { }

        protected override IDataObject GetClipbordContentData()
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

        #endregion
    }
}
