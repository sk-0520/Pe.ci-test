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
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.IF;

namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
    /// <summary>
    /// 呼び出し元データ。
    /// <para>一つずつ指定するのがだるい。</para>
    /// </summary>
    public struct CallerModel: IModel
    {
        #region variable

        string _callerFile;
        int _callerLine;
        string _callerMember;

        #endregion

        public CallerModel([CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
        {
            this._callerFile = callerFile;
            this._callerLine = callerLine;
            this._callerMember = callerMember;
        }

        #region property

        /// <summary>
        /// 呼び出しファイル名。
        /// </summary>
        public string CallerFile { get { return this._callerFile; } }
        /// <summary>
        /// 呼び出し行。
        /// </summary>
        public int CallerLine { get { return this._callerLine; } }
        /// <summary>
        /// 呼び出しメンバ名。
        /// </summary>
        public string CallerMember { get { return this._callerMember; } }

        #endregion

        #region IModel

        [IgnoreDataMember, XmlIgnore]
        public string DisplayText
        {
            get { return GetType().FullName; }
        }

        public void Correction()
        { }

        #endregion

    }
}
