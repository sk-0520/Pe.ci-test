using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Unmanaged;
using Drawing = System.Drawing;

namespace ContentTypeTextNet.Pe.Core.Compatibility.Forms
{
    /// <summary>
    /// <see cref="System.Drawing.Icon"/>のラッパー。
    /// </summary>
    public class IconWrapper : RawModel<Drawing.Icon>, IMakeBitmapSource
    {
        public IconWrapper(Stream stream, IconSize iconSize)
            : base(new Drawing.Icon(stream, DrawingUtility.Convert(iconSize.ToSize())))
        { }

        public IconWrapper(StreamResourceInfo streamInfo, IconSize iconSize)
            : this(streamInfo.Stream, iconSize)
        { }


        #region property

        public Drawing.Icon Icon => Raw;

        #endregion

        #region IMakeBitmapSource

        public BitmapSource MakeBitmapSource()
        {
            using(var hIcon = new IconHandleWrapper(Icon.Handle)) {
                return hIcon.MakeBitmapSource();
            }
        }

        #endregion
    }
}
