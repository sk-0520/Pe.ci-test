﻿namespace ContentTypeTextNet.Pe.Library.Utility.DB
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
	/// DTO, Entity で使用するカラム名。
	/// 
	/// Entity の場合はテーブル名まで指定する。
	/// </summary>
	[AttributeUsage(
		AttributeTargets.Class  | AttributeTargets.Property,
		AllowMultiple = true,
		Inherited = true
	)]
	public sealed class TargetNameAttribute: Attribute
	{
		public TargetNameAttribute(string name, bool primary)
		{
			TargetName = name;
			PrimaryKey = primary;
		}
		
		public TargetNameAttribute(string name): this(name, false)
		{ }
		/// <summary>
		/// 物理名
		/// </summary>
		public string TargetName { get; private set; }
		/// <summary>
		/// 主キー
		/// </summary>
		public bool PrimaryKey { get; private set; }
	}
	
	/// <summary>
	/// 物理名・プロパティ紐付。
	/// 
	/// TargetNameAttributeに紐付く物理名とプロパティ情報。
	/// </summary>
	public sealed class TargetInfo
	{
		public TargetInfo(TargetNameAttribute attribute, PropertyInfo propertyInfo)
		{
			TargetNameAttribute = attribute;
			PropertyInfo = propertyInfo;
		}
		public TargetNameAttribute TargetNameAttribute { get; private set; }
		/// <summary>
		/// TargetNameAttributeで紐付くプロパティ。
		/// </summary>
		public PropertyInfo PropertyInfo { get; private set; }
	}
	
	/// <summary>
	/// エンティティ一覧情報
	/// 
	/// エンティティとして必要な物理名とエンティティオブジェクトのプロパティ一覧。
	/// </summary>
	public sealed class EntitySet
	{
		public EntitySet(string tableName, IList<TargetInfo> targetInfos)
		{
			TableName = tableName;
			TargetInfos = targetInfos;
		}
		/// <summary>
		/// テーブル名。
		/// </summary>
		public string TableName { get ; private set; }
		/// <summary>
		/// 対象TargetInfoの集合
		/// </summary>
		public IList<TargetInfo> TargetInfos { get; private set; }
	}
	

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
