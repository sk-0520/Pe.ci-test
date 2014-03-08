/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/02/28
 * 時刻: 22:03
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using PeUtility;

namespace PeMain.Logic
{
	public abstract class SQLiteDBManager: DBManager
	{
		public SQLiteDBManager(DbConnection connection, bool isOpened, bool sharedCommand): base(connection, isOpened, sharedCommand)
		{ }
		
		public override T To<T>(object value)
		{
			var map = new Dictionary<Type, Func<object>>() {
				{ typeof(bool),     () => Convert.ToBoolean(value) },
				{ typeof(DateTime), () => Convert.ToDateTime(value) },
			};
			if(map.ContainsKey(typeof(T))) {
				return (T)map[typeof(T)]();
			}
			
			return base.To<T>(value);
		}
		
		protected override DbType DbTypeFromType(Type type)
		{
			var map = new Dictionary<Type, DbType>() {
				{ typeof(bool),     DbType.Int32 },
				{ typeof(DateTime), DbType.String },
			};
			if(map.ContainsKey(type)) {
				return map[type];
			}
			
			return base.DbTypeFromType(type);
		}
		
		protected override object DbValueFromValue(object value, Type type)
		{
			var map = new Dictionary<Type, Func<object>>() {
				{ typeof(bool),     () => Convert.ToInt32(value) },
				{ typeof(DateTime), () => ((DateTime)value).ToString("s") },
			};
			if(map.ContainsKey(type)) {
				return map[type]();
			}
			
			return base.DbValueFromValue(value, type);
		}
		
	}
	/// <summary>
	/// DBManagerをSQLiteとPe用に特化。
	/// </summary>
	public class PeDBManager: SQLiteDBManager
	{
		public PeDBManager(DbConnection connection, bool isOpened, bool sharedCommand): base(connection, isOpened, sharedCommand)
		{ }
		
		public bool ExistsTable(string tableName)
		{
			Clear();
			
			Parameter["table_name"] = tableName;

			using(var reader = ExecuteReader(global::PeMain.Properties.SQL.CheckTable)) {
				reader.Read();
				return To<bool>(reader["NUM"]);
			}
		}
		
	}
}
