using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.NotifyLog
{
    public class NotifyLogItemElement : ElementBase, IReadOnlyNotifyMessage
    {
        #region variable

        string _content;

        #endregion
        public NotifyLogItemElement(Guid notifyLogId, NotifyMessage message, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            NotifyLogId = notifyLogId;
            Message = message;
            this._content = Message.Content;

            ContentHistoriesImpl = new ObservableCollection<string>(new[] { this._content });
            ContentHistories = new ReadOnlyObservableCollection<string>(ContentHistoriesImpl);
        }

        #region property

        NotifyMessage Message { get; }
        ObservableCollection<string> ContentHistoriesImpl { get; }
        public ReadOnlyObservableCollection<string> ContentHistories { get; }

        #endregion

        #region IReadOnlyNotifyMessage

        public NotifyLogKind Kind => Message.Kind;

        public string Header => Message.Header;
        public string Content
        {
            get => this._content;
            private set => SetProperty(ref this._content, value);
        }

        public Action Callback => Message.Callback;

        #endregion

        #region function

        public void ChangeContent(string content)
        {
            if(content == null) {
                throw new ArgumentNullException(nameof(content));
            }

            ContentHistoriesImpl.Add(content);
            Content = content;
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
