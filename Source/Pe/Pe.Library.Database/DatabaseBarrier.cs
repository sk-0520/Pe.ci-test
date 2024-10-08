using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Base;

namespace ContentTypeTextNet.Pe.Library.Database
{
    file sealed class DatabaseBarrierTransaction: DisposerBase, IDatabaseTransaction
    {
        public DatabaseBarrierTransaction(IDisposable locker, IDatabaseTransaction transaction, IDatabaseImplementation implementation)
        {
            Locker = locker;
            Transaction = transaction;
            Implementation = implementation;
        }

        #region property

        private IDisposable Locker { get; [Unused(UnusedKinds.Dispose)] set; }
        public IDatabaseTransaction Transaction { get; [Unused(UnusedKinds.Dispose)] set; }

        IDbTransaction? IDatabaseTransaction.Transaction => Transaction.Transaction;

        public IDatabaseImplementation Implementation { get; }

        #endregion

        #region function
        #endregion

        #region IDatabaseTransaction

        public IDatabaseContext Context => this;

        public int Execute(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return Transaction.Execute(statement, parameter);
        }

        public Task<int> ExecuteAsync(string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            return Transaction.ExecuteAsync(statement, parameter, cancellationToken);
        }

        public IDataReader GetDataReader(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return Transaction.GetDataReader(statement, parameter);
        }

        public Task<IDataReader> GetDataReaderAsync(string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            return Transaction.GetDataReaderAsync(statement, parameter, cancellationToken);
        }

        public DataTable GetDataTable(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return Transaction.GetDataTable(statement, parameter);
        }

        public Task<DataTable> GetDataTableAsync(string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            return Transaction.GetDataTableAsync(statement, parameter, cancellationToken);
        }

        public TResult? GetScalar<TResult>(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return Transaction.GetScalar<TResult?>(statement, parameter);
        }

        public Task<TResult?> GetScalarAsync<TResult>(string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            return Transaction.GetScalarAsync<TResult?>(statement, parameter, cancellationToken);
        }

        public IEnumerable<T> Query<T>(string statement, object? parameter = null, bool buffered = true)
        {
            ThrowIfDisposed();

            return Transaction.Query<T>(statement, parameter, buffered);
        }

        public Task<IEnumerable<T>> QueryAsync<T>(string statement, object? parameter = null, bool buffered = true, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            return Transaction.QueryAsync<T>(statement, parameter, buffered, cancellationToken);
        }

        public IEnumerable<dynamic> Query(string statement, object? parameter = null, bool buffered = true)
        {
            ThrowIfDisposed();

            return Transaction.Query(statement, parameter, buffered);
        }

        public Task<IEnumerable<dynamic>> QueryAsync(string statement, object? parameter = null, bool buffered = true, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            return Transaction.QueryAsync(statement, parameter, buffered, cancellationToken);
        }

        public T QueryFirst<T>(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return Transaction.QueryFirst<T>(statement, parameter);
        }

        public Task<T> QueryFirstAsync<T>(string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            return Transaction.QueryFirstAsync<T>(statement, parameter, cancellationToken);
        }

        [return: MaybeNull]
        public T QueryFirstOrDefault<T>(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return Transaction.QueryFirstOrDefault<T>(statement, parameter);
        }

        public Task<T?> QueryFirstOrDefaultAsync<T>(string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            return Transaction.QueryFirstOrDefaultAsync<T?>(statement, parameter, cancellationToken);
        }

        public Task<T> QuerySingleAsync<T>(string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            return Transaction.QuerySingleAsync<T>(statement, parameter, cancellationToken);
        }


        public T QuerySingle<T>(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return Transaction.QuerySingle<T>(statement, parameter);
        }

        [return: MaybeNull]
        public T QuerySingleOrDefault<T>(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return Transaction.QuerySingleOrDefault<T>(statement, parameter);
        }

        public Task<T?> QuerySingleOrDefaultAsync<T>(string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            return Transaction.QuerySingleOrDefaultAsync<T>(statement, parameter, cancellationToken);
        }

        public void Commit()
        {
            ThrowIfDisposed();

            Transaction.Commit();
        }

        public void Rollback()
        {
            Transaction.Rollback();

            ThrowIfDisposed();
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
                Transaction = null!;
                Locker = null!;
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    /// <inheritdoc cref="IDatabaseBarrier" />
    public class DatabaseBarrier: IDatabaseBarrier
    {
        public DatabaseBarrier(IDatabaseAccessor accessor, ReadWriteLockHelper locker)
        {
            Accessor = accessor;
            Locker = locker;
        }

        #region property

        protected IDatabaseAccessor Accessor { get; }
        protected ReadWriteLockHelper Locker { get; }

        #endregion

        #region IDatabaseBarrier

        /// <summary>
        /// <inheritdoc cref="IDatabaseBarrier.WaitWrite" />
        /// </summary>
        /// <remarks>
        /// <para><see cref="ReadWriteLockHelper.WaitWriteByDefaultTimeout()"/>が規定時間。</para>
        /// </remarks>
        public virtual IDatabaseTransaction WaitWrite()
        {
            var locker = Locker.WaitWriteByDefaultTimeout();
            var transaction = Accessor.BeginTransaction();
            var result = new DatabaseBarrierTransaction(locker, transaction, Accessor.DatabaseFactory.CreateImplementation());
            return result;
        }

        /// <inheritdoc cref="IDatabaseBarrier.WaitWrite(TimeSpan)" />
        public virtual IDatabaseTransaction WaitWrite(TimeSpan timeout)
        {
            var locker = Locker.WaitWrite(timeout);
            var transaction = Accessor.BeginTransaction();
            var result = new DatabaseBarrierTransaction(locker, transaction, Accessor.DatabaseFactory.CreateImplementation());
            return result;
        }

        /// <summary>
        /// <inheritdoc cref="IDatabaseBarrier.WaitRead" />
        /// </summary>
        /// <remarks>
        /// <para><see cref="ReadWriteLockHelper.WaitReadByDefaultTimeout()"/>が規定時間。</para>
        /// </remarks>
        /// <returns></returns>
        public virtual IDatabaseTransaction WaitRead()
        {
            var locker = Locker.WaitReadByDefaultTimeout();
            var transaction = Accessor.BeginReadOnlyTransaction();
            var result = new DatabaseBarrierTransaction(locker, transaction, Accessor.DatabaseFactory.CreateImplementation());
            return result;
        }

        /// <inheritdoc cref="IDatabaseBarrier.WaitRead(TimeSpan)" />
        public virtual IDatabaseTransaction WaitRead(TimeSpan timeout)
        {
            var locker = Locker.WaitRead(timeout);
            var transaction = Accessor.BeginReadOnlyTransaction();
            var result = new DatabaseBarrierTransaction(locker, transaction, Accessor.DatabaseFactory.CreateImplementation());
            return result;
        }

        #endregion
    }
}
