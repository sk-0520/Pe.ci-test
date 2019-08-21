using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using ContentTypeTextNet.Pe.Core.Model;
using ContentTypeTextNet.Pe.Core.Model.Unmanaged;
using Drawing = System.Drawing;

namespace ContentTypeTextNet.Pe.Core.Compatibility.Forms
{
    /// <summary>
    /// <see cref="System.Drawing.Icon"/>のラッパー。
    /// </summary>
    public class IconWrapper : RawModel<Drawing.Icon>, IMakeBitmapSource
    {
        public IconWrapper(Stream stream, IconScale iconScale)
            : base(new Drawing.Icon(stream, DrawingUtility.Convert(iconScale.ToSize())))
        { }

        public IconWrapper(StreamResourceInfo streamInfo, IconScale iconScale)
            : this(streamInfo.Stream, iconScale)
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
