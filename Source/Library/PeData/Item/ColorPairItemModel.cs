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
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Pe.Library.PeData.IF;

namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
    /// <summary>
    /// 前景色・背景色データ。
    /// </summary>
    [Serializable, DataContract]
    public class ColorPairItemModel: ItemModelBase, IDeepClone, IColorPair
    {
        public ColorPairItemModel()
            : base()
        { }

        public ColorPairItemModel(Color fore, Color back)
            : this()
        {
            ForeColor = fore;
            BackColor = back;
        }

        #region IColorPair

        /// <summary>
        /// 前景色。
        /// </summary>
        [DataMember, IsDeepClone]
        public Color ForeColor { get; set; }
        /// <summary>
        /// 背景色。
        /// </summary>
        [DataMember, IsDeepClone]
        public Color BackColor { get; set; }

        #endregion

        #region IDeepClone

        //public void DeepCloneTo(IDeepClone target)
        //{
        //    var obj = (ColorPairItemModel)target;

        //    obj.ForeColor = ForeColor;
        //    obj.BackColor = BackColor;
        //}

        public IDeepClone DeepClone()
        {
            //var result = new ColorPairItemModel();

            //DeepCloneTo(result);

            //return result;
            return DeepCloneUtility.Copy(this);
        }

        #endregion
    }
}
