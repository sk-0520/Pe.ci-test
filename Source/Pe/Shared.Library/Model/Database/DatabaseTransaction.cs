using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database
{
    public sealed class DatabaseTransaction<TDbConnection> : DisposerBase, IDatabaseCommander
        where TDbConnection: IDbConnection, new()
    {
        public DatabaseTransaction(DatabaseAccessorBase<TDbConnection> databaseAccessor)
        {
            DatabaseAccessor = databaseAccessor;
            Transaction = DatabaseAccessor.Connection.BeginTransaction();
        }

        public DatabaseTransaction(DatabaseAccessorBase<TDbConnection> databaseAccessor, IsolationLevel isolationLevel)
        {
            DatabaseAccessor = DatabaseAccessor;
            Transaction = DatabaseAccessor.Connection.BeginTransaction(isolationLevel);
        }

        #region property

        DatabaseAccessorBase<TDbConnection> DatabaseAccessor { get; set; }
        IDbTransaction Transaction { get; set; }

        #endregion

        #region function

        public void Commit()
        {
            Transaction.Commit();
        }

        public void Rollback()
        {
            Transaction.Rollback();
        }

        #endregion

        #region IDatabaseCommand

        public IEnumerable<T> Query<T>(string sql, object param = null, bool buffered = true)
        {
            var formattedSql = DatabaseAccessor.PreFormatSql(sql);
            return DatabaseAccessor.Query<T>(formattedSql, param, Transaction, buffered);
        }

        public IEnumerable<dynamic> Query(string sql, object param = null, bool buffered = true)
        {
            var formattedSql = DatabaseAccessor.PreFormatSql(sql);
            return DatabaseAccessor.Query(formattedSql, param, Transaction, buffered);
        }

        public int Execute(string sql, object param = null)
        {
            var formattedSql = DatabaseAccessor.PreFormatSql(sql);
            return DatabaseAccessor.Execute(formattedSql, param, Transaction);
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    Transaction.Dispose();
                    Transaction = null;
                    DatabaseAccessor = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
