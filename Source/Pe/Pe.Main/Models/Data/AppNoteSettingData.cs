using System;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    public class SettingAppNoteSettingData: DataBase
    {
        #region property

        public Guid FontId { get; set; }
        public NoteCreateTitleKind TitleKind { get; set; }
        public NoteLayoutKind LayoutKind { get; set; }
        public Color ForegroundColor { get; set; }
        public Color BackgroundColor { get; set; }
        public bool IsTopmost { get; set; }
        public NoteCaptionPosition CaptionPosition { get; set; }
        #endregion
    }
}
