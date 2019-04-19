using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Main.Model.Logic;

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

    public class NoteScreenData : DataBase, IScreenData
    {
        #region property

        public Guid NoteId { get; set; }

        #endregion

        #region IScreenData

        public string ScreenName { get; set; }
        [PixelKind(Px.Device)]
        public long X { get; set; }
        [PixelKind(Px.Device)]
        public long Y { get; set; }
        [PixelKind(Px.Device)]
        public long Width { get; set; }
        [PixelKind(Px.Device)]
        public long Height { get; set; }

        #endregion
    }

}
