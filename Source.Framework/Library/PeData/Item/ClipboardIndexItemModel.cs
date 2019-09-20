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
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.Define;

namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
    /// <summary>
    /// クリップボードのインデックス個別データ。
    /// </summary>
    public class ClipboardIndexItemModel: IndexItemModelBase
    {
        public ClipboardIndexItemModel()
            : base()
        {
            Sort = History.CreateTimestamp;
        }

        #region property

        /// <summary>
        /// 保持するクリップボードの型。
        /// </summary>
        [DataMember, IsDeepClone]
        public ClipboardType Type { get; set; }
        /// <summary>
        /// 自身のデータ(インデックス + ボディ)を示すハッシュデータ。
        /// </summary>
        [DataMember, IsDeepClone]
        public HashItemModel Hash { get; set; } = new HashItemModel();

        /// <summary>
        /// 並べ替えに用いる基準値。
        /// </summary>
        [DataMember, IsDeepClone]
        public DateTime Sort { get; set; }

        #endregion

        #region IndexItemModelBase

        //public override void DeepCloneTo(IDeepClone target)
        //{
        //    base.DeepCloneTo(target);

        //    var obj = (ClipboardIndexItemModel)target;

        //    obj.Type = Type;
        //    obj.Sort = Sort;
        //    //Hash.DeepCloneTo(obj.Hash);
        //    obj.Hash = (HashItemModel)Hash.DeepClone();
        //}

        public override IDeepClone DeepClone()
        {
            //var result = new ClipboardIndexItemModel();

            //DeepCloneTo(result);

            //return result;
            return DeepCloneUtility.Copy(this);
        }

        #endregion
    }
}
