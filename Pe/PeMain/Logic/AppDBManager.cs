namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System.Linq;
	using System.Data.Common;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.Library.Utility.DB;
	using ContentTypeTextNet.Pe.PeMain.Data.DB;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Kind;
	using ContentTypeTextNet.Pe.PeMain.Logic.DB;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using System.Collections.Generic;
	using System;

	/// <summary>
	/// DBManagerをSQLiteとPe用に特化。
	/// </summary>
	public class AppDBManager: SQLiteDBManager
	{
		public AppDBManager(DbConnection connection, bool isOpened): base(connection, isOpened)
		{ }
		
		/// <summary>
		/// 指定テーブルが存在するか。
		/// </summary>
		/// <param name="tableName">テーブル名。</param>
		/// <returns>存在していれば真。</returns>
		public bool ExistsTable(string tableName)
		{
			using(var query = CreateQuery()) {
				query.Parameter["table_name"] = tableName;
				var count = query.GetResultSingle<CountDto>(global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.SQL_CheckTable);
				return count.Has;
			}
		}
		
		/// <summary>
		/// 指定テーブルのIDを取得する。
		/// </summary>
		/// <param name="tableName">テーブル名。</param>
		/// <param name="idColumnName">数値型のカラム名。</param>
		/// <returns></returns>
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
		public bool DeleteDisableItem(ILogger logger)
		{
			var tran = BeginTransaction();
			try {
				var noteDB = new NoteDB(this);
				noteDB.DeleteDisableItem();
				tran.Commit();

				return true;
			} catch(Exception ex) {
				logger.Puts(LogType.Error, ex.Message, ex);
				tran.Rollback();

				return false;
			}
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
