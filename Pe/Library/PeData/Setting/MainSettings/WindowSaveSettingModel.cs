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
using ContentTypeTextNet.Pe.Library.PeData.Item;

namespace ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings
{
    /// <summary>
    /// ウィンドウ状態復元設定。
    /// </summary>
    [Serializable]
    public class WindowSaveSettingModel: SettingModelBase, IDeepClone
    {
        #region property

        /// <summary>
        /// 有効。
        /// </summary>
        [DataMember]
        public bool IsEnabled { get; set; }
        /// <summary>
        /// 保存数。
        /// </summary>
        [DataMember]
        public int SaveCount { get; set; }
        /// <summary>
        /// 保存間隔。
        /// </summary>
        [DataMember]
        public TimeSpan SaveIntervalTime { get; set; }

        #endregion

        #region IDeepClone

        public void DeepCloneTo(IDeepClone target)
        {
            var obj = (WindowSaveSettingModel)target;

            obj.IsEnabled = IsEnabled;
            obj.SaveCount = SaveCount;
            obj.SaveIntervalTime = SaveIntervalTime;
        }

        public IDeepClone DeepClone()
        {
            var result = new WindowSaveSettingModel();

            DeepCloneTo(result);

            return result;
        }

        #endregion
    }
}
