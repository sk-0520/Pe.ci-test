using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Applications
{
    public sealed class ApplicationDatabaseBarrierTransaction : DisposerBase, IDatabaseTransaction
    {
        public ApplicationDatabaseBarrierTransaction(IDisposable locker, IDatabaseTransaction transaction)
        {
            Locker = locker;
            Transaction = transaction;
        }

        #region property

        IDisposable Locker { get; }
        IDatabaseTransaction Transaction { get; }

        IDbTransaction IDatabaseTransaction.Transaction => Transaction.Transaction;

        #endregion

        #region function
        #endregion

        #region IDatabaseTransaction

        public int Execute(string sql, object param = null)
        {
            return Transaction.Execute(sql, param);
        }

        public DataTable GetDataTable(string sql, object param = null)
        {
            return Transaction.GetDataTable(sql, param);
        }

        public IEnumerable<T> Query<T>(string sql, object param = null, bool buffered = true)
        {
            return Transaction.Query<T>(sql, param, buffered);
        }

        public IEnumerable<dynamic> Query(string sql, object param = null, bool buffered = true)
        {
            return Transaction.Query(sql, param, buffered);
        }

        public T QueryFirst<T>(string sql, object param = null)
        {
            return Transaction.QueryFirst<T>(sql, param);
        }

        public T QueryFirstOrDefault<T>(string sql, object param = null)
        {
            return Transaction.QueryFirstOrDefault<T>(sql, param);
        }

        public T QuerySingle<T>(string sql, object param = null)
        {
            return Transaction.QuerySingle<T>(sql, param);
        }

        public void Commit()
        {
            Transaction.Commit();
        }

        public void Rollback()
        {
            Transaction.Rollback();
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    Transaction.Dispose();
                    Locker.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    public interface IApplicationDatabaseBarrier
    {
        #region property

        IDatabaseAccessor Accessor { get; }
        ReaderWriterLocker Locker { get; }

        #endregion

        #region function

        ApplicationDatabaseBarrierTransaction WaitWrite();
        ApplicationDatabaseBarrierTransaction WaitRead();

        #endregion
    }

    public interface IMainDatabaseBarrier : IApplicationDatabaseBarrier
    { }
    public interface IFileDatabaseBarrier : IApplicationDatabaseBarrier
    { }
    public interface ITemporaryDatabaseBarrier : IApplicationDatabaseBarrier
    { }

    public sealed class ApplicationDatabaseBarrier : IMainDatabaseBarrier, IFileDatabaseBarrier, ITemporaryDatabaseBarrier
    {
        public ApplicationDatabaseBarrier(IDatabaseAccessor accessor, ReaderWriterLocker locker)
        {
            Accessor = accessor;
            Locker = locker;
        }

        #region IDatabaseBarrier

        public IDatabaseAccessor Accessor { get; }
        public ReaderWriterLocker Locker { get; }

        public ApplicationDatabaseBarrierTransaction WaitWrite()
        {
            var locker = Locker.WaitWriteByDefaultTimeout();
            var commander = Accessor.BeginTransaction();
            var result = new ApplicationDatabaseBarrierTransaction(locker, commander);
            return result;
        }

        public ApplicationDatabaseBarrierTransaction WaitRead()
        {
            var locker = Locker.WaitWriteByDefaultTimeout();
            var commander = Accessor.BeginTransaction();
            var result = new ApplicationDatabaseBarrierTransaction(locker, commander);
            return result;
        }

        #endregion
    }
}
