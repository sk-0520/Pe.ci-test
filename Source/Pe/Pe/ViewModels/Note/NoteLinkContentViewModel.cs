using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.Note;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Note;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Note
{
    public class NoteLinkContentViewModel : NoteContentViewModelBase
    {
        #region property

        string? _content;
        string? _illegalMessage;

        #endregion

        public NoteLinkContentViewModel(NoteContentElement model, IClipboardManager clipboardManager, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(model, clipboardManager, dispatcherWapper, loggerFactory)
        { }

        #region property

        NoteLinkContentData? LinkData { get; set; }

        public string? Content
        {
            get => this._content;
            set
            {
                if(SetProperty(ref this._content, value)) {
                    if(CanVisible && !HasIllegalMessage && IsEnabledRaiseContentChange) {
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
                        Model.ChangeLinkContent(Content);
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
                    }
                }
            }
        }

        public string? IllegalMessage
        {
            get => this._illegalMessage;
            set
            {
                SetProperty(ref this._illegalMessage, value);
                HasIllegalMessage = !string.IsNullOrWhiteSpace(IllegalMessage);
                RaisePropertyChanged(nameof(HasIllegalMessage));
            }
        }

        public bool HasIllegalMessage { get; set; }
        NoteLinkContentWatcher? LinkWatcher { get; set; }

        bool IsEnabledRaiseContentChange { get; set; }

        #endregion

        #region command
        #endregion

        #region function


        #endregion

        #region NoteContentViewModelBase

        protected override Task LoadContentAsync(Control control)
        {
            IllegalMessage = string.Empty;

            return Task.Run(() => {
                LinkData = Model.LoadLinkSetting();
                try {
                    var content = Model.LoadLinkContent(LinkData.ToFileInfo(), LinkData.ToEncoding());
                    Content = content;
                    LinkWatcher = Model.StartLinkWatch(LinkData);
                    LinkWatcher.NoteContentChanged += LinkWatcher_NoteContentChanged!;
                    IsEnabledRaiseContentChange = true;
                } catch(Exception ex) {
                    Logger.LogError(ex, ex.Message);
                    IllegalMessage = ex.Message;
                }
            });
        }
        protected override void UnloadContent()
        {
            if(LinkWatcher != null) {
                LinkWatcher.Stop();
                LinkWatcher.NoteContentChanged -= LinkWatcher_NoteContentChanged!;
                LinkWatcher.Dispose();
                LinkWatcher = null;
            }
        }

        protected override IDataObject GetContentData()
        {
            var data = new DataObject();
            if(CanVisible && !HasIllegalMessage) {
                data.SetText(Content, TextDataFormat.UnicodeText);
            } else if(HasIllegalMessage) {
                Logger.LogWarning("{0}: {1}", Model.NoteId, HasIllegalMessage);
                data.SetText(IllegalMessage, TextDataFormat.UnicodeText);
            } else {
                try {
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
                    var value = Model.LoadLinkContent(LinkData.ToFileInfo(), LinkData.ToEncoding());
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
                    data.SetText(value, TextDataFormat.UnicodeText);
                } catch(Exception ex) {
                    Logger.LogError(ex, ex.Message);
                    data.SetText(ex.ToString(), TextDataFormat.UnicodeText);
                }
            }

            return data;
        }

        #endregion

        private void LinkWatcher_NoteContentChanged(object sender, NoteContentChangedEventArgs e)
        {
            try {
                var content = Model.LoadLinkContent(e.File, e.Encoding);
                IsEnabledRaiseContentChange = false;
                Content = content;
            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
                IllegalMessage = ex.Message;
            } finally {
                IsEnabledRaiseContentChange = true;
            }
        }
    }
}
