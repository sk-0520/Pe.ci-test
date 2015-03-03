namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System.Data.Common;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.Library.Utility.DB;
	using ContentTypeTextNet.Pe.PeMain.Data.DB;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Kind;

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

		/// <summary>
		/// 不要データを削除。
		/// </summary>
		/// <param name="logger"></param>
		public void Delete(ILogger logger)
		{
		}

		/// <summary>
		/// アナライズだったりインデックスだったり。
		/// </summary>
		/// <param name="logger"></param>
		public void Analyze(ILogger logger)
		{
		}
	}
}
