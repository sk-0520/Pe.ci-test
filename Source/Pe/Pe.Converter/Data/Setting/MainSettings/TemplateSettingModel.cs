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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.IF;
using ContentTypeTextNet.Pe.Library.PeData.Item;

namespace ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings
{
    [Serializable]
    public class TemplateSettingModel: SettingModelBase, IWindowStatus
    {
        public TemplateSettingModel()
            : base()
        { }

        #region property

        /// <summary>
        /// 表示非表示切り替え。
        /// </summary>
        [DataMember, IsDeepClone]
        public HotKeyModel ToggleHotKey { get; set; } = new HotKeyModel();

        /// <summary>
        /// リスト部の幅。
        /// </summary>
        [DataMember, IsDeepClone]
        public double ItemsListWidth { get; set; }

        /// <summary>
        /// 置き換えリスト部の幅。
        /// </summary>
        [DataMember, IsDeepClone]
        public double ReplaceListWidth { get; set; }

        [DataMember, IsDeepClone]
        public FontModel Font { get; set; } = new FontModel();

        /// <summary>
        /// アイテムダブルクリック時の処理。
        /// </summary>
        [DataMember, IsDeepClone]
        public IndexItemsDoubleClickBehavior DoubleClickBehavior { get; set; }

        #endregion

        #region IWindowStatus

        [DataMember, IsDeepClone]
        [PixelKind(Px.Logical)]
        public double WindowTop { get; set; }
        [DataMember, IsDeepClone]
        [PixelKind(Px.Logical)]
        public double WindowLeft { get; set; }
        [DataMember, IsDeepClone]
        [PixelKind(Px.Logical)]
        public double WindowWidth { get; set; }
        [DataMember, IsDeepClone]
        [PixelKind(Px.Logical)]
        public double WindowHeight { get; set; }
        [DataMember, IsDeepClone]
        [PixelKind(Px.Logical)]
        public WindowState WindowState { get; set; }

        #region ITopMost

        [DataMember, IsDeepClone]
        public bool IsTopmost { get; set; }

        #endregion

        #region IVisible

        [DataMember, IsDeepClone]
        public bool IsVisible { get; set; }

        #endregion

        #endregion

        #region IDeepClone

        //public void DeepCloneTo(IDeepClone target)
        //{
        //    var obj = (TemplateSettingModel)target;

        //    obj.ToggleHotKey = (HotKeyModel)ToggleHotKey.DeepClone();
        //    obj.ItemsListWidth = ItemsListWidth;
        //    obj.ReplaceListWidth = ReplaceListWidth;
        //    obj.WindowTop = WindowTop;
        //    obj.WindowLeft = WindowLeft;
        //    obj.WindowWidth = WindowWidth;
        //    obj.WindowHeight = WindowHeight;
        //    obj.WindowState = WindowState;
        //    obj.IsTopmost = IsTopmost;
        //    obj.IsVisible = IsVisible;
        //    //Font.DeepCloneTo(obj.Font);
        //    obj.Font = (FontModel)Font.DeepClone();
        //    obj.DoubleClickBehavior = DoubleClickBehavior;
        //}

        #endregion
    }
}
