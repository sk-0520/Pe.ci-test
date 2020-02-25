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

namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
    internal static class FreezableUtility
    {
        /// <summary>
        /// 安全に<see cref="Freezable.Freeze()"/>する。
        /// <para>どんな時にできないのかは知らん。</para>
        /// </summary>
        /// <param name="freezable"></param>
        /// <returns></returns>
        public static bool SafeFreeze(Freezable freezable)
        {
            var result = freezable.CanFreeze;
            if(result) {
                freezable.Freeze();
            }

            return result;
        }
    }
}
