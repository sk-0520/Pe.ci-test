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
    /// コマンドと条件式のどちらを使用するか。
    /// </summary>
    public enum QueryPattern
    {
        /// <summary>
        /// コマンドを使用する。
        /// </summary>
        Command,
        /// <summary>
        /// 条件式を使用する。
        /// <para>入れ子で使用する。</para>
        /// </summary>
        Expression,
    }
}
