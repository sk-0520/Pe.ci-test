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
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Logic;

namespace ContentTypeTextNet.Library.SharedLibrary.IF
{
    /// <summary>
    /// 各種操作なんかで持ち運びたい処理しない(ように見える)データ群。
    /// </summary>
    public interface INonProcess
    {
        /// <summary>
        /// ログ取り。
        /// </summary>
        ILogger Logger { get; }
        /// <summary>
        /// 言語。
        /// </summary>
        ILanguage Language { get; }
    }
}
