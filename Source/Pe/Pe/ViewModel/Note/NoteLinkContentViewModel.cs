using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Element.Note;
using ContentTypeTextNet.Pe.Main.Model.Note;

namespace ContentTypeTextNet.Pe.Main.ViewModel.Note
{
    public class NoteLinkContentViewModel : NoteContentViewModelBase
    {
        #region property

        string _content;
        string _illegalMessage;

        #endregion

        public NoteLinkContentViewModel(NoteContentElement model, IDispatcherWapper dispatcherWapper, ILogger logger)
            : base(model, dispatcherWapper, logger)
        { }

        public NoteLinkContentViewModel(NoteContentElement model, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWapper, loggerFactory)
        { }

        #region property

        NoteLinkContentData LinkData { get; set; }

        public string Content
        {
            get => this._content;
            set
            {
                if(SetProperty(ref this._content, value)) {
                    if(CanVisible && !HasIllegalMessage && IsEnabledRaiseContentChange) {
                        Model.ChangeLinkContent(Content);
                    }
                }
            }
        }

        public string IllegalMessage
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
        NoteLinkContentWatcher LinkWatcher { get; set; }

        bool IsEnabledRaiseContentChange { get; set; }

        #endregion

        #region command
        #endregion

        #region function


        #endregion

        #region NoteContentViewModelBase

        protected override void DetachModelEventsImpl()
        {
            base.DetachModelEventsImpl();
            if(LinkWatcher != null) {
                LinkWatcher.NoteContentChanged -= LinkWatcher_NoteContentChanged;
            }
        }

        protected override Task LoadContentAsync(Control control)
        {
            IllegalMessage = string.Empty;

            return Task.Run(() => {
                LinkData = Model.LoadLinkSetting();
                try {
                    var content = Model.LoadLinkContent(LinkData);
                    Content = content;
                    LinkWatcher = Model.StartLinkWatch(LinkData);
                    LinkWatcher.NoteContentChanged += LinkWatcher_NoteContentChanged;
                    IsEnabledRaiseContentChange = true;
                } catch(Exception ex) {
                    Logger.Error(ex);
                    IllegalMessage = ex.Message;
                }
            });
        }

        #endregion

        private void LinkWatcher_NoteContentChanged(object sender, NoteContentChangedEventArgs e)
        {
            try {
                var content = Model.LoadLinkContent(e.File, e.Encoding);
                IsEnabledRaiseContentChange = false;
                Content = content;
            } catch(Exception ex) {
                Logger.Error(ex);
                IllegalMessage = ex.Message;
            } finally {
                IsEnabledRaiseContentChange = true;
            }
        }
    }
}
