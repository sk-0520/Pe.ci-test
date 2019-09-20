/*
This file is part of SharedLibrary.

SharedLibrary is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharedLibrary is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SharedLibrary.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Model.Unmanaged.Gdi;
using Drawing = System.Drawing;

namespace ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows
{
    /// <summary>
    /// <see cref="System.Drawing.Icon"/>のラッパー。
    /// </summary>
    public class IconWrapper: DisposeFinalizeBase, IMakeBitmapSource
    {
        IconWrapper(Drawing.Icon icon)
        {
            Icon = icon;
        }

        public IconWrapper(Stream stream, IconScale iconScale)
            : this(new Drawing.Icon(stream, DrawingUtility.Convert(iconScale.ToSize())))
        { }

        public IconWrapper(StreamResourceInfo streamInfo, IconScale iconScale)
            : this(streamInfo.Stream, iconScale)
        { }

        public IconWrapper(string applicationResourcePath, IconScale iconScale)
            : this(Application.GetResourceStream(SharedConstants.GetPackUri(applicationResourcePath)), iconScale)
        {
            var uri = SharedConstants.GetPackUri(applicationResourcePath);
        }

        #region property

        Drawing.Icon Icon { get; set; }

        #endregion

        #region IMakeBitmapSource

        public BitmapSource MakeBitmapSource()
        {
            using(var hIcon = new IconHandleModel(Icon.Handle)) {
                return hIcon.MakeBitmapSource();
            }
        }

        #endregion
    }
}
