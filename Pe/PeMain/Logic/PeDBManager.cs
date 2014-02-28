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
using System.Data.Common;
using PeUtility;

namespace PeMain.Logic
{
	/// <summary>
	/// DBManagerをSQLiteとPe用に特化。
	/// </summary>
	public class PeDBManager: DBManager
	{
		public PeDBManager(DbConnection connection, bool isOpened, bool sharedCommand): base(connection, isOpened, sharedCommand)
		{ }
		
		public bool ExistsTable(string tableName)
		{
			var param = new Dictionary<string, object>() {
				{"table", DataTables.masterTableVersion}
			};

			using(var reader = ExecuteReader(tableName, param)) {
				reader.Read();
				return Convert.ToInt32(reader["NUM"]) == 1;
			}
		}
		
	}
}
