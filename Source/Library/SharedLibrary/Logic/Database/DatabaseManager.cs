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
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Data.Database;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Database
{
    /// <summary>
    /// DB接続・操作の一元化
    /// <para>すんごいとろくない限り処理速度は考えない。</para>
    /// </summary>
    public abstract class DatabaseManager: DisposeFinalizeBase
    {
        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="connection">コネクション</param>
        /// <param name="isOpened">コネクションは開いているか。閉じている場合は開く。</param>
        public DatabaseManager(DbConnection connection, bool isOpened)
        {
            Connection = connection;

            if(!isOpened) {
                Connection.Open();
            }
        }

        #region property

        /// <summary>
        /// DB接続。
        /// </summary>
        public DbConnection Connection { get; }

        #endregion

        #region function

        /// <summary>
        /// トランザクションの開始
        /// </summary>
        /// <returns></returns>
        public DbTransaction BeginTransaction()
        {
            var tran = Connection.BeginTransaction();

            return tran;
        }

        /// <summary>
        /// コマンド生成。
        /// <para>ユーザーコードでは多分出番ない、はず。</para>
        /// </summary>
        /// <returns></returns>
        public virtual DatabaseQuery CreateQuery()
        {
            return new DatabaseQuery(this);
        }

        /// <summary>
        /// 型変換。
        /// <para>キャストでなく実際の変換処理も担当する</para>
        /// <para>いろいろあってpublicだけどサブクラスみたいな拡張時に使用して、外部からは原則的に<see cref="To&lt;T&gt;(object)"/>を使用する。</para>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="toType"></param>
        /// <returns></returns>
        public virtual object To(object value, Type toType)
        {
            return value;
        }
        /// <summary>
        /// 型変換。
        /// <para>実際の型変換には <see cref="To(object, Type)"/> を使用する。</para>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public TResult To<TResult>(object value)
        {
            return (TResult)(To(value, typeof(TResult)));
        }

        /// <summary>
        /// DBに合わせてデータ調整
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual object DbValueFromValue(object value, Type type)
        {
            return value;
        }

        /// <summary>
        /// DBに合わせて型調整
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual DbType DbTypeFromType(Type type)
        {
            var map = new Dictionary<Type, DbType>() {
                { typeof(byte), DbType.Byte },
                { typeof(sbyte), DbType.SByte },
                { typeof(short), DbType.Int16 },
                { typeof(ushort), DbType.UInt16 },
                { typeof(int), DbType.Int32 },
                { typeof(uint), DbType.UInt32 },
                { typeof(long), DbType.Int64 },
                { typeof(ulong), DbType.UInt64 },
                { typeof(float), DbType.Single },
                { typeof(double), DbType.Double },
                { typeof(decimal), DbType.Decimal },
                { typeof(bool), DbType.Boolean },
                { typeof(string), DbType.String },
                { typeof(char), DbType.StringFixedLength },
                { typeof(Guid), DbType.Guid },
                { typeof(DateTime), DbType.DateTime },
                { typeof(DateTimeOffset), DbType.DateTimeOffset },
                { typeof(byte[]), DbType.Binary },
                { typeof(byte?), DbType.Byte },
                { typeof(sbyte?), DbType.SByte },
                { typeof(short?), DbType.Int16 },
                { typeof(ushort?), DbType.UInt16 },
                { typeof(int?), DbType.Int32 },
                { typeof(uint?), DbType.UInt32 },
                { typeof(long?), DbType.Int64 },
                { typeof(ulong?), DbType.UInt64 },
                { typeof(float?), DbType.Single },
                { typeof(double?), DbType.Double },
                { typeof(decimal?), DbType.Decimal },
                { typeof(bool?), DbType.Boolean },
                { typeof(char?), DbType.StringFixedLength },
                { typeof(Guid?), DbType.Guid },
                { typeof(DateTime?), DbType.DateTime },
            };

#if DEBUG
            if(map.ContainsKey(type)) {
#endif
                return map[type];
#if DEBUG
            }
            throw new ArgumentException(type.ToString());
#endif
        }

        /// <summary>
        /// 行取得用コードの生成。
        /// </summary>
        /// <param name="entitySet"></param>
        /// <returns></returns>
        public virtual string CreateSelectCommandCode(EntityMappingSet entitySet)
        {
            // 主キー
            var primary = entitySet.TargetInfos.Where(t => t.EntityMappingAttribute.PrimaryKey);

            var code = string.Format(
                "select * from {0} where {1}",
                entitySet.TableName,
                string.Join("and ", primary.Select(t => string.Format("{0} = :{1}", t.EntityMappingAttribute.PhysicalName, t.PropertyInfo.Name)))
            );

            return code;
        }

        /// <summary>
        /// 行挿入用コードの生成。
        /// </summary>
        /// <param name="entitySet"></param>
        /// <returns></returns>
        public virtual string CreateInsertCommandCode(EntityMappingSet entitySet)
        {
            var code = string.Format(
                "insert into {0} ({1}) values({2})",
                entitySet.TableName,
                string.Join(", ", entitySet.TargetInfos.Select(t => t.EntityMappingAttribute.PhysicalName)),
                string.Join(", ", entitySet.TargetInfos.Select(t => ":" + t.PropertyInfo.Name))
            );

            return code;
        }

        /// <summary>
        /// エンティティ更新用コードの生成。
        /// </summary>
        /// <param name="entitySet"></param>
        /// <returns></returns>
        public virtual string CreateUpdateCommandCode(EntityMappingSet entitySet)
        {
            // 主キー
            var primary = entitySet.TargetInfos.Where(t => t.EntityMappingAttribute.PrimaryKey);
            // 変更データ
            var data = entitySet.TargetInfos.Where(t => !t.EntityMappingAttribute.PrimaryKey);

            var code = string.Format(
                "update {0} set {1} where {2}",
                entitySet.TableName,
                string.Join(", ", data.Select(t => string.Format("{0} = :{1}", t.EntityMappingAttribute.PhysicalName, t.PropertyInfo.Name))),
                string.Join(" and ", primary.Select(t => string.Format("{0} = :{1}", t.EntityMappingAttribute.PhysicalName, t.PropertyInfo.Name)))
            );

            return code;
        }

        /// <summary>
        /// エンティティ削除用コードの生成。
        /// </summary>
        /// <param name="entitySet"></param>
        /// <returns></returns>
        public virtual string CreateDeleteCommandCode(EntityMappingSet entitySet)
        {
            // 主キー
            var primary = entitySet.TargetInfos.Where(t => t.EntityMappingAttribute.PrimaryKey);

            var code = string.Format(
                "delete from {0} where {1}",
                entitySet.TableName,
                string.Join("and ", primary.Select(t => string.Format("{0} = :{1}", t.EntityMappingAttribute.PhysicalName, t.PropertyInfo.Name)))
            );

            return code;
        }

        #endregion

        #region DisposeFinalizeBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Connection.Dispose();
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
