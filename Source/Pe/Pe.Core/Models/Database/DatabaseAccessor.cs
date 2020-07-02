using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using Dapper;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Models.Database
{
    public interface IDatabaseReader
    {
        /// <summary>
        /// 指定の型で問い合わせ。
        /// </summary>
        /// <typeparam name="T">問い合わせ型</typeparam>
        /// <param name="statement">データベース文。</parameter>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</parameter>
        /// <param name="buffered"><see cref="Dapper.SqlMapper.Query"/>のbufferd</parameter>
        /// <returns></returns>
        IEnumerable<T> Query<T>(string statement, object? parameter = null, bool buffered = true);
        /// <summary>
        /// 動的型で問い合わせ。
        /// </summary>
        /// <param name="statement">データベース文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <param name="buffered"><see cref="Dapper.SqlMapper.Query"/>のbufferd</parameter>
        /// <returns></returns>
        IEnumerable<dynamic> Query(string statement, object? parameter = null, bool buffered = true);

        /// <summary>
        /// 最初のデータを取得。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="statement"></param>
        /// <param name="parameter"></param>
        /// <exception cref="InvalidOperationException">空っぽ。</exception>
        /// <returns>一番最初に見つかったデータ。</returns>
        T QueryFirst<T>(string statement, object? parameter = null);
        /// <summary>
        /// 最初のデータを取得。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="statement"></param>
        /// <param name="parameter"></param>
        /// <returns>一番最初に見つかったデータ。見つかんなかったら default(T)</returns>
        [return: MaybeNull]
        T QueryFirstOrDefault<T>(string statement, object? parameter = null);
        /// <summary>
        /// 単一データ取得。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="statement"></param>
        /// <param name="parameter"></param>
        /// <exception cref="InvalidOperationException">空っぽか複数あり。</exception>
        /// <returns></returns>
        T QuerySingle<T>(string statement, object? parameter = null);

        /// <summary>
        /// <see cref="DataTable"/> でデータ取得。
        /// </summary>
        /// <param name="statement"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        DataTable GetDataTable(string statement, object? parameter = null);
    }

    public interface IDatabaseWriter
    {
        /// <summary>
        /// insert, update, delete, select(sequence) 的なデータ変動するやつを実行。
        /// </summary>
        /// <param name="statement">データベース文。</parameter>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</parameter>
        /// <returns>影響行数。</returns>
        int Execute(string statement, object? parameter = null);
    }

    /// <summary>
    /// データベースとの会話用インターフェイス。
    /// </summary>
    public interface IDatabaseCommander : IDatabaseReader, IDatabaseWriter
    { }

    public interface IDatabaseAccessor : IDatabaseCommander
    {
        #region property

        /// <summary>
        /// 接続元。
        /// </summary>
        IDbConnection BaseConnection { get; }
        /// <summary>
        /// <see cref="IDatabaseFactory"/>
        /// </summary>
        IDatabaseFactory DatabaseFactory { get; }

        #endregion

        #region function

        /// <summary>
        /// 一時的に切断状態へ遷移。
        /// <para><see cref="IDisposable.Dispose()"/>が完了するまでの間接続できない状態になる。</para>
        /// </summary>
        /// <returns></returns>
        IDisposable StopConnection();

        /// <summary>
        /// 指定の型で問い合わせ。
        /// </summary>
        /// <typeparam name="T">問い合わせ型</typeparam>
        /// <param name="statement">データベース文。</parameter>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</parameter>
        /// <param name="transaction">トランザクション。 null ならトランザクションなし。</parameter>
        /// <param name="buffered"><see cref="Dapper.SqlMapper.Query"/>のbufferd</parameter>
        /// <returns></returns>
        IEnumerable<T> Query<T>(string statement, object? parameter, IDatabaseTransaction? transaction, bool buffered);
        /// <summary>
        /// 動的型で問い合わせ。
        /// </summary>
        /// <param name="statement">データベース文。</param>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</param>
        /// <param name="transaction">トランザクション。 null ならトランザクションなし。</parameter>
        /// <param name="buffered"><see cref="Dapper.SqlMapper.Query"/>のbufferd</parameter>
        /// <returns></returns>
        IEnumerable<dynamic> Query(string statement, object? parameter, IDatabaseTransaction? transaction, bool buffered);

        T QueryFirst<T>(string statement, object? parameter, IDatabaseTransaction? transaction);
        [return: MaybeNull]
        T QueryFirstOrDefault<T>(string statement, object? parameter, IDatabaseTransaction? transaction);
        T QuerySingle<T>(string statement, object? parameter, IDatabaseTransaction? transaction);

        /// <summary>
        /// insert, update, delete, select(sequence) 的なデータ変動するやつを実行。
        /// </summary>
        /// <param name="statement">データベース文。</parameter>
        /// <param name="parameter"><paramref name="statement"/>に対するパラメータ。</parameter>
        /// <param name="transaction">トランザクション。 null ならトランザクションなし。</parameter>
        /// <returns>影響行数。</returns>
        int Execute(string statement, object? parameter, IDatabaseTransaction? transaction);
        DataTable GetDataTable(string statement, object? parameter, IDatabaseTransaction? transaction);

        IDatabaseTransaction BeginTransaction();
        IDatabaseTransaction BeginTransaction(IsolationLevel isolationLevel);
        IDatabaseTransaction BeginReadOnlyTransaction();
        IDatabaseTransaction BeginReadOnlyTransaction(IsolationLevel isolationLevel);

        IResultFailureValue<Exception> Batch(Func<IDatabaseCommander, bool> executor);
        IResultFailureValue<Exception> Batch(Func<IDatabaseCommander, bool> executor, IsolationLevel isolationLevel);

        #endregion
    }

    /// <summary>
    /// DBアクセスに対してラップする。
    /// <para>DBまで行く前にプログラム側で制御する目的。</para>
    /// </summary>
    public class DatabaseAccessor : DisposerBase, IDatabaseAccessor
    {
        public DatabaseAccessor(IDatabaseFactory databaseFactory, ILogger logger)
        {
            Logger = logger;
            DatabaseFactory = databaseFactory;
            LazyConnection = new Lazy<IDbConnection>(OpenConnection);
            LazyImplementation = new Lazy<IDatabaseImplementation>(DatabaseFactory.CreateImplementation);
        }

        public DatabaseAccessor(IDatabaseFactory databaseFactory, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            DatabaseFactory = databaseFactory;
            LazyConnection = new Lazy<IDbConnection>(OpenConnection);
            LazyImplementation = new Lazy<IDatabaseImplementation>(DatabaseFactory.CreateImplementation);
        }

        #region property

        Lazy<IDbConnection> LazyConnection { get; set; }

        Lazy<IDatabaseImplementation> LazyImplementation { get; }
        protected IDatabaseImplementation Implementation => LazyImplementation.Value;

        protected ILogger Logger { get; }

        public bool IsOpend {get; private set;}
        public bool StoppingConnection { get; private set;}

        #endregion

        #region function

        IDbConnection OpenConnection()
        {
            if(StoppingConnection) {
                throw new InvalidOperationException(nameof(StoppingConnection));
            }
            if(IsOpend) {
                throw new InvalidOperationException(nameof(IsOpend));
            }
            ThrowIfDisposed();

            var con = DatabaseFactory.CreateConnection();
            con.Open();
            IsOpend = true;
            return con;
        }

        protected virtual IResultFailureValue<Exception> BatchImpl(Func<IDatabaseTransaction> transactionCreator, Func<IDatabaseCommander, bool> function)
        {
            ThrowIfDisposed();

            var transaction = transactionCreator();
            try {
                var commit = function(transaction);
                if(commit) {
                    transaction.Commit();
                } else {
                    transaction.Rollback();
                }
                return ResultFailureValue.Success<Exception>();
            } catch(Exception ex) {
                transaction.Rollback();
                return ResultFailureValue.Failure(ex);
            }
        }

        protected virtual void LoggingStatement(string statement, object? parameter)
        {
            Logger.LogTrace(statement, parameter);
        }

        protected virtual void LoggingExecuteResult(int result, [DateTimeKind(DateTimeKind.Local)] DateTime startTime, [DateTimeKind(DateTimeKind.Local)] DateTime endTime)
        {
            Logger.LogTrace($"result: {result}, {endTime - startTime}", new { startTime, endTime });
        }
        protected virtual void LoggingDataTable(DataTable table, [DateTimeKind(DateTimeKind.Local)] DateTime startTime, [DateTimeKind(DateTimeKind.Local)] DateTime endTime)
        {
            Logger.LogTrace($"table: {table.TableName} -> {table.Columns.Count} * {table.Rows.Count}, {endTime - startTime}", new { startTime, endTime });
        }

        #endregion

        #region IDatabaseAccessor

        /// <inheritdoc cref="IDatabaseAccessor.DatabaseFactory"/>
        public IDatabaseFactory DatabaseFactory { get; }

        /// <inheritdoc cref="IDatabaseAccessor.BaseConnection"/>
        public virtual IDbConnection BaseConnection => LazyConnection.Value;

        /// <inheritdoc cref="IDatabaseAccessor.StopConnection"/>
        public virtual IDisposable StopConnection()
        {
            ThrowIfDisposed();

            if(!IsOpend) {
                return new ActionDisposer(d => { });
            }

            if(!StoppingConnection) {
                BaseConnection.Close();
                IsOpend = false;
                StoppingConnection = true;
                return new ActionDisposer(d => {
                    StoppingConnection = false;
                    LazyConnection = new Lazy<IDbConnection>(OpenConnection);
                });
            }

            return new ActionDisposer(d => { });
        }

        public virtual IEnumerable<T> Query<T>(string statement, object? parameter, IDatabaseTransaction? transaction, bool buffered)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);
            return BaseConnection.Query<T>(formattedStatement, parameter, transaction?.Transaction, buffered);
        }

        public IEnumerable<T> Query<T>(string statement, object? parameter = null, bool buffered = true)
        {
            ThrowIfDisposed();

            return Query<T>(statement, parameter, null, buffered);
        }

        public virtual IEnumerable<dynamic> Query(string statement, object? parameter, IDatabaseTransaction? transaction, bool buffered)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);
            return BaseConnection.Query(formattedStatement, parameter, transaction?.Transaction, buffered);
        }

        /// <inheritdoc cref="IDatabaseReader.Query(string, object?, bool)"/>
        public IEnumerable<dynamic> Query(string statement, object? parameter = null, bool buffered = true)
        {
            ThrowIfDisposed();

            return Query(statement, parameter, null, buffered);
        }

        public virtual T QueryFirst<T>(string statement, object? parameter, IDatabaseTransaction? transaction)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);
            return BaseConnection.QueryFirst<T>(formattedStatement, parameter, transaction?.Transaction);
        }

        public virtual T QueryFirst<T>(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return QueryFirst<T>(statement, parameter, null);
        }

        [return: MaybeNull]
        public virtual T QueryFirstOrDefault<T>(string statement, object? parameter, IDatabaseTransaction? transaction)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);
            return BaseConnection.QueryFirstOrDefault<T>(formattedStatement, parameter, transaction?.Transaction);
        }

        [return: MaybeNull]
        public T QueryFirstOrDefault<T>(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return QueryFirstOrDefault<T>(statement, parameter, null);
        }

        public virtual T QuerySingle<T>(string statement, object? parameter, IDatabaseTransaction? transaction)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);
            return BaseConnection.QuerySingle<T>(formattedStatement, parameter, transaction?.Transaction);
        }

        public virtual T QuerySingle<T>(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return QuerySingle<T>(statement, parameter, null);
        }

        public virtual int Execute(string statement, object? parameter, IDatabaseTransaction? transaction)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);
            var startTime = DateTime.UtcNow;
            var result = BaseConnection.Execute(formattedStatement, parameter, transaction?.Transaction);
            LoggingExecuteResult(result, startTime, DateTime.UtcNow);
            return result;
        }

        public int Execute(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return Execute(statement, parameter, null);
        }

        public virtual DataTable GetDataTable(string statement, object? parameter, IDatabaseTransaction? transaction)
        {
            ThrowIfDisposed();

            var formattedStatement = Implementation.PreFormatStatement(statement);

            LoggingStatement(formattedStatement, parameter);

            var dataTable = new DataTable();
            var startTime = DateTime.UtcNow;
            dataTable.Load(BaseConnection.ExecuteReader(statement, parameter, transaction?.Transaction));
            LoggingDataTable(dataTable, startTime, DateTime.UtcNow);
            return dataTable;
        }

        public DataTable GetDataTable(string statement, object? parameter = null)
        {
            ThrowIfDisposed();

            return GetDataTable(statement, parameter, null);
        }

        /// <summary>
        /// トランザクション開始。
        /// </summary>
        /// <returns></returns>
        public virtual IDatabaseTransaction BeginTransaction()
        {
            ThrowIfDisposed();

            return new DatabaseTransaction(this);
        }

        /// <summary>
        /// トランザクション開始。
        /// </summary>
        /// <param name="isolationLevel">トランザクションの分離レベル。</param>
        /// <returns></returns>
        public virtual IDatabaseTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            ThrowIfDisposed();

            return new DatabaseTransaction(this, isolationLevel);
        }

        public virtual IDatabaseTransaction BeginReadOnlyTransaction()
        {
            ThrowIfDisposed();

            return new ReadOnlyDatabaseTransaction(this);
        }
        public virtual IDatabaseTransaction BeginReadOnlyTransaction(IsolationLevel isolationLevel)
        {
            ThrowIfDisposed();

            return new ReadOnlyDatabaseTransaction(this, isolationLevel);
        }


        public IResultFailureValue<Exception> Batch(Func<IDatabaseCommander, bool> executor)
        {
            ThrowIfDisposed();

            return BatchImpl(() => new DatabaseTransaction(this), executor);
        }

        public IResultFailureValue<Exception> Batch(Func<IDatabaseCommander, bool> executor, IsolationLevel isolationLevel)
        {
            ThrowIfDisposed();

            return BatchImpl(() => new DatabaseTransaction(this, isolationLevel), executor);
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    if(LazyConnection.IsValueCreated) {
                        BaseConnection.Dispose();
                    }
                }
                IsOpend = false;
                StoppingConnection = false;
            }

            base.Dispose(disposing);
        }

        #endregion
    }

    public class DatabaseAccessor<TDbConnection> : DatabaseAccessor
        where TDbConnection : IDbConnection
    {
        public DatabaseAccessor(IDatabaseFactory connectionFactory, ILogger logger)
            : base(connectionFactory, logger)
        { }

        public DatabaseAccessor(IDatabaseFactory connectionFactory, ILoggerFactory loggerFactory)
            : base(connectionFactory, loggerFactory)
        { }

        #region proeprty


        /// <summary>
        /// 接続元。
        /// </summary>
        public TDbConnection Connection => (TDbConnection)BaseConnection;

        #endregion
    }
}
