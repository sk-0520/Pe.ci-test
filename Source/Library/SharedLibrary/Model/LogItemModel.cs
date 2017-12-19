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
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Define;

namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
    /// <summary>
    /// ログとして出力するデータ。
    /// </summary>
    [Serializable]
    public class LogItemModel: ModelBase
    {
        #region property

        /// <summary>
        /// 発生日。
        /// </summary>
        [DataMember]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ログ種別。
        /// </summary>
        [DataMember]
        public LogKind LogKind { get; set; }
        /// <summary>
        /// ログメッセージ。
        /// </summary>
        [DataMember]
        public string Message { get; set; }
        /// <summary>
        /// 詳細。
        /// <para>nullで何もなし。</para>
        /// <para>判定にはHasDetailを使用。</para>
        /// </summary>
        [DataMember]
        public string Detail { get; set; }
        /// <summary>
        /// 詳細はあるか。
        /// </summary>
        public bool HasDetail { get { return Detail != null; } }
        public string DetailText
        {
            get
            {
                if(HasDetail) {
                    return Detail;
                } else {
                    return string.Empty;
                }
            }
        }
        /// <summary>
        /// スタックトレース情報。
        /// </summary>
        [DataMember]
        public StackTrace StackTrace { get; set; }
        /// <summary>
        /// 呼び出しファイルパス。
        /// </summary>
        [DataMember]
        public string CallerFile { get; set; }
        /// <summary>
        /// 呼び出し行番号。
        /// </summary>
        [DataMember]
        public int CallerLine { get; set; }
        /// <summary>
        /// 呼び出しメンバ。
        /// </summary>
        [DataMember]
        public string CallerMember { get; set; }
        /// <summary>
        /// 呼び出しアセンブリ。
        /// </summary>
        [DataMember]
        public Assembly CallerAssembly { get; set; }
        /// <summary>
        /// 呼び出しスレッド。
        /// </summary>
        [DataMember]
        public Thread CallerThread { get; set; }

        #endregion
    }
}
