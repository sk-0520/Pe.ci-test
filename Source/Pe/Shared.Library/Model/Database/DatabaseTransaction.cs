using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database
{
    public interface IDatabaseTransaction: IDatabaseCommander
    {
        #region function

        void Commit();

        void Rollback();

        #endregion
    }

    public sealed class DatabaseTransaction : DisposerBase, IDatabaseTransaction
    {
        public DatabaseTransaction(DatabaseAccessorBase databaseAccessor)
        {
            DatabaseAccessor = databaseAccessor;
            Transaction = DatabaseAccessor.BaseConnection.BeginTransaction();
        }

        public DatabaseTransaction(DatabaseAccessorBase databaseAccessor, IsolationLevel isolationLevel)
        {
            DatabaseAccessor = DatabaseAccessor;
            Transaction = DatabaseAccessor.BaseConnection.BeginTransaction(isolationLevel);
        }

        #region property

        DatabaseAccessorBase DatabaseAccessor { get; set; }
        IDbTransaction Transaction { get; set; }

        #endregion

        #region IDatabaseTransaction

        public void Commit()
        {
            Transaction.Commit();
        }

        public void Rollback()
        {
            Transaction.Rollback();
        }

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
