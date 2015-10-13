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
namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using System.Xml.Serialization;
    using ContentTypeTextNet.Library.SharedLibrary.IF;

    [Serializable]
    public class HotKeyModel: ModelBase, IDeepClone
    {
        public HotKeyModel()
            : base()
        {
            Key = Key.None;
            ModifierKeys = ModifierKeys.None;

            IsRegistered = false;
        }

        #region property

        [DataMember]
        public ModifierKeys ModifierKeys { get; set; }
        [DataMember]
        public Key Key { get; set; }

        /// <summary>
        /// 登録されているか
        /// </summary>
        [XmlIgnore, IgnoreDataMember]
        public bool IsRegistered { get; set; }

        /// <summary>
        /// 有効なキー設定か。
        /// </summary>
        [XmlIgnore, IgnoreDataMember]
        public bool IsEnabled
        {
            get
            {
                return Key != Key.None && ModifierKeys != ModifierKeys.None;
            }
        }
        #endregion

        #region IDeepClone

        public virtual void DeepCloneTo(IDeepClone target)
        {
            var obj = (HotKeyModel)target;

            obj.Key = Key;
            obj.ModifierKeys = ModifierKeys;
            obj.IsRegistered = IsRegistered;
        }

        public IDeepClone DeepClone()
        {
            var result = new HotKeyModel();

            DeepCloneTo(result);

            return result;
        }

        #endregion
    }
}
