using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database
{
    public interface IDatabaseTransaction: IDatabaseCommander, IDisposable
    {
        #region property

        IDbTransaction Transaction { get; }

        #endregion

        #region function

        void Commit();

        void Rollback();

        #endregion
    }

    public class DatabaseTransaction : DisposerBase, IDatabaseTransaction
    {
        public DatabaseTransaction(IDatabaseAccessor databaseAccessor)
        {
            DatabaseAccessor = databaseAccessor;
            Transaction = DatabaseAccessor.BaseConnection.BeginTransaction();
        }

        public DatabaseTransaction(IDatabaseAccessor databaseAccessor, IsolationLevel isolationLevel)
        {
            DatabaseAccessor = DatabaseAccessor;
            Transaction = DatabaseAccessor.BaseConnection.BeginTransaction(isolationLevel);
        }

        #region property

        IDatabaseAccessor DatabaseAccessor { get; set; }
        public bool Committed { get; private set; }

        #endregion

        #region IDatabaseTransaction

        public IDbTransaction Transaction { get; private set; }

        public void Commit()
        {
            Committed = true;
            Transaction.Commit();
        }

        public void Rollback()
        {
            Transaction.Rollback();
        }

        public IEnumerable<T> Query<T>(string sql, object param = null, bool buffered = true)
        {
            return DatabaseAccessor.Query<T>(sql, param, this, buffered);
        }

        public IEnumerable<dynamic> Query(string sql, object param = null, bool buffered = true)
        {
            return DatabaseAccessor.Query(sql, param, this, buffered);
        }

        public int Execute(string sql, object param = null)
        {
            return DatabaseAccessor.Execute(sql, param, this);
        }

        public DataTable GetDataTable(string sql, object param = null)
        {
            return DatabaseAccessor.GetDataTable(sql, param, this);
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    if(!Committed) {
                        Rollback();
                    }
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
