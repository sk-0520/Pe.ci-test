using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.NotifyLog
{
    public class NotifyLogItemElement: ElementBase, IReadOnlyNotifyMessage, INotifyLogId
    {
        #region variable

        private IReadOnlyNotifyLogContent _content;

        #endregion

        public NotifyLogItemElement(NotifyLogId notifyLogId, NotifyMessage message, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            NotifyLogId = notifyLogId;
            Message = message;
            this._content = Message.Content;

            ContentHistoriesImpl = new ObservableCollection<NotifyLogContent>(new[] { Message.Content });
            ContentHistories = new ReadOnlyObservableCollection<NotifyLogContent>(ContentHistoriesImpl);
        }

        #region property

        private NotifyMessage Message { get; }
        private ObservableCollection<NotifyLogContent> ContentHistoriesImpl { get; }
        public ReadOnlyObservableCollection<NotifyLogContent> ContentHistories { get; }

        #endregion

        #region IReadOnlyNotifyMessage

        public NotifyLogKind Kind => Message.Kind;

        public string Header => Message.Header;
        public IReadOnlyNotifyLogContent Content
        {
            get => this._content;
            private set => SetProperty(ref this._content, value);
        }

        public Action Callback => Message.Callback;

        #endregion

        #region function

        public void ChangeContent(NotifyLogContent notifyContent)
        {
            if(notifyContent == null) {
                throw new ArgumentNullException(nameof(notifyContent));
            }

            ContentHistoriesImpl.Add(notifyContent);
            Content = notifyContent;
        }

        #endregion

        #region ElementBase

        protected override Task InitializeCoreAsync()
        {
            return Task.CompletedTask;
        }

        #endregion

        #region INotifyLogId

        public NotifyLogId NotifyLogId { get; }

        #endregion
    }
}
