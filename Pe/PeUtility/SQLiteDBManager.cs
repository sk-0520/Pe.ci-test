/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/03/08
 * 時刻: 15:54
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace PeUtility
{
	public abstract class SQLiteDBManager: DBManager
	{
		public SQLiteDBManager(DbConnection connection, bool isOpened, bool sharedCommand): base(connection, isOpened, sharedCommand)
		{ }
		
		protected override object To(object value, Type toType)
		{
			var map = new Dictionary<Type, Func<object>>() {
				{ typeof(bool),     () => Convert.ToBoolean(value) },
				{ typeof(DateTime), () => Convert.ToDateTime(value) },
				{ typeof(int),      () => Convert.ToInt32(value) },
			};
			if(map.ContainsKey(toType)) {
				return map[toType]();
			}
			
			return base.To(value, toType);
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
}
