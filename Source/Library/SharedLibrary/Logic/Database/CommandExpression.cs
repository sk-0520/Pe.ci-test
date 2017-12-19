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
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Database
{
    /// <summary>
    /// 条件式からコマンドの構築。
    /// <para><see cref="DatabaseManager"/>に渡すコマンドに対してさらに実装側で条件分岐を行う。</para>
    /// </summary>
    public class CommandExpression
    {
        /// <summary>
        /// 条件式のデフォルト値生成。
        /// </summary>
        public CommandExpression()
        {
            Condition = false;
            True = new QueryParts();
            False = new QueryParts();
        }

        /// <summary>
        /// 条件を真とし、真の文字列コマンドを設定する。
        /// </summary>
        /// <param name="trueCommand">コマンド。</param>
        public CommandExpression(string trueCommand)
            : this()
        {
            Condition = true;

            True.QueryPattern = QueryPattern.Command;
            True.Command = trueCommand;
        }

        /// <summary>
        /// 条件式を指定値で生成。
        /// <para>偽の場合は空文字列となる。</para>
        /// </summary>
        /// <param name="condition">条件。</param>
        /// <param name="trueCommand">真の場合のコマンド。</param>
        public CommandExpression(bool condition, string trueCommand)
            : this()
        {
            Condition = condition;

            True.QueryPattern = QueryPattern.Command;
            True.Command = trueCommand;

            False.QueryPattern = QueryPattern.Command;
            False.Command = string.Empty;
        }

        /// <summary>
        /// 条件式を指定値で生成。
        /// </summary>
        /// <param name="condition">条件。</param>
        /// <param name="trueCommand">真の場合のコマンド。</param>
        /// <param name="falseCommand">偽の場合のコマンド。</param>
        public CommandExpression(bool condition, string trueCommand, string falseCommand)
            : this()
        {
            Condition = condition;

            True.QueryPattern = QueryPattern.Command;
            True.Command = trueCommand;

            False.QueryPattern = QueryPattern.Command;
            False.Command = falseCommand;
        }

        /// <summary>
        /// 条件式を指定値で生成。
        /// </summary>
        /// <param name="condition">条件。</param>
        /// <param name="trueCommand">真の場合のコマンド。</param>
        /// <param name="falseCommandExpression">偽の場合の条件式。</param>
        public CommandExpression(bool condition, string trueCommand, CommandExpression falseCommandExpression)
            : this()
        {
            CheckUtility.EnforceNotNull(falseCommandExpression);

            Condition = condition;

            True.QueryPattern = QueryPattern.Command;
            True.Command = trueCommand;

            False.QueryPattern = QueryPattern.Expression;
            False.Expression = falseCommandExpression;
        }

        /// <summary>
        /// 条件式を指定値で生成。
        /// </summary>
        /// <param name="condition">条件。</param>
        /// <param name="trueCommandExpression">真の場合の条件式。</param>
        /// <param name="falseCommandExpression">偽の場合の条件式。</param>
        public CommandExpression(bool condition, CommandExpression trueCommandExpression, CommandExpression falseCommandExpression)
            : this()
        {
            CheckUtility.EnforceNotNull(trueCommandExpression);
            CheckUtility.EnforceNotNull(falseCommandExpression);

            Condition = condition;

            True.QueryPattern = QueryPattern.Expression;
            True.Expression = trueCommandExpression;

            False.QueryPattern = QueryPattern.Expression;
            False.Expression = falseCommandExpression;
        }

        #region property

        /// <summary>
        /// 条件。
        /// </summary>
        public bool Condition { get; private set; }

        /// <summary>
        /// 真の場合に使用するクエリ。
        /// </summary>
        public QueryParts True { get; private set; }
        /// <summary>
        /// 偽の場合に使用するクエリ。
        /// </summary>
        public QueryParts False { get; private set; }

        #endregion

        #region function

        /// <summary>
        /// 条件をコマンドに落とし込む。
        /// </summary>
        /// <returns></returns>
        public string ToCode()
        {
            if(Condition) {
                return True.ToCode();
            } else {
                return False.ToCode();
            }
        }

        #endregion
    }
}
