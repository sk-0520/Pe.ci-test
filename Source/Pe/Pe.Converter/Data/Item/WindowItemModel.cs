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
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;

namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
    /// <summary>
    /// Windowsのウィンドウデータを保持する。
    /// </summary>
    public class WindowItemModel: PeDataBase
    {
        /// <summary>
        /// 対象プロセス。
        /// </summary>
        public Process Process { get; set; }
        /// <summary>
        /// 対象ウィンドウハンドル。
        /// </summary>
        public IntPtr WindowHandle { get; set; }
        /// <summary>
        /// ウィンドウの領域。
        /// </summary>
        [PixelKind(Px.Device)]
        public Rect WindowArea { get; set; }
        /// <summary>
        /// ウィンドウ状態。
        /// </summary>
        public WindowState WindowState { get; set; }

        #region IName

        [DataMember, XmlAttribute]
        public string Name { get; set; }

        #endregion
    }
}
