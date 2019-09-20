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
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Pe.Library.PeData.Define;

namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
    /// <summary>
    /// クリップボード取込制限データ。
    /// </summary>
    [DataContract, Serializable]
    public class ClipboardLimitSizeItemModel: ItemModelBase, IDeepClone
    {
        /// <summary>
        /// 取り込み制限対象。
        /// </summary>
        [DataMember, IsDeepClone]
        public ClipboardType LimitType { get; set; }
        /// <summary>
        /// テキストデータの取込有効長。
        /// </summary>
        [DataMember, IsDeepClone]
        public int Text { get; set; }
        /// <summary>
        /// RTFデータの取込有効長。
        /// </summary>
        [DataMember, IsDeepClone]
        public int Rtf { get; set; }
        /// <summary>
        /// HTMLデータの取込有効長。
        /// </summary>
        [DataMember, IsDeepClone]
        public int Html { get; set; }
        /// <summary>
        /// 画像データの取込有効幅。
        /// </summary>
        [DataMember, IsDeepClone]
        public int ImageWidth { get; set; }
        /// <summary>
        /// 画像データの取込有効高。
        /// </summary>
        [DataMember, IsDeepClone]
        public int ImageHeight { get; set; }

        #region IDeepClone

        //public void DeepCloneTo(IDeepClone target)
        //{
        //    var obj = (ClipboardLimitSizeItemModel)target;

        //    obj.LimitType = LimitType;
        //    obj.Text = Text;
        //    obj.Rtf = Rtf;
        //    obj.Html = Html;
        //    obj.ImageWidth = ImageWidth;
        //    obj.ImageHeight = ImageHeight;
        //}

        public IDeepClone DeepClone()
        {
            //var result = new ClipboardLimitSizeItemModel();

            //DeepCloneTo(result);

            //return result;
            return DeepCloneUtility.Copy(this);
        }

        #endregion


    }
}
