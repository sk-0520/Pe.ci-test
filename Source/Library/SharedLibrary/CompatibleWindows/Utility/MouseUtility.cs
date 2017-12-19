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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using Drawing = System.Drawing;

namespace ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility
{
    public class MouseUtility
    {
        /// <summary>
        /// マウスカーソルの現在位置を物理座標で取得。
        /// </summary>
        /// <returns></returns>
        [return: PixelKind(Px.Device)]
        public static Point GetDevicePosition()
        {
            var deviceCursolPosition = new POINT();
            NativeMethods.GetCursorPos(out deviceCursolPosition);

            return PodStructUtility.Convert(deviceCursolPosition);
        }
    }
}
