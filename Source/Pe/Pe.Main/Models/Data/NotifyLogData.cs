using System;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

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
        [EnumResource]
        Cursor,
        /// <summary>
        /// 中央表示。
        /// </summary>
        /// <remarks>
        /// <para>デスクトップ固定。</para>
        /// </remarks>
        [EnumResource]
        Center,
        /// <summary>
        /// 左上。
        /// </summary>
        /// <remarks>
        /// <para>デスクトップ固定。</para>
        /// </remarks>
        [EnumResource]
        LeftTop,
        /// <summary>
        /// 右上。
        /// </summary>
        /// <remarks>
        /// <para>デスクトップ固定。</para>
        /// </remarks>
        [EnumResource]
        RightTop,
        /// <summary>
        /// 左下。
        /// </summary>
        /// <remarks>
        /// <para>デスクトップ固定。</para>
        /// </remarks>
        [EnumResource]
        LeftBottom,
        /// <summary>
        /// 右下。
        /// </summary>
        /// <remarks>
        /// <para>デスクトップ固定。</para>
        /// </remarks>
        [EnumResource]
        RightBottom,
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

        NotifyLogId NotifyLogId { get; }

        #endregion
    }


    public interface IReadOnlyNotifyLogContent
    {
        #region property

        string Message { get; }
        DateTime Timestamp { get; }

        #endregion
    }

    public record NotifyLogContent: IReadOnlyNotifyLogContent
    {
        public NotifyLogContent(string content)
            : this(content, DateTime.UtcNow)
        { }

        public NotifyLogContent(string content, [DateTimeKind(DateTimeKind.Utc)] DateTime timestamp)
        {
            Message = content;
            Timestamp = timestamp;
        }

        #region IReadOnlyNotifyLogContent

        public string Message { get; }
        [DateTimeKind(DateTimeKind.Utc)]
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

    public record NotifyMessage: IReadOnlyNotifyMessage
    {
        public NotifyMessage(NotifyLogKind kind, string header, NotifyLogContent notifyLogContent)
        {
            if(!(kind == NotifyLogKind.Normal || kind == NotifyLogKind.Topmost)) {
                throw new ArgumentException(null, nameof(kind));
            }

            Kind = kind;
            Header = !string.IsNullOrWhiteSpace(header) ? header : throw new ArgumentException(nameof(header));
            Content = notifyLogContent ?? throw new ArgumentException(null, nameof(notifyLogContent));
            Callback = EmptyCallback;
        }

        public NotifyMessage(NotifyLogKind kind, string header, NotifyLogContent notifyLogContent, Action callback)
        {
            if(!(kind == NotifyLogKind.Platform || kind == NotifyLogKind.Undo || kind == NotifyLogKind.Command)) {
                throw new ArgumentException(null, nameof(kind));
            }

            Kind = kind;
            Header = !string.IsNullOrWhiteSpace(header) ? header : throw new ArgumentException(null, nameof(header));
            Content = notifyLogContent ?? throw new ArgumentException(null, nameof(notifyLogContent));
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
