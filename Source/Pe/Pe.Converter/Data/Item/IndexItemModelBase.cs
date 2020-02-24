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
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
    /// <summary>
    /// インデックスデータのヘッダ部。
    /// </summary>
    public abstract class IndexItemModelBase: GuidModelBase, IDeepClone
    {
        public IndexItemModelBase()
            : base()
        { }

        #region IName

        /// <summary>
        /// 名前。
        /// </summary>
        [DataMember, XmlAttribute, IsDeepClone]
        public string Name { get; set; }

        #endregion

        #region property

        /// <summary>
        /// 履歴。
        /// </summary>
        [DataMember, IsDeepClone]
        public HistoryItemModel History { get; set; } = new HistoryItemModel();

        #endregion

        #region IDeepClone

        //public override void DeepCloneTo(IDeepClone target)
        //{
        //    base.DeepCloneTo(target);

        //    var obj = (IndexItemModelBase)target;

        //    obj.Name = Name;
        //    obj.History = (HistoryItemModel)History.DeepClone();
        //}

        public abstract IDeepClone DeepClone();

        #endregion
    }
}
