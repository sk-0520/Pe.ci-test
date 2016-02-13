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
using ContentTypeTextNet.Pe.Library.PeData.Define;

namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
    /// <summary>
    /// コンポーネント情報。
    /// </summary>
    [Serializable]
    [XmlType("Component"), XmlRoot("Component")]
    public class ComponentItemModel: ItemModelBase, IName
    {
        public ComponentItemModel()
        { }

        #region property

        /// <summary>
        /// コンポーネント種別。
        /// </summary>
        [DataMember, XmlAttribute]
        public ComponentKind Kind { get; set; }
        /// <summary>
        /// URI。
        /// </summary>
        [DataMember, XmlAttribute]
        public string Uri { get; set; }
        /// <summary>
        /// ライセンス。
        /// </summary>
        [DataMember, XmlAttribute]
        public string License { get; set; }

        #endregion

        #region IName

        /// <summary>
        /// コンポーネント名。
        /// </summary>
        [DataMember, XmlAttribute]
        public string Name { get; set; }

        #endregion

    }
}
