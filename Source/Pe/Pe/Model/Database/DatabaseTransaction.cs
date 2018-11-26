using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Database
{
    public class DatabaseTransaction : DisposerBase, IDatabaseCommander
    {
        public DatabaseTransaction(DatabaseAccessorBase databaseAccessor)
        {
            DatabaseAccessor = databaseAccessor;
            Transaction = DatabaseAccessor.ConnectionBase.BeginTransaction();
        }

        public DatabaseTransaction(DatabaseAccessorBase databaseAccessor, IsolationLevel isolationLevel)
        {
            DatabaseAccessor = DatabaseAccessor;
            Transaction = DatabaseAccessor.ConnectionBase.BeginTransaction(isolationLevel);
        }

        #region property

        protected DatabaseAccessorBase DatabaseAccessor { get; private set; }
        protected IDbTransaction Transaction { get; private set; }

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
