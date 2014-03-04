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
				return Convert.ToInt32(reader["NUM"]) == 1;
			}
		}
		
	}
}
