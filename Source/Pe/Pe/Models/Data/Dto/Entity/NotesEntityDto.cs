using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity
{
    public class NotesEntityDto : CommonDtoBase
    {
        #region property

        public Guid NoteId { get; set; }
        public string? Title { get; set; }
        public string? ScreenName { get; set; }
        public string? LayoutKind { get; set; }
        public bool IsVisible { get; set; }
        public Guid FontId { get; set; }
        public string? ForegroundColor { get; set; }
        public string? BackgroundColor { get; set; }
        public bool IsLocked { get; set; }
        public bool IsTopmost { get; set; }
        public bool IsCompact { get; set; }
        public bool TextWrap { get; set; }
        public string? ContentKind { get; set; }
        public bool IsLinkContent { get; set; }

        #endregion
    }
}
