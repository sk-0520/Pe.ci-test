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
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
    /// <summary>
    /// 履歴保持アイテム。
    /// </summary>
    [Serializable]
    public class HistoryItemModel: ItemModelBase, IDeepClone
    {
        public HistoryItemModel()
            : base()
        {
            UpdateTimestamp = CreateTimestamp = DateTime.Now;
            UpdateCount = 0;
        }

        #region property

        /// <summary>
        /// 作成日。
        /// </summary>
        [DataMember, XmlAttribute]
        public DateTime CreateTimestamp { get; set; }
        /// <summary>
        /// 更新日。
        /// </summary>
        [DataMember, XmlAttribute]
        public DateTime UpdateTimestamp { get; set; }
        /// <summary>
        /// 更新回数。
        /// </summary>
        [DataMember, XmlAttribute]
        public uint UpdateCount { get; set; }

        #endregion

        #region function

        /// <summary>
        /// 更新。
        /// </summary>
        /// <param name="dateTime"></param>
        public virtual void Update(DateTime dateTime)
        {
            UpdateCount = RangeUtility.Increment(UpdateCount);
            UpdateTimestamp = DateTime.Now;
        }
        /// <summary>
        /// 更新。
        /// </summary>
        public void Update()
        {
            Update(DateTime.Now);
        }

        #endregion

        #region IDeepClone

        public virtual void DeepCloneTo(IDeepClone target)
        {
            var obj = (HistoryItemModel)target;

            obj.CreateTimestamp = CreateTimestamp;
            obj.UpdateTimestamp = UpdateTimestamp;
            obj.UpdateCount = UpdateCount;
        }

        public virtual IDeepClone DeepClone()
        {
            var result = new HistoryItemModel();

            DeepCloneTo(result);

            return result;
        }

        #endregion
    }
}
