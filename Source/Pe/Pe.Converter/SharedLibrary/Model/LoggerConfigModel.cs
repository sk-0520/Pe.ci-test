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

namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
    /// <summary>
    /// ILoggerで使用する設定。
    /// </summary>
    [Serializable]
    public class LoggerConfigModel: ModelBase
    {
        #region proprty

        /// <summary>
        /// デバッグ情報をログ対象とするか。
        /// </summary>
        [DataMember]
        public bool EnabledDebug { get; set; }
        /// <summary>
        /// トレース情報をログ対象とするか。
        /// </summary>
        [DataMember]
        public bool EnabledTrace { get; set; }
        /// <summary>
        /// 操作情報をログ対象とするか。
        /// </summary>
        [DataMember]
        public bool EnabledInformation { get; set; }
        /// <summary>
        /// 注意をログ対象とするか。
        /// </summary>
        [DataMember]
        public bool EnabledWarning { get; set; }
        /// <summary>
        /// エラーをログ対象とするか。
        /// </summary>
        [DataMember]
        public bool EnabledError { get; set; }
        /// <summary>
        /// 異常をログ対象とするか。
        /// </summary>
        [DataMember]
        public bool EnabledFatal { get; set; }

        /// <summary>
        /// すべて。
        /// </summary>
        public bool EnabledAll
        {
            get
            {
                return EnabledDebug
                    && EnabledTrace
                    && EnabledInformation
                    && EnabledWarning
                    && EnabledError
                    && EnabledFatal
                ;
            }
            set
            {
                EnabledDebug
                    = EnabledTrace
                    = EnabledInformation
                    = EnabledWarning
                    = EnabledError
                    = EnabledFatal
                    = value
                ;
            }
        }
        /// <summary>
        /// ストリーム出力を行うか。
        /// </summary>
        [DataMember]
        public bool PutsStream { get; set; }
        /// <summary>
        /// コンソール出力を行うか。
        /// </summary>
        [DataMember]
        public bool PutsConsole { get; set; }
        /// <summary>
        /// デバッグ出力を行うか。
        /// </summary>
        [DataMember]
        public bool PutsDebug { get; set; }
        /// <summary>
        /// カスタム出力を行うか。
        /// </summary>
        [DataMember]
        public bool PutsCustom { get; set; }

        #endregion
    }
}
