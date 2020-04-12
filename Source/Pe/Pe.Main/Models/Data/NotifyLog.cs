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

    public class NotifyMessage
    {
        public NotifyMessage(NotifyLogKind kind, string header, string content)
        {
            Kind = kind;
            Header = header;
            Content = content;
            Command = EmptyCommand;
        }

        public NotifyMessage(NotifyLogKind kind, string header, string content, ICommand command)
        {
            Kind = kind;
            Header = header;
            Content = content;
            Command = command;
        }

        #region property

        private static ICommand EmptyCommand { get; } = new DelegateCommand(() => { });

        public NotifyLogKind Kind { get; }
        public string Header { get; } = string.Empty;
        public string Content { get; } = string.Empty;
        public ICommand Command { get; }

        #endregion
    }
}
