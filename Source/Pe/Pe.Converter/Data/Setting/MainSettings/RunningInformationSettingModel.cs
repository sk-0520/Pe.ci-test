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
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.PeMain.Converter;
using Newtonsoft.Json;

namespace ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings
{
    /// <summary>
    /// 実行情報。
    /// </summary>
    [Serializable]
    public class RunningInformationSettingModel: SettingModelBase
    {
        public RunningInformationSettingModel()
            : base()
        {
            FirstRunning = new FirstRunningItemModel();
        }

        #region property

        /// <summary>
        /// 実行が許可されているか。
        /// </summary>
        [DataMember, IsDeepClone]
        public bool Accept { get; set; }
        /// <summary>
        /// 前回終了時のバージョン。
        /// </summary>
        [DataMember, IsDeepClone]
        [JsonConverter(typeof(JsonVersionConverter))]
        public Version LastExecuteVersion { get; set; }
        /// <summary>
        /// アップデートチェックを行うか。
        /// </summary>
        [DataMember, IsDeepClone]
        public bool CheckUpdateRelease { get; set; }
        /// <summary>
        /// RCアップデートチェックを行うか。
        /// </summary>
        [DataMember, IsDeepClone]
        public bool CheckUpdateRC { get; set; }
        /// <summary>
        /// アップデートチェックで無視するバージョン。
        /// </summary>
        [DataMember, IsDeepClone]
        [JsonConverter(typeof(JsonVersionConverter))]
        public Version IgnoreUpdateVersion { get; set; }
        /// <summary>
        /// プログラム実行回数。
        /// </summary>
        [DataMember, IsDeepClone]
        public int ExecuteCount { get; set; }

        /// <summary>
        /// ユーザー識別子。
        /// </summary>
        [DataMember, IsDeepClone]
        public string UserId { get; set; }

        /// <summary>
        /// 初回起動情報。
        /// </summary>
        [DataMember, IsDeepClone]
        public FirstRunningItemModel FirstRunning { get; set; }

        /// <summary>
        /// ユーザー情報を送信しても良いか。
        /// </summary>
        [DataMember, IsDeepClone]
        public bool SendPersonalInformation { get; set; }

        #endregion

        #region IDeepClone

        //public void DeepCloneTo(IDeepClone target)
        //{
        //    var obj = (RunningInformationSettingModel)target;

        //    obj.Accept = Accept;
        //    if(LastExecuteVersion != null) {
        //        obj.LastExecuteVersion = (Version)LastExecuteVersion.Clone();
        //    }
        //    obj.CheckUpdateRelease = CheckUpdateRelease;
        //    obj.CheckUpdateRC = CheckUpdateRC;
        //    if(IgnoreUpdateVersion != null) {
        //        obj.IgnoreUpdateVersion = (Version)IgnoreUpdateVersion.Clone();
        //    }
        //    obj.ExecuteCount = ExecuteCount;
        //}

        #endregion
    }
}
