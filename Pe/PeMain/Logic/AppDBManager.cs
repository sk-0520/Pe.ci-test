using System.Data.Common;
using ContentTypeTextNet.Pe.Library.Utility;
using ContentTypeTextNet.Pe.PeMain.Data.DB;

namespace ContentTypeTextNet.Pe.PeMain.Logic
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
				var count = query.GetResultSingle<CountDto>(global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.SQL_CheckTable);
				return count.Has;
			}
		}
		
		public SingleIdDto GetTableId(string tableName, string idColumnName)
		{
			using(var query = CreateQuery()) {
				query.SetExpression("table_name", tableName);
				query.SetExpression("id_column_name", idColumnName);

				return query.GetResultSingle<SingleIdDto>(global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.SQL_GetId);
			}
		}
		
	}
}
