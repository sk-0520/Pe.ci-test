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
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF.Marker;

namespace ContentTypeTextNet.Library.SharedLibrary.IF.WindowsViewExtend
{
    public interface IWindowAreaCorrectionData: IWindowsViewExtendRestrictionViewModelMarker
    {
        /// <summary>
        /// ウィンドウサイズの倍数制御を行うか。
        /// </summary>
        bool UsingMultipleResize { get; }
        /// <summary>
        /// ウィンドウサイズの倍数制御に使用する元となる論理サイズ。
        /// </summary>
        [PixelKind(Px.Logical)]
        Size MultipleSize { get; }
        /// <summary>
        /// タイトルバーとかボーダーを含んだ領域。
        /// </summary>
        [PixelKind(Px.Logical)]
        Thickness MultipleThickness { get; }
        /// <summary>
        /// 移動制限を行うか。
        /// </summary>
        bool UsingMoveLimitArea { get; }
        /// <summary>
        /// 移動制限に使用する論理領域。
        /// </summary>
        [PixelKind(Px.Logical)]
        Rect MoveLimitArea { get; }
        /// <summary>
        /// 最大化・最小化を抑制するか。
        /// </summary>
        bool UsingMaxMinSuppression { get; }
    }
}
