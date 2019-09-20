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

namespace ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility
{
    /// <summary>
    /// POD構造体用ユーティリティ。
    /// </summary>
    public static class PodStructUtility
    {
        public static RECT Convert(Rect rect)
        {
            var result = new RECT() {
                Left = (int)rect.Left,
                Top = (int)rect.Top,
                Width = (int)rect.Width,
                Height = (int)rect.Height,
            };

            return result;
        }

        public static Rect Convert(RECT rect)
        {
            var result = new Rect(
                rect.Left,
                rect.Top,
                rect.Width,
                rect.Height
            );

            return result;
        }

        public static SIZE Convert(Size size)
        {
            return new SIZE((int)size.Width, (int)size.Height);
        }

        public static Size Convert(SIZE size)
        {
            return new Size(size.cx, size.cy);
        }

        public static POINT Convert(Point point)
        {
            return new POINT((int)point.X, (int)point.Y);
        }

        public static Point Convert(POINT point)
        {
            return new Point(point.X, point.Y);
        }
    }
}
