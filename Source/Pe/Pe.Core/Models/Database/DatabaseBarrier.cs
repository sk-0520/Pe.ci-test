using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models.Database
{
    sealed class DatabaseBarrierTransaction : DisposerBase, IDatabaseTransaction
    {
        public DatabaseBarrierTransaction(IDisposable locker, IDatabaseTransaction transaction, IDatabaseImplementation implementation)
        {
            Locker = locker;
            Transaction = transaction;
            Implementation = implementation;
        }

        #region property

        IDisposable Locker { get; }
        IDatabaseTransaction Transaction { get; }

        IDbTransaction IDatabaseTransaction.Transaction => Transaction.Transaction;

        public IDatabaseImplementation Implementation { get; }

        #endregion

        #region function
        #endregion

        #region IDatabaseTransaction

        public int Execute(string statement, object? param = null)
        {
            return Transaction.Execute(statement, param);
        }

        public DataTable GetDataTable(string statement, object? param = null)
        {
            return Transaction.GetDataTable(statement, param);
        }

        public IEnumerable<T> Query<T>(string statement, object? param = null, bool buffered = true)
        {
            return Transaction.Query<T>(statement, param, buffered);
        }

        public IEnumerable<dynamic> Query(string statement, object? param = null, bool buffered = true)
        {
            return Transaction.Query(statement, param, buffered);
        }

        public T QueryFirst<T>(string statement, object? param = null)
        {
            return Transaction.QueryFirst<T>(statement, param);
        }

        public T QueryFirstOrDefault<T>(string statement, object? param = null)
        {
            return Transaction.QueryFirstOrDefault<T>(statement, param);
        }

        public T QuerySingle<T>(string statement, object? param = null)
        {
            return Transaction.QuerySingle<T>(statement, param);
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

    public interface IDatabaseBarrier
    {
        #region property

        IDatabaseAccessor Accessor { get; }
        ReaderWriterLocker Locker { get; }

        #endregion

        #region function

        IDatabaseTransaction WaitWrite();
        IDatabaseTransaction WaitRead();

        #endregion
    }

    public class DatabaseBarrier : IDatabaseBarrier
    {
        public DatabaseBarrier(IDatabaseAccessor accessor, ReaderWriterLocker locker)
        {
            Accessor = accessor;
            Locker = locker;
        }

        #region IDatabaseBarrier

        public IDatabaseAccessor Accessor { get; }
        public ReaderWriterLocker Locker { get; }

        public virtual IDatabaseTransaction WaitWrite()
        {
            var locker = Locker.WaitWriteByDefaultTimeout();
            var commander = Accessor.BeginTransaction();
            var result = new DatabaseBarrierTransaction(locker, commander, Accessor.DatabaseFactory.CreateImplementation());
            return result;
        }

        public virtual IDatabaseTransaction WaitRead()
        {
            var locker = Locker.WaitWriteByDefaultTimeout();
            var commander = Accessor.BeginTransaction();
            var result = new DatabaseBarrierTransaction(locker, commander, Accessor.DatabaseFactory.CreateImplementation());
            return result;
        }

        #endregion
    }

}
