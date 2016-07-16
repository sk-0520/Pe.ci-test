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
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.Item;

namespace ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings
{
    [Serializable]
    public class SystemEnvironmentSettingModel: SettingModelBase, IDeepClone
    {
        public SystemEnvironmentSettingModel()
            : base()
        { }

        #region property

        /// <summary>
        /// 隠しファイル表示切り替えホットキー。
        /// </summary>
        [DataMember]
        public HotKeyModel HideFileHotkey { get; set; } = new HotKeyModel();
        /// <summary>
        /// 拡張子表示切替ホットキー。
        /// </summary>
        [DataMember]
        public HotKeyModel ExtensionHotkey { get; set; } = new HotKeyModel();
        /// <summary>
        /// F1キーを抑制するか。
        /// </summary>
        [DataMember]
        public bool SuppressFunction1Key { get; set; }

        #endregion

        #region IDeepClone

        public void DeepCloneTo(IDeepClone target)
        {
            var obj = (SystemEnvironmentSettingModel)target;

            obj.HideFileHotkey = (HotKeyModel)HideFileHotkey.DeepClone();
            obj.ExtensionHotkey = (HotKeyModel)ExtensionHotkey.DeepClone();
            obj.SuppressFunction1Key = SuppressFunction1Key;
        }

        public IDeepClone DeepClone()
        {
            var result = new SystemEnvironmentSettingModel();

            DeepCloneTo(result);

            return result;
        }

        #endregion
    }
}
