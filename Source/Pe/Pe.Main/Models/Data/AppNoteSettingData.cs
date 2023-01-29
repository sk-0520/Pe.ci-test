using System;
using System.Runtime.Serialization;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    /// <summary>
    /// ノートの設定。
    /// </summary>
    [Serializable, DataContract]
    public class SettingAppNoteSettingData
    {
        #region property

        /// <summary>
        /// フォントID。
        /// </summary>
        public FontId FontId { get; set; }
        /// <summary>
        /// タイトル新規作成種別。
        /// </summary>
        public NoteCreateTitleKind TitleKind { get; set; }
        /// <summary>
        /// レイアウト種別。
        /// </summary>
        public NoteLayoutKind LayoutKind { get; set; }
        /// <summary>
        /// 前景色。
        /// </summary>
        public Color ForegroundColor { get; set; }
        /// <summary>
        /// 背景色。
        /// </summary>
        public Color BackgroundColor { get; set; }
        /// <summary>
        /// 最前面。
        /// </summary>
        public bool IsTopmost { get; set; }
        /// <summary>
        /// タイトルバー位置。
        /// </summary>
        public NoteCaptionPosition CaptionPosition { get; set; }

        #endregion
    }
}
