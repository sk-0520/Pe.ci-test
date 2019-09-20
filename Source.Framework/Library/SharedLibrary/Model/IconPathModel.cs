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
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
    /// <summary>
    /// アイコンのパスを保持。
    /// </summary>
    [DataContract, Serializable]
    public class IconPathModel: ModelBase, IDeepClone
    {
        #region property

        /// <summary>
        /// パス。
        /// </summary>
        [DataMember, IsDeepClone]
        public string Path { get; set; }
        /// <summary>
        /// アイコンインデックス。
        /// </summary>
        [DataMember, IsDeepClone]
        public int Index { get; set; }

        #endregion

        #region ModelBase

        public override string DisplayText
        {
            get
            {
                if(string.IsNullOrWhiteSpace(Path)) {
                    if(Index > 0) {
                        return $":{nameof(Index)} = {Index}";
                    } else {
                        return string.Empty;
                    }
                } else {
                    return $"{Path},{Index}";
                }
            }
        }

        #endregion

        #region IDeepClone

        public virtual IDeepClone DeepClone()
        {
            return DeepCloneUtility.Copy(this);
        }

        #endregion
    }
}
