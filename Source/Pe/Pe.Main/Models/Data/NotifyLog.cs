using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
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

    public interface IReadOnlyNotifyMessage
    {
        #region property

        NotifyLogKind Kind { get; }
        string Header { get; }
        string Content { get; }
        Action Callback { get; }

        #endregion
    }

    public class NotifyMessage: IReadOnlyNotifyMessage
    {
        public NotifyMessage(NotifyLogKind kind, string header, string content)
        {
            if(!(kind == NotifyLogKind.Normal || kind == NotifyLogKind.Topmost)) {
                throw new ArgumentException(nameof(kind));
            }

            Kind = kind;
            Header = string.IsNullOrWhiteSpace(header) ? header : throw new ArgumentException(nameof(header));
            Content = string.IsNullOrWhiteSpace(content) ? header : throw new ArgumentException(nameof(content)); ;
            Callback = EmptyCallback;
        }

        public NotifyMessage(NotifyLogKind kind, string header, string content, Action callback)
        {
            if(!(kind == NotifyLogKind.Platform || kind == NotifyLogKind.Undo || kind == NotifyLogKind.Command)) {
                throw new ArgumentException(nameof(kind));
            }

            Kind = kind;
            Header = string.IsNullOrWhiteSpace(header) ? header : throw new ArgumentException(nameof(header));
            Content = string.IsNullOrWhiteSpace(content) ? header : throw new ArgumentException(nameof(content)); ;
            Callback = callback ?? throw new ArgumentNullException(nameof(callback));
        }

        #region function

        private static void EmptyCallback() { }

        #endregion

        #region IReadOnlyNotifyMessage

        public NotifyLogKind Kind { get; }
        public string Header { get; } = string.Empty;
        public string Content { get; } = string.Empty;
        public Action Callback { get; }

        #endregion
    }
}
