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
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
    /// <summary>
    /// 初回起動情報の保持。
    /// </summary>
    [Serializable, DataContract]
    public class FirstRunningItemModel: ItemModelBase, IDeepClone
    {
        #region property

        /// <summary>
        /// 初回起動時のタイムスタンプ。
        /// </summary>
        [DataMember, IsDeepClone]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 初回起動時のバージョン。
        /// </summary>
        [DataMember, IsDeepClone]
        public Version Version { get; set; }

        #endregion

        #region IDeepClone

        public IDeepClone DeepClone()
        {
            return DeepCloneUtility.Copy(this);
        }

        #endregion
    }
}
