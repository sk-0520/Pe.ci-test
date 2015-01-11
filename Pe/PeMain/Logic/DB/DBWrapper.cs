using System.Data.Common;

namespace ContentTypeTextNet.Pe.PeMain.Logic.DB
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
