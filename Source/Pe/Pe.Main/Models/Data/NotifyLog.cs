using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Data;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    /// <summary>
    /// 通知表示位置。
    /// </summary>
    public enum NotifyLogPosition
    {
        /// <summary>
        /// カーソル位置。
        /// </summary>
        Cursor,
        /// <summary>
        /// デスクトップ固定。
        /// </summary>
        Desktop,
    }

    /// <summary>
    /// 通知種別。
    /// </summary>
    public enum NotifyLogKind
    {
        /// <summary>
        /// プラットフォーム固有。
        /// </summary>
        Platform,
        /// <summary>
        /// 通常メッセージ。
        /// </summary>
        Normal,
        /// <summary>
        /// 元に戻すことができるメッセージ。
        /// </summary>
        Undo,
        /// <summary>
        /// コマンド実行メッセージ。
        /// </summary>
        Command,
        /// <summary>
        /// 最上位固定メッセージ。
        /// </summary>
        Topmost,
    }

    public enum NotifyEventKind
    {
        Add,
        Change,
        Clear,
    }

    public interface INotifyLogId
    {
        #region property

        Guid NotifyLogId { get; }

        #endregion
    }


    public interface IReadOnlyNotifyLogContent
    {
        #region property

        string Message { get; }
        DateTime Timestamp { get; }

        #endregion
    }
    public class NotifyLogContent: IReadOnlyNotifyLogContent
    {
        public NotifyLogContent(string content)
            : this(content, DateTime.UtcNow)
        { }

        public NotifyLogContent(string content, [Timestamp(DateTimeKind.Utc)] DateTime timestamp)
        {
            Message = content;
            Timestamp = timestamp;
        }

        #region IReadOnlyNotifyLogContent

        public string Message { get; }
        [Timestamp(DateTimeKind.Utc)]
        public DateTime Timestamp { get; }

        #endregion
    }


    public interface IReadOnlyNotifyMessage
    {
        #region property

        NotifyLogKind Kind { get; }
        string Header { get; }
        IReadOnlyNotifyLogContent Content { get; }
        Action Callback { get; }

        #endregion
    }

    public class NotifyMessage : IReadOnlyNotifyMessage
    {
        public NotifyMessage(NotifyLogKind kind, string header, NotifyLogContent notifyLogContent)
        {
            if(!(kind == NotifyLogKind.Normal || kind == NotifyLogKind.Topmost)) {
                throw new ArgumentException(nameof(kind));
            }

            Kind = kind;
            Header = !string.IsNullOrWhiteSpace(header) ? header : throw new ArgumentException(nameof(header));
            Content = notifyLogContent ?? throw new ArgumentException(nameof(notifyLogContent));
            Callback = EmptyCallback;
        }

        public NotifyMessage(NotifyLogKind kind, string header, NotifyLogContent notifyLogContent, Action callback)
        {
            if(!(kind == NotifyLogKind.Platform || kind == NotifyLogKind.Undo || kind == NotifyLogKind.Command)) {
                throw new ArgumentException(nameof(kind));
            }

            Kind = kind;
            Header = !string.IsNullOrWhiteSpace(header) ? header : throw new ArgumentException(nameof(header));
            Content = notifyLogContent ?? throw new ArgumentException(nameof(notifyLogContent));
            Callback = callback ?? throw new ArgumentNullException(nameof(callback));
        }

        #region function

        private static void EmptyCallback() { }

        #endregion

        #region IReadOnlyNotifyMessage

        public NotifyLogKind Kind { get; }
        public string Header { get; }
        public NotifyLogContent Content { get; }
        IReadOnlyNotifyLogContent IReadOnlyNotifyMessage.Content => Content;
        public Action Callback { get; }

        #endregion
    }

}
