/*
This file is part of SharedLibrary.

SharedLibrary is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharedLibrary is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SharedLibrary.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.IF;

namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
    /// <summary>
    /// 言語設定。
    /// </summary>
    [Serializable, XmlRoot("Language")]
    public class LanguageCollectionModel: ModelBase, IName
    {
        public LanguageCollectionModel()
            : base()
        {
            Define = new List<LanguageItemModel>();
            Words = new List<LanguageItemModel>();
        }

        #region property

        /// <summary>
        /// 言語コード。
        /// </summary>
        [DataMember, XmlAttribute]
        public string CultureCode { get; set; }

        /// <summary>
        /// 共通定義部。
        /// </summary>
        [DataMember, XmlArrayItem("Item")]
        public List<LanguageItemModel> Define { get; set; }
        /// <summary>
        /// 言語データ。
        /// </summary>
        [DataMember, XmlArrayItem("Item")]
        public List<LanguageItemModel> Words { get; set; }

        #endregion

        #region IName

        /// <summary>
        /// 言語名。
        /// </summary>
        [DataMember, XmlAttribute]
        public string Name { get; set; }

        #endregion
    }
}
