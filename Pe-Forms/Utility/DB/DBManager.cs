namespace ContentTypeTextNet.Pe.Library.Utility.DB
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Data.Common;
	using System.Diagnostics;
	using System.Linq;
	using System.Reflection;
	using System.Text.RegularExpressions;

	
	/// <summary>
	/// DB接続・操作の一元化
	/// 
	/// すんごいとろくない限り処理速度は考えない。
	/// </summary>
	public abstract class DBManager: IDisposable
	{
		/// <summary>
		/// 生成。
		/// 
		/// </summary>
		/// <param name="connection">コネクション</param>
		/// <param name="isOpened">コネクションは開いているか。閉じている場合は開く。</param>
		public DBManager(DbConnection connection, bool isOpened)
		{
			///Parameter = new Dictionary<string, object>();
			///Expression = new Dictionary<string, CommandExpression>();
			
			Connection = connection;
			
			
			if(!isOpened) {
				Connection.Open();
			}
		}

		/// <summary>
		/// DB接続。
		/// </summary>
		public DbConnection Connection { get; private set; }

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
		/// 
		/// ユーザーコードでは多分出番ない、はず。
		/// </summary>
		/// <returns></returns>
		public virtual DbQuery CreateQuery()
		{
			return new DbQuery(this);
		}
		
		/// <summary>
		/// 型変換。
		/// 
		/// キャストでなく実際の変換処理も担当する
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
		/// 
		/// 実際の型変換には object To(object, Type) を使用する。
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public T To<T>(object value)
		{
			return (T)(To(value, typeof(T)));
		}
		
		/// <summary>
		/// DBに合わせてデータ調整
		/// </summary>
		/// <param name="value"></param>
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

		#region IDisposable

		protected virtual void Dispose(bool disposing)
		{
			Connection.Dispose();
		}

		/// <summary>
		/// とじるん。
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}

		#endregion
	}
}
