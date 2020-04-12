using System;
using System.Collections.Generic;
using System.Text;

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

    }
}
