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
    /// 並び順。
    /// </summary>
    public enum OrderBy
    {
        /// <summary>
        /// 降順(Z-&gt;A)
        /// </summary>
        Descending,
        /// <summary>
        /// 昇順(A-&gt;Z)
        /// </summary>
        Ascending,
        /// <summary>
        /// 降順
        /// </summary>
        [Obsolete("use Descending")]
        Desc = Descending,
        /// <summary>
        /// 昇順
        /// </summary>
        [Obsolete("use Ascending")]
        Asc = Ascending,
    }
}
