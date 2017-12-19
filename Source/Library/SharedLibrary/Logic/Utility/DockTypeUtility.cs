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
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Library.SharedLibrary.Define;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
    public static class DockTypeUtility
    {
        /// <summary>
        /// ABEへ変換。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ABE ToABE(DockType type)
        {
            switch(type) {
                case DockType.Left: return ABE.ABE_LEFT;
                case DockType.Top: return ABE.ABE_TOP;
                case DockType.Bottom: return ABE.ABE_BOTTOM;
                case DockType.Right: return ABE.ABE_RIGHT;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// DesktopDockTypeへ変換。
        /// </summary>
        /// <param name="abe"></param>
        /// <returns></returns>
        public static DockType ToDockType(ABE abe)
        {
            switch(abe) {
                case ABE.ABE_LEFT: return DockType.Left;
                case ABE.ABE_TOP: return DockType.Top;
                case ABE.ABE_RIGHT: return DockType.Right;
                case ABE.ABE_BOTTOM: return DockType.Bottom;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
