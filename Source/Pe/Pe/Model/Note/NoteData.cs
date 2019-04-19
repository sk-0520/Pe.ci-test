using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Note
{
    public enum NoteLayoutKind
    {
        Absolute,
        Relative
    }

    public enum NoteContentKind
    {
        Plain,
        Rtf,
    }

    public class NoteData : DataBase
    {
        #region property

        public Guid NoteId { get; set; }
        public string ScreenName { get; set; }
        public string Title { get; set; }
        public NoteLayoutKind LayoutKind { get; set; }
        public bool IsVisible { get; set; }

        public Guid FontId { get; set; }
        public Color ForegdoundColor { get; set; }
        public Color BackgroundColor { get; set; }
        public bool IsLocked { get; set; }
        public bool IsTopmost { get; set; }
        public bool IsCompact { get; set; }
        public bool TextWrap { get; set; }
        public NoteContentKind ContentKind { get; set; }
        #endregion
    }

    public class NoteLayoutData
    {
        #region property

        public Guid NoteId { get; set; }
        public NoteLayoutKind LayoutKind { get; set; }

        [PixelKind(Px.Device)]
        public double X { get; set; }
        [PixelKind(Px.Device)]
        public double Y { get; set; }
        [PixelKind(Px.Device)]
        public double Width { get; set; }
        [PixelKind(Px.Device)]
        public double Height { get; set; }

        #endregion
    }

    public class NoteContentData
    {
        #region property

        public Guid NoteId { get; set; }
        public NoteContentKind ContentKind { get; set; }

        public string Content { get; set; }

        #endregion
    }
}
