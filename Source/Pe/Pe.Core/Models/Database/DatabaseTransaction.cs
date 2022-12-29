using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;

namespace ContentTypeTextNet.Pe.Core.Models.Database
{
    /// <summary>
    /// データベース実装におけるトランザクション処理。
    /// <para>これが実体化されてればトランザクション中でしょうね。</para>
    /// </summary>
    public interface IDatabaseTransaction: IDatabaseContext, IDatabaseContexts, IDisposable
    {
        #region property

        /// <summary>
        /// CRL上のトランザクション実体。
        /// </summary>
        IDbTransaction Transaction { get; }

        #endregion

        #region function

        /// <summary>
        /// コミット！
        /// </summary>
        void Commit();

        /// <summary>
        /// なかったことにしたい人生の一部。
        /// </summary>
        void Rollback();

        #endregion
    }

    /// <summary>
    /// トランザクション中の処理をサポート。
    /// <para>基本的にはユーザーコードでお目にかからない。往々にして<see cref="IDatabaseContext"/>がすべて上位から良しなに対応する。</para>
    /// </summary>
    public class DatabaseTransaction: DisposerBase, IDatabaseTransaction
    {
        public DatabaseTransaction(IDatabaseAccessor databaseAccessor)
        {
            DatabaseAccessor = databaseAccessor;
            Implementation = DatabaseAccessor.DatabaseFactory.CreateImplementation();
            Transaction = DatabaseAccessor.BaseConnection.BeginTransaction();
        }

        public DatabaseTransaction(IDatabaseAccessor databaseAccessor, IsolationLevel isolationLevel)
        {
            DatabaseAccessor = databaseAccessor;
            Implementation = DatabaseAccessor.DatabaseFactory.CreateImplementation();
            Transaction = DatabaseAccessor.BaseConnection.BeginTransaction(isolationLevel);
        }

        #region property

        IDatabaseAccessor DatabaseAccessor { get; [Unuse(UnuseKinds.Dispose)] set; }
        public bool Committed { get; private set; }

        #endregion

        #region IDatabaseTransaction

        /// <summary>
        /// <see cref="IDatabaseContext"/>としての自身を返す。
        /// </summary>
        public IDatabaseContext Context => this;
        public IDbTransaction Transaction { get; [Unuse(UnuseKinds.Dispose)] private set; }
        public IDatabaseImplementation Implementation { get; }

        public virtual void Commit()
        {
            ThrowIfDisposed();

            Committed = true;
            Transaction.Commit();
        }

        public virtual void Rollback()
        {
            ThrowIfDisposed();

            Transaction.Rollback();
        }

        public IEnumerable<T> Query<T>(string statement, object? parameter = null, bool buffered = true)
        {
            ThrowIfDisposed();

            return DatabaseAccessor.Query<T>(statement, parameter, this, buffered);
        }

        public Task<IEnumerable<T>> QueryAsync<T>(string statement, object? parameter = null, bool buffered = true, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            return DatabaseAccessor.QueryAsync<T>(statement, parameter, buffered, cancellationToken);
        }

        public IEnumerable<dynamic> Query(string statement, object? parameter = null, bool buffered = true)
        {
            ThrowIfDisposed();

            return DatabaseAccessor.Query(statement, parameter, this, buffered);
        }

        public Task<IEnumerable<dynamic>> QueryAsync(string statement, object? parameter = null, bool buffered = true, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();

            return DatabaseAccessor.QueryAsync(statement, parameter, buffered, cancellationToken);
        }

        public T QueryFirst<T>(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return DatabaseAccessor.QueryFirst<T>(statement, parameter, this);
        }

        [return: MaybeNull]
        public T QueryFirstOrDefault<T>(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return DatabaseAccessor.QueryFirstOrDefault<T>(statement, parameter, this);
        }

        public T QuerySingle<T>(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return DatabaseAccessor.QuerySingle<T>(statement, parameter, this);
        }

        [return: MaybeNull]
        public T QuerySingleOrDefault<T>(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return DatabaseAccessor.QuerySingleOrDefault<T>(statement, parameter, this);
        }

        public virtual int Execute(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return DatabaseAccessor.Execute(statement, parameter, this);
        }

        public DataTable GetDataTable(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return DatabaseAccessor.GetDataTable(statement, parameter, this);
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
                    Transaction = null!;
                    DatabaseAccessor = null!;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    public class ReadOnlyDatabaseTransaction: DatabaseTransaction
    {
        public ReadOnlyDatabaseTransaction(IDatabaseAccessor databaseAccessor)
            : base(databaseAccessor)
        { }

        public ReadOnlyDatabaseTransaction(IDatabaseAccessor databaseAccessor, IsolationLevel isolationLevel)
            : base(databaseAccessor, isolationLevel)
        { }

        #region DatabaseTransaction

        public override void Commit() => throw new NotSupportedException();

        public override int Execute(string statement, object? parameter = null) => throw new NotSupportedException();

        #endregion

    }
}
