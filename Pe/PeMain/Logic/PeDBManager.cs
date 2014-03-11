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

using PeMain.Data.DB;
using PeUtility;

namespace PeMain.Logic
{
	/// <summary>
	/// DBManagerをSQLiteとPe用に特化。
	/// </summary>
	public class PeDBManager: SQLiteDBManager
	{
		public PeDBManager(DbConnection connection, bool isOpened): base(connection, isOpened)
		{ }
		
		public bool ExistsTable(string tableName)
		{
			Clear();
			
			Parameter["table_name"] = tableName;

			/*
			using(var reader = ExecuteReader(global::PeMain.Properties.SQL.CheckTable)) {
				reader.Read();
				return To<long>(reader["NUM"]) == 1;
			}
			*/
			var count = GetDtoSingle<CountDto>(global::PeMain.Properties.SQL.CheckTable);
			return count.Has;
		}
		
		public SingleIdDto GetTableId(string tableName, string idColumnName)
		{
			Clear();
			
			Parameter["table_name"] = tableName;
			Parameter["id_column_name"] = idColumnName;
			
			return GetDtoSingle<SingleIdDto>(global::PeMain.Properties.SQL.GetId);
		}
		
	}
}
