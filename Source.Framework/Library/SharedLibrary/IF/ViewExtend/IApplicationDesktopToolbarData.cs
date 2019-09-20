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
using System.Windows;
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF.Marker;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.Library.SharedLibrary.IF.WindowsViewExtend
{
    public interface IApplicationDesktopToolbarData: IWindowsViewExtendRestrictionViewModelMarker
    {
        uint CallbackMessage { get; set; }
        /// <summary>
        /// 他ウィンドウがフルスクリーン表示。
        /// </summary>
        bool NowFullScreen { get; set; }
        /// <summary>
        /// ドッキング中か。
        /// </summary>
        bool IsDocking { get; set; }
        /// <summary>
        /// ドッキング種別。
        /// </summary>
        DockType DockType { get; set; }
        /// <summary>
        /// 自動的に隠す。
        /// </summary>
        bool AutoHide { get; set; }
        /// <summary>
        /// 隠れているか。
        /// </summary>
        bool IsHidden { get; set; }
        /// <summary>
        /// 表示状態。
        /// </summary>
        Visibility Visibility { get; }
        /// <summary>
        /// バーの論理サイズ
        /// </summary>
        [PixelKind(Px.Logical)]
        Size BarSize { get; set; }
        /// <summary>
        /// 表示中の論理バーサイズ。
        /// </summary>
        [PixelKind(Px.Logical)]
        Rect ShowLogicalBarArea { get; set; }
        /// <summary>
        /// 隠れた状態のバー論理サイズ。
        /// </summary>
        [PixelKind(Px.Logical)]
        double HideWidth { get; }
        /// <summary>
        /// 表示中の隠れたバーの論理領域。
        /// </summary>
        [PixelKind(Px.Logical)]
        Rect HideLogicalBarArea { get; set; }
        /// <summary>
        /// 自動的に隠すまでの時間。
        /// </summary>
        TimeSpan HideWaitTime { get; }
        /// <summary>
        /// 自動的に隠す際のアニメーション時間。
        /// </summary>
        TimeSpan HideAnimateTime { get; }
        /// <summary>
        /// ドッキングに使用するスクリーン。
        /// </summary>
        ScreenModel DockScreen { get; }

        /// <summary>
        /// 指定位置に合わせてデータ書き換え
        /// </summary>
        void ChangingWindowMode(DockType dockType);
    }
}
