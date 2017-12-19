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
using Forms = System.Windows.Forms;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.Define;

namespace ContentTypeTextNet.Library.SharedLibrary.CompatibleForms
{
    /// <summary>
    /// <see cref="System.Windows.Forms.SystemInformation" /> の互換ラッパー。
    /// </summary>
    public static class SystemInformation
    {
        /// <summary>
        /// マウス操作がダブルクリックであると OS に認識されるための、1 回目のクリックと 2 回目のクリックの間の最大経過時間 (ミリ秒単位) を取得します。
        /// </summary>
        public static TimeSpan DoubleClickTime
        {
            get { return TimeSpan.FromMilliseconds(Forms.SystemInformation.DoubleClickTime); }
        }
    }
}
