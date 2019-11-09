using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    public enum NotePosition
    {
        Setting,
        CursorPosition,
        CenterScreen,
    }

    public enum NoteLayoutKind
    {
        Absolute,
        Relative,
    }

    public enum NoteContentKind
    {
        Plain,
        RichText,
        [Obsolete]
        Link,
    }

    [Flags]
    public enum ViewAreaChangeTarget
    {
        None = 0,
        Location = 0b0000_0001,
        Suze = 0b0000_0010,
    }

    public class NoteData : DataBase
    {
        #region property

        public Guid NoteId { get; set; }
        public string ScreenName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public NoteLayoutKind LayoutKind { get; set; }
        public bool IsVisible { get; set; }

        public Guid FontId { get; set; }
        public Color ForegroundColor { get; set; }
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

        [PixelKind(Px.Logical)]
        public double X { get; set; }
        [PixelKind(Px.Logical)]
        public double Y { get; set; }
        [PixelKind(Px.Logical)]
        public double Width { get; set; }
        [PixelKind(Px.Logical)]
        public double Height { get; set; }

        #endregion
    }

    public class NoteContentData
    {
        #region property

        public Guid NoteId { get; set; }

        public NoteContentKind ContentKind { get; set; }

        public string Content { get; set; } = string.Empty;

        public bool IsLink { get; set; }

        /// <summary>
        /// リンク対象ファイル名。
        /// </summary>
        [DataMember]
        public string FilePath { get; set; } = string.Empty;

        /// <summary>
        /// <see cref="FilePath"/> のエンコーディング。
        /// </summary>
        [DataMember]
        public Encoding Encoding { get; set; } = EncodingConverter.DefaultEncoding;

        /// <summary>
        /// ファイル変更から実際に読むまでの待機時間。
        /// </summary>
        [DataMember]
        public TimeSpan DelayTime { get; set; }

        /// <summary>
        /// <see cref="System.IO.FileSystemWatcher.InternalBufferSize"/>。
        /// </summary>
        [DataMember]
        public int BufferSize { get; set; }

        /// <summary>
        /// <see cref="System.IO.FileSystemWatcher"/> で取りこぼした際の更新時間。
        /// </summary>
        [DataMember]
        public TimeSpan RefreshTime { get; set; }
        /// <summary>
        /// そもそも取りこぼしを考慮するか。
        /// <para>将来用。</para>
        /// </summary>
        [DataMember]
        public bool IsEnabledRefresh { get; set; }

        #endregion
    }

    public class NoteScreenData : DataBase, IScreenData
    {
        #region property

        public Guid NoteId { get; set; }

        #endregion

        #region IScreenData

        public string ScreenName { get; set; } = string.Empty;
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

    [Obsolete]
    [Serializable, DataContract]
    public class NoteLinkContentData
    {
        #region property

        /// <summary>
        /// リンク対象ファイル名。
        /// </summary>
        [DataMember]
        public string FilePath { get; set; } = string.Empty;

        /// <summary>
        /// <see cref="FilePath"/> のエンコーディング。
        /// </summary>
        [DataMember]
        public string EncodingName { get; set; } = string.Empty;

        /// <summary>
        /// ファイル変更から実際に読むまでの待機時間。
        /// </summary>
        [DataMember]
        public TimeSpan DelayTime { get; set; }

        /// <summary>
        /// <see cref="System.IO.FileSystemWatcher.InternalBufferSize"/>。
        /// </summary>
        [DataMember]
        public int BufferSize { get; set; }

        /// <summary>
        /// <see cref="System.IO.FileSystemWatcher"/> で取りこぼした際の更新時間。
        /// </summary>
        [DataMember]
        public TimeSpan RefreshTime { get; set; }
        /// <summary>
        /// そもそも取りこぼしを考慮するか。
        /// <para>将来用。</para>
        /// </summary>
        [DataMember]
        public bool IsEnabledRefresh { get; set; }

        #endregion
    }

}
