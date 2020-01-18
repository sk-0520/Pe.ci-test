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

namespace ContentTypeTextNet.Library.SharedLibrary.Define
{
    /// <summary>
    /// ログ種別。
    /// </summary>
    public enum LogKind
    {
        /// <summary>
        /// 基本的に使用しない。
        /// </summary>
        None,
        /// <summary>
        /// デバッグ情報。
        /// </summary>
        Debug,
        /// <summary>
        /// トレース情報。
        /// </summary>
        Trace,
        /// <summary>
        /// 操作情報。
        /// </summary>
        Information,
        /// <summary>
        /// 注意。
        /// </summary>
        Warning,
        /// <summary>
        /// エラー。
        /// </summary>
        Error,
        /// <summary>
        /// 異常。
        /// </summary>
        Fatal
    }
}
