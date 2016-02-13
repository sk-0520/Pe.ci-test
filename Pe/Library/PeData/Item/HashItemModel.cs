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
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Pe.Library.PeData.Converter;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using Newtonsoft.Json;

namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
    /// <summary>
    /// ハッシュデータ。
    /// </summary>
    [Serializable, DataContract]
    public class HashItemModel: ItemModelBase, IDeepClone, IIsEqual
    {
        public HashItemModel()
            : base()
        { }

        #region property

        /// <summary>
        /// ハッシュ関数。
        /// <para>SHA-1 一択だけど将来変更する場合の保険。</para>
        /// </summary>
        [DataMember]
        public HashType Type { get; set; }
        /// <summary>
        /// ハッシュ値。
        /// </summary>
        [DataMember, JsonConverter(typeof(ByteArrayConverter))]
        public byte[] Code { get; set; }

        #endregion

        #region IDeepClone

        public void DeepCloneTo(IDeepClone target)
        {
            var obj = (HashItemModel)target;

            obj.Type = Type;
            if(Code != null && Code.Any()) {
                obj.Code = new byte[Code.Length];
                Code.CopyTo(obj.Code, 0);
            }
        }

        public IDeepClone DeepClone()
        {
            var result = new HashItemModel();

            DeepCloneTo(result);

            return result;
        }

        #endregion

        #region IIsEqual

        public bool IsEqual(IIsEqual target)
        {
            var obj = target as HashItemModel;
            if(obj == null) {
                return false;
            }

            if(Type != obj.Type) {
                return false;
            }
            if(Code == obj.Code) {
                return true;
            }

            if(Code == null || obj.Code == null) {
                return false;
            }

            if(Code.Length != obj.Code.Length) {
                return false;
            }

            return Code.SequenceEqual(obj.Code);
        }

        #endregion
    }
}
