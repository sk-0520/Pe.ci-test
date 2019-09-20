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
    /// 言語データの最小データ。
    /// </summary>
    [Serializable]
    public class LanguageItemModel: ModelBase, ITId<string>
    {
        public LanguageItemModel()
            : base()
        { }

        #region property

        /// <summary>
        /// 表示用文字列。
        /// </summary>
        [DataMember, XmlAttribute]
        public string Word { get; set; }

        #endregion

        #region ITId

        /// <summary>
        /// キーとして使用される、
        /// </summary>
        [DataMember, XmlAttribute]
        public string Id { get; set; }

        public bool IsSafeId(string s)
        {
            return !string.IsNullOrEmpty(s);
        }

        public string ToSafeId(string s)
        {
            if(string.IsNullOrEmpty(s)) {
                return "id";
            }

            return s;
        }

        #endregion
    }
}
