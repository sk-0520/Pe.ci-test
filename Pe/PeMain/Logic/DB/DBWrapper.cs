/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/08/08
 * 時刻: 21:12
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Data.Common;
using PeUtility;

namespace PeMain.Logic.DB
{
	/// <summary>
	/// Description of DBWrapper.
	/// </summary>
	abstract public class DBWrapper
	{
		protected AppDBManager db;
		
		public DBWrapper(AppDBManager db)
		{
			this.db = db;
		}
		
		public DbTransaction BeginTransaction()
		{
			return this.db.BeginTransaction();
		}

	}
}
