/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/02/28
 * 時刻: 22:03
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System.Data.Common;
using PeMain.Data.DB;
using ContentTypeTextNet.Pe.Library.Utility;

namespace PeMain.Logic
{
	/// <summary>
	/// DBManagerをSQLiteとPe用に特化。
	/// </summary>
	public class AppDBManager: SQLiteDBManager
	{
		public AppDBManager(DbConnection connection, bool isOpened): base(connection, isOpened)
		{ }
		
		public bool ExistsTable(string tableName)
		{
			using(var query = CreateQuery()) {
				query.Parameter["table_name"] = tableName;
				var count = query.GetResultSingle<CountDto>(global::PeMain.Properties.SQL.CheckTable);
				return count.Has;
			}
		}
		
		public SingleIdDto GetTableId(string tableName, string idColumnName)
		{
			using(var query = CreateQuery()) {
				query.SetExpression("table_name", tableName);
				query.SetExpression("id_column_name", idColumnName);

				return query.GetResultSingle<SingleIdDto>(global::PeMain.Properties.SQL.GetId);
			}
		}
		
	}
}
