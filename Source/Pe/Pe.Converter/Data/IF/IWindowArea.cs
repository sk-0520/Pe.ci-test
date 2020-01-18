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
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.Define;

namespace ContentTypeTextNet.Pe.Library.PeData.IF
{
    /// <summary>
    /// ウィンドウ位置を保持する。
    /// </summary>
    public interface IWindowArea
    {
        /// <summary>
        /// 論理ウィンドウ座標(X)
        /// </summary>
        [PixelKind(Px.Logical)]
        double WindowLeft { get; set; }
        /// <summary>
        /// 論理ウィンドウ座標(Y)
        /// </summary>
        [PixelKind(Px.Logical)]
        double WindowTop { get; set; }
        /// <summary>
        /// 論理ウィンドウサイズ(横)
        /// </summary>
        [PixelKind(Px.Logical)]
        double WindowWidth { get; set; }
        /// <summary>
        /// 論理ウィンドウサイズ(縦)
        /// </summary>
        [PixelKind(Px.Logical)]
        double WindowHeight { get; set; }
    }
}
