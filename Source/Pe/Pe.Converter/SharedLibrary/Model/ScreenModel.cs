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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.Define;

namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
    /// <summary>
    /// 使用データは全て物理ピクセル。
    /// </summary>
    [Serializable]
    public class ScreenModel: ModelBase
    {
        #region property

        /// <summary>
        /// 1 ピクセルのデータに関連付けられているメモリのビット数を取得します。
        /// </summary>
        public int BitsPerPixel { get; protected internal set; }
        /// <summary>
        /// ディスプレイの範囲を取得します。
        /// </summary>
        [PixelKind(Px.Device)]
        public Rect DeviceBounds { get; protected internal set; }
        /// <summary>
        /// ディスプレイに関連付けられているデバイス名を取得します。
        /// </summary>
        public string DeviceName { get; protected internal set; }
        /// <summary>
        /// 特定のディスプレイがプライマリ デバイスかどうかを示す値を取得します。
        /// </summary>
        public bool Primary { get; protected internal set; }
        /// <summary>
        /// ディスプレイの作業領域を取得します。 作業領域とは、ディスプレイのデスクトップ領域からタスクバー、ドッキングされたウィンドウ、およびドッキングされたツール バーを除いた部分です。 
        /// </summary>
        [PixelKind(Px.Device)]
        public Rect DeviceWorkingArea { get; protected internal set; }

        #endregion
    }
}
