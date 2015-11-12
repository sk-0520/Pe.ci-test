namespace ContentTypeTextNet.Pe.Library.Utility.DB
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Data.Common;
	using System.Drawing;
	using ContentTypeTextNet.Pe.Library.Utility.DB;

	/// <summary>
	/// DBManager を SQLite で使えるようにする。
	/// </summary>
	public class SQLiteDBManager: DBManager
	{
		public SQLiteDBManager(DbConnection connection, bool isOpened): base(connection, isOpened)
		{ }
		
		public override object To(object value, Type toType)
		{
			var map = new Dictionary<Type, Func<object>>() {
				{ typeof(bool),     () => Convert.ToBoolean(value) },
				{ typeof(DateTime), () => Convert.ToDateTime(value) },
				{ typeof(int),      () => Convert.ToInt32(value) },
				{ typeof(Color),    () => Color.FromArgb(Convert.ToInt32(value)) },
				{ typeof(float),    () => Convert.ToSingle(value) },
			};
			
			if(map.ContainsKey(toType)) {
				return map[toType]();
			}
			
			return base.To(value, toType);
		}

		public override DbType DbTypeFromType(Type type)
		{
			var map = new Dictionary<Type, DbType>() {
				{ typeof(bool),     DbType.Int32 },
				{ typeof(DateTime), DbType.String },
				{ typeof(Color),    DbType.Int32 },
			};
			if(map.ContainsKey(type)) {
				return map[type];
			}
			
			return base.DbTypeFromType(type);
		}

		public override object DbValueFromValue(object value, Type type)
		{
			var map = new Dictionary<Type, Func<object>>() {
				{ typeof(bool),     () => Convert.ToInt32(value) },
				{ typeof(DateTime), () => ((DateTime)value).ToString("s") },
				{ typeof(Color),    () => ((Color)value).ToArgb() },
			};
			if(map.ContainsKey(type)) {
				return map[type]();
			}
			
			return base.DbValueFromValue(value, type);
		}

		/// <summary>
		/// テーブル一覧を取得する。
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> GetTables()
		{
			using(var query = CreateQuery()) {
				var reader = query.ExecuteReader(global::ContentTypeTextNet.Pe.Library.Utility.Properties.Resources.SQL_GetTables);
				while(reader.Read()) {
					yield return To<string>(reader["NAME"]);
				}
			}
		}
	}
}
