using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
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

        public int Execute(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return Transaction.Execute(statement, parameter);
        }

        public DataTable GetDataTable(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return Transaction.GetDataTable(statement, parameter);
        }

        public IEnumerable<T> Query<T>(string statement, object? parameter = null, bool buffered = true)
        {
            ThrowIfDisposed();

            return Transaction.Query<T>(statement, parameter, buffered);
        }

        public IEnumerable<dynamic> Query(string statement, object? parameter = null, bool buffered = true)
        {
            ThrowIfDisposed();

            return Transaction.Query(statement, parameter, buffered);
        }

        public T QueryFirst<T>(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return Transaction.QueryFirst<T>(statement, parameter);
        }

        [return: MaybeNull]
        public T QueryFirstOrDefault<T>(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return Transaction.QueryFirstOrDefault<T>(statement, parameter);
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
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    /// <summary>
    /// データベースに対する読み書き制御。
    /// <para>NOTE: 役割が完全にSQLiteに合わせた挙動。</para>
    /// </summary>
    public interface IDatabaseBarrier
    {
        #region function

        /// <summary>
        /// 既定の待機時間で書き込み処理を実施する。
        /// </summary>
        /// <returns></returns>
        IDatabaseTransaction WaitWrite();
        /// <summary>
        /// 既定の待機時間で読み込み処理を実施する。
        /// </summary>
        /// <returns></returns>
        IDatabaseTransaction WaitRead();

        #endregion
    }

    public static class IDatabaseBarrierExtensions
    {
        #region function

        public static TResult ReadData<TResult>(this IDatabaseBarrier @this, Func<IDatabaseTransaction, TResult> func)
        {
            using var transaction = @this.WaitRead();
            return func(transaction);
        }

        public static TResult ReadData<TArgument, TResult>(this IDatabaseBarrier @this, Func<IDatabaseTransaction, TArgument, TResult> func, TArgument argument)
        {
            using var transaction = @this.WaitRead();
            return func(transaction, argument);
        }

        #endregion
    }

    /// <inheritdoc cref="IDatabaseBarrier" />
    public class DatabaseBarrier : IDatabaseBarrier
    {
        public DatabaseBarrier(IDatabaseAccessor accessor, ReaderWriterLocker locker)
        {
            Accessor = accessor;
            Locker = locker;
        }

        #region property

        protected IDatabaseAccessor Accessor { get; }
        protected ReaderWriterLocker Locker { get; }

        #endregion

        #region IDatabaseBarrier

        //public IDatabaseAccessor Accessor { get; }
        //public ReaderWriterLocker Locker { get; }

        /// <summary>
        /// <inheritdoc cref="IDatabaseBarrier.WaitWrite" />
        /// <para><see cref="Locker.WaitWriteByDefaultTimeout()"/>が規定時間。</para>
        /// </summary>
        public virtual IDatabaseTransaction WaitWrite()
        {
            var locker = Locker.WaitWriteByDefaultTimeout();
            var transaction = Accessor.BeginTransaction();
            var result = new DatabaseBarrierTransaction(locker, transaction, Accessor.DatabaseFactory.CreateImplementation());
            return result;
        }

        /// <summary>
        /// <inheritdoc cref="IDatabaseBarrier.WaitRead" />
        /// <para><see cref="Locker.WaitReadByDefaultTimeout()"/>が規定時間。</para>
        /// </summary>
        /// <returns></returns>
        public virtual IDatabaseTransaction WaitRead()
        {
            var locker = Locker.WaitReadByDefaultTimeout();
            var transaction = Accessor.BeginReadOnlyTransaction();
            var result = new DatabaseBarrierTransaction(locker, transaction, Accessor.DatabaseFactory.CreateImplementation());
            return result;
        }

        #endregion
    }

}
