using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Threading;
using ContentTypeTextNet.Pe.Library.Base;
using System.Diagnostics;

namespace ContentTypeTextNet.Pe.Library.Database
{
    /// <summary>
    /// トランザクション中の処理をサポート。
    /// </summary>
    /// <remarks>
    /// <para>基本的にはユーザーコードで登場せず <see cref="IDatabaseContext"/>がすべて上位から良しなに対応する。</para>
    /// </remarks>
    public class DatabaseTransaction: DisposerBase, IDatabaseTransaction
    {
        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="beginTransaction">トランザクションを開始するか。</param>
        /// <param name="databaseAccessor">アクセサ。</param>
        public DatabaseTransaction(bool beginTransaction, IDatabaseAccessor databaseAccessor)
        {
            DatabaseAccessor = databaseAccessor;
            Implementation = DatabaseAccessor.DatabaseFactory.CreateImplementation();

            if(beginTransaction) {
                Transaction = DatabaseAccessor.BaseConnection.BeginTransaction();
            } else {
                Transaction = null;
            }
        }

        /// <summary>
        /// 生成。
        /// </summary>
        /// <param name="beginTransaction">トランザクションを開始するか。</param>
        /// <param name="databaseAccessor">アクセサ。</param>
        /// <param name="isolationLevel"><see cref="IsolationLevel"/></param>
        public DatabaseTransaction(bool beginTransaction, IDatabaseAccessor databaseAccessor, IsolationLevel isolationLevel)
        {
            DatabaseAccessor = databaseAccessor;
            Implementation = DatabaseAccessor.DatabaseFactory.CreateImplementation();

            if(beginTransaction) {
                Transaction = DatabaseAccessor.BaseConnection.BeginTransaction(isolationLevel);
            } else {
                Transaction = null;
            }
        }

        #region property

        private IDatabaseAccessor DatabaseAccessor { get; [Unused(UnusedKinds.Dispose)] set; }
        public bool Committed { get; private set; }

        #endregion

        #region IDatabaseTransaction

        /// <summary>
        /// <see cref="IDatabaseContext"/>としての自身を返す。
        /// </summary>
        public IDatabaseContext Context => this;
        public IDbTransaction? Transaction { get; [Unused(UnusedKinds.Dispose)] private set; }
        public IDatabaseImplementation Implementation { get; }

        public virtual void Commit()
        {
            Debug.Assert(Transaction is not null);

            ThrowIfDisposed();

            Transaction.Commit();
            Committed = true;
        }

        public virtual void Rollback()
        {
            ThrowIfDisposed();

            if(Transaction is not null && Transaction.Connection is not null) {
                Transaction.Rollback();
            }
        }

        public IDataReader GetDataReader(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return DatabaseAccessor.GetDataReader(this, statement, parameter);
        }

        public Task<IDataReader> GetDataReaderAsync(string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            return DatabaseAccessor.GetDataReaderAsync(this, statement, parameter, cancellationToken);
        }

        public DataTable GetDataTable(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return DatabaseAccessor.GetDataTable(this, statement, parameter);
        }

        public Task<DataTable> GetDataTableAsync(string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            return DatabaseAccessor.GetDataTableAsync(statement, parameter, cancellationToken);
        }

        public virtual TResult? GetScalar<TResult>(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return DatabaseAccessor.GetScalar<TResult?>(this, statement, parameter);
        }

        public virtual Task<TResult?> GetScalarAsync<TResult>(string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            return DatabaseAccessor.GetScalarAsync<TResult?>(this, statement, parameter, cancellationToken);
        }

        public IEnumerable<T> Query<T>(string statement, object? parameter = null, bool buffered = true)
        {
            ThrowIfDisposed();

            return DatabaseAccessor.Query<T>(this, statement, parameter, buffered);
        }

        public IEnumerable<dynamic> Query(string statement, object? parameter = null, bool buffered = true)
        {
            ThrowIfDisposed();

            return DatabaseAccessor.Query(this, statement, parameter, buffered);
        }

        public Task<IEnumerable<T>> QueryAsync<T>(string statement, object? parameter = null, bool buffered = true, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            return DatabaseAccessor.QueryAsync<T>(statement, parameter, buffered, cancellationToken);
        }

        public Task<IEnumerable<dynamic>> QueryAsync(string statement, object? parameter = null, bool buffered = true, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            return DatabaseAccessor.QueryAsync(statement, parameter, buffered, cancellationToken);
        }

        public T QueryFirst<T>(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return DatabaseAccessor.QueryFirst<T>(this, statement, parameter);
        }

        public Task<T> QueryFirstAsync<T>(string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            return DatabaseAccessor.QueryFirstAsync<T>(this, statement, parameter, cancellationToken);
        }

        [return: MaybeNull]
        public T QueryFirstOrDefault<T>(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return DatabaseAccessor.QueryFirstOrDefault<T>(this, statement, parameter);
        }

        public Task<T?> QueryFirstOrDefaultAsync<T>(string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            return DatabaseAccessor.QueryFirstOrDefaultAsync<T>(this, statement, parameter, cancellationToken);
        }

        public T QuerySingle<T>(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return DatabaseAccessor.QuerySingle<T>(this, statement, parameter);
        }

        public Task<T> QuerySingleAsync<T>(string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            return DatabaseAccessor.QuerySingleAsync<T>(statement, parameter, cancellationToken);
        }

        [return: MaybeNull]
        public T QuerySingleOrDefault<T>(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return DatabaseAccessor.QuerySingleOrDefault<T>(this, statement, parameter);
        }

        public Task<T?> QuerySingleOrDefaultAsync<T>(string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            return DatabaseAccessor.QuerySingleOrDefaultAsync<T>(this, statement, parameter, cancellationToken);
        }

        public virtual int Execute(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return DatabaseAccessor.Execute(this, statement, parameter);
        }

        public virtual Task<int> ExecuteAsync(string statement, object? parameter = null, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            return DatabaseAccessor.ExecuteAsync(this, statement, parameter, cancellationToken);
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
                    if(Transaction is not null) {
                        Transaction.Dispose();
                    }
                    Transaction = null;
                    DatabaseAccessor = null!;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
