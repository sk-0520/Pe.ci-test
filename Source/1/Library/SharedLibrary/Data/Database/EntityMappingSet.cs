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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.Data.Database;

namespace ContentTypeTextNet.Library.SharedLibrary.Data.Database
{
    /// <summary>
    /// エンティティ一覧情報
    /// <para>エンティティとして必要な物理名とエンティティオブジェクトのプロパティ一覧。</para>
    /// </summary>
    public sealed class EntityMappingSet
    {
        public EntityMappingSet(string tableName, IList<EntityMappingInformation> targetInfos)
        {
            TableName = tableName;
            TargetInfos = targetInfos;
        }

        #region property

        /// <summary>
        /// テーブル名。
        /// </summary>
        public string TableName { get; private set; }
        /// <summary>
        /// 対象<see cref="EntityMappingInformation"/>の集合
        /// </summary>
        public IList<EntityMappingInformation> TargetInfos { get; private set; }

        #endregion
    }
}
