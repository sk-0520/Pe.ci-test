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
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Define;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Database
{
    public class QueryParts
    {
        public QueryParts()
        {
            Command = string.Empty;
        }

        #region property

        /// <summary>
        /// 条件が真の場合にコマンドと式のどちらを使用するか
        /// </summary>
        public QueryPattern QueryPattern { get; set; }
        /// <summary>
        /// 条件が真の場合のコマンド
        /// </summary>
        public string Command { get; set; }
        /// <summary>
        /// 条件が真の場合の式
        /// </summary>
        public CommandExpression Expression { get; set; }

        #endregion

        #region function

        public virtual string ToCode()
        {
            if(QueryPattern == QueryPattern.Command) {
                return Command;
            } else {
                Debug.Assert(QueryPattern == QueryPattern.Expression);
                return Expression.ToCode();
            }
        }

        #endregion
    }
}
