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
using System.Windows;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Pe.Library.PeData.Item;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
    public static class NoteUtility
    {
        /// <summary>
        /// ノートメニュー表示テキスト
        /// </summary>
        /// <param name="indexItem"></param>
        /// <returns></returns>
        public static string MakeMenuText(NoteIndexItemModel indexItem)
        {
            return DisplayTextUtility.GetDisplayName(indexItem);
        }
        /// <summary>
        /// ノートメニューアイコンの生成。
        /// TODO: 未実装
        /// </summary>
        /// <param name="indexItem"></param>
        /// <returns></returns>
        public static FrameworkElement MakeMenuIcon(NoteIndexItemModel indexItem)
        {
            var size = IconScale.Small.ToSize();
            if(indexItem.IsCompacted) {
                size.Height /= 3;
            }
            var element = ImageUtility.CreateBox(indexItem.ForeColor, indexItem.BackColor, size);
            //var image = ImageUtility.MakeBitmapBitmapSourceDefualtDpi(element);
            //return image;
            return element;
        }
    }
}
