/*
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Converter
{
    public class ToolbarDockTypeImageConverter: IValueConverter
    {
        readonly Size imageSize = new Size(IconScale.Small.ToWidth() * 2, IconScale.Small.ToHeight());

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var dockType = CastUtility.GetCastWPFValue<DockType>(value, DockType.None);
            var usingImageSize = imageSize;
            if(parameter is Size) {
                usingImageSize = (Size)parameter;
            }
            var element = LauncherToolbarUtility.MakeDockIcon(dockType, usingImageSize);
            return element;
            //return ImageUtility.MakeBitmapBitmapSourceDefualtDpi(element);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
