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
using System.Windows.Media;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.IF;

namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
    /// <summary>
    /// ノートインデックスのヘッダ部。
    /// </summary>
    [Serializable]
    public class NoteIndexItemModel: IndexItemModelBase, IWindowArea, ITopMost, IVisible, IColorPair
    {
        public NoteIndexItemModel()
            : base()
        {
            Font = new FontModel();
        }

        #region property

        /// <summary>
        /// ノート種別。
        /// </summary>
        [DataMember]
        public NoteKind NoteKind { get; set; }

        /// <summary>
        /// 固定されているか。
        /// </summary>
        [DataMember]
        public bool IsLocked { get; set; }
        /// <summary>
        /// 最小化されているか。
        /// </summary>
        [DataMember]
        public bool IsCompacted { get; set; }
        /// <summary>
        /// フォント情報。
        /// </summary>
        [DataMember]
        public FontModel Font { get; set; }
        /// <summary>
        /// 自動改行するか。
        /// </summary>
        [DataMember]
        public bool AutoLineFeed { get; set; }
        #endregion

        #region IColorPair

        [DataMember]
        public Color ForeColor { get; set; }
        [DataMember]
        public Color BackColor { get; set; }

        #endregion

        #region IWindowArea

        [DataMember]
        [PixelKind(Px.Logical)]
        public double WindowTop { get; set; }
        [DataMember]
        [PixelKind(Px.Logical)]
        public double WindowLeft { get; set; }
        [DataMember]
        [PixelKind(Px.Logical)]
        public double WindowWidth { get; set; }
        [DataMember]
        [PixelKind(Px.Logical)]
        public double WindowHeight { get; set; }

        #endregion

        #region ITopMost

        [DataMember]
        public bool IsTopmost { get; set; }

        #endregion

        #region IVisible

        [DataMember]
        public bool IsVisible { get; set; }

        #endregion

        #region IndexItemModelBase

        public override void DeepCloneTo(IDeepClone target)
        {
            base.DeepCloneTo(target);

            var obj = (NoteIndexItemModel)target;

            obj.NoteKind = NoteKind;
            obj.IsLocked = IsLocked;
            obj.IsCompacted = IsCompacted;
            obj.AutoLineFeed = AutoLineFeed;
            //Font.DeepCloneTo(obj.Font);
            obj.Font = (FontModel)Font.DeepClone();

            obj.ForeColor = ForeColor;
            obj.BackColor = BackColor;

            obj.WindowTop = WindowTop;
            obj.WindowLeft = WindowLeft;
            obj.WindowWidth = WindowWidth;
            obj.WindowHeight = WindowHeight;

            obj.IsTopmost = IsTopmost;

            obj.IsVisible = IsVisible;
        }

        public override IDeepClone DeepClone()
        {
            var result = new NoteIndexItemModel();

            DeepCloneTo(result);

            return result;
        }

        #endregion
    }
}
