/**
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

namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
    /// <summary>
    /// フロート状態ツールバー設定データ。
    /// </summary>
    [Serializable, DataContract]
    public class FloatToolbarItemModel: ItemModelBase, IDeepClone
    {
        public FloatToolbarItemModel()
            : base()
        { }

        /// <summary>
        /// 横に表示するアイテム数。
        /// </summary>
        [DataMember, IsDeepClone]
        public int WidthButtonCount { get; set; }
        /// <summary>
        /// 縦に表示するアイテム数。
        /// </summary>
        [DataMember, IsDeepClone]
        public int HeightButtonCount { get; set; }
        /// <summary>
        /// 論理X座標。
        /// </summary>
        [DataMember, IsDeepClone]
        [PixelKind(Px.Logical)]
        public double Left { get; set; }
        /// <summary>
        /// 論理Y座標。
        /// </summary>
        [DataMember, IsDeepClone]
        [PixelKind(Px.Logical)]
        public double Top { get; set; }

        #region IDeepClone

        //public void DeepCloneTo(IDeepClone target)
        //{
        //    var obj = (FloatToolbarItemModel)target;

        //    obj.WidthButtonCount = WidthButtonCount;
        //    obj.HeightButtonCount = HeightButtonCount;
        //    obj.Left = Left;
        //    obj.Top = Top;
        //}

        public IDeepClone DeepClone()
        {
            //var result = new FloatToolbarItemModel();

            //DeepCloneTo(result);

            //return result;
            return DeepCloneUtility.Copy(this);
        }

        #endregion
    }
}
