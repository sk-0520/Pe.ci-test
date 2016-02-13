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
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
    /// <summary>
    /// タグを管理。
    /// </summary>
    [Serializable]
    public class TagItemModel: PeDataBase, IDeepClone
    {
        public TagItemModel()
            : base()
        {
            Items = new CollectionModel<string>();
        }

        /// <summary>
        /// タグ。
        /// </summary>
        [DataMember, XmlArray("Items"), XmlArrayItem("Item")]
        public CollectionModel<string> Items { get; set; }

        #region IDeepClone

        public virtual void DeepCloneTo(IDeepClone target)
        {
            var obj = (TagItemModel)target;

            obj.Items.InitializeRange(Items);
        }

        public IDeepClone DeepClone()
        {
            var result = new TagItemModel();

            DeepCloneTo(result);

            return result;
        }

        #endregion
    }
}
