/**
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
using ContentTypeTextNet.Library.SharedLibrary.Model;
namespace ContentTypeTextNet.Library.SharedLibrary.IF
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;
    using ContentTypeTextNet.Library.SharedLibrary.Model;

    /// <summary>
    /// 範囲持ちアイテム。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class RangeModel<T>: ModelBase
    {
        public RangeModel()
        { }

        #region propert

        /// <summary>
        /// 範囲の開始点。
        /// </summary>
        [DataMember]
        public T Head { get; set; }
        /// <summary>
        /// 範囲の終了点。
        /// </summary>
        [DataMember]
        public T Tail { get; set; }

        #endregion
    }
}
