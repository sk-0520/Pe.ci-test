using System;
using System.Collections.ObjectModel;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.NotifyLog
{
    public class NotifyLogItemElement: ElementBase, IReadOnlyNotifyMessage, INotifyLogId
    {
        #region variable

        IReadOnlyNotifyLogContent _content;

        #endregion
        public NotifyLogItemElement(Guid notifyLogId, NotifyMessage message, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            NotifyLogId = notifyLogId;
            Message = message;
            this._content = Message.Content;

            ContentHistoriesImpl = new ObservableCollection<NotifyLogContent>(new[] { Message.Content });
            ContentHistories = new ReadOnlyObservableCollection<NotifyLogContent>(ContentHistoriesImpl);
        }

        #region property

        NotifyMessage Message { get; }
        ObservableCollection<NotifyLogContent> ContentHistoriesImpl { get; }
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

        protected override void InitializeImpl()
        { }

        #endregion

        #region INotifyLogId

        public Guid NotifyLogId { get; }

        #endregion
    }
}
