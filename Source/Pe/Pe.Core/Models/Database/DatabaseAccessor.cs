using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
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
        /// <parameter name="statement">データベース文。</parameter>
        /// <parameter name="parameter"><paramref name="statement"/>に対するパラメータ。</parameter>
        /// <parameter name="buffered"><see cref="Dapper.SqlMapper.Query"/>のbufferd</parameter>
        /// <returns></returns>
        IEnumerable<T> Query<T>(string statement, object? parameter = null, bool buffered = true);

        IEnumerable<dynamic> Query(string statement, object? parameter = null, bool buffered = true);

        T QueryFirst<T>(string statement, object? parameter = null);
        T QueryFirstOrDefault<T>(string statement, object? parameter = null);
        T QuerySingle<T>(string statement, object? parameter = null);

        DataTable GetDataTable(string statement, object? parameter = null);
    }

    public interface IDatabaseWriter
    {
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

        IDbConnection BaseConnection { get; }
        IDatabaseFactory DatabaseFactory { get; }

        #endregion

        #region function

        IEnumerable<T> Query<T>(string statement, object? parameter, IDatabaseTransaction? transaction, bool buffered);
        IEnumerable<dynamic> Query(string statement, object? parameter, IDatabaseTransaction? transaction, bool buffered);

        T QueryFirst<T>(string statement, object? parameter, IDatabaseTransaction? transaction);
        T QueryFirstOrDefault<T>(string statement, object? parameter, IDatabaseTransaction? transaction);
        T QuerySingle<T>(string statement, object? parameter, IDatabaseTransaction? transaction);

        int Execute(string statement, object? parameter, IDatabaseTransaction? transaction);
        DataTable GetDataTable(string statement, object? parameter, IDatabaseTransaction? transaction);

        IDatabaseTransaction BeginTransaction();
        IDatabaseTransaction BeginTransaction(IsolationLevel isolationLevel);

        IResultFailureValue<Exception> Batch(Func<IDatabaseCommander, bool> action);
        IResultFailureValue<Exception> Batch(Func<IDatabaseCommander, bool> action, IsolationLevel isolationLevel);

        #endregion
    }

    /// <summary>
    /// DBアクセスに対してラップする。
    /// <para>DBまで行く前にプログラム側で制御する目的。</para>
    /// </summary>
    public class DatabaseAccessor : DisposerBase, IDatabaseAccessor
    {
#pragma warning disable CS8618 // Null 非許容フィールドが初期化されていません。
        private DatabaseAccessor(IDatabaseFactory databaseFactory)
#pragma warning restore CS8618 // Null 非許容フィールドが初期化されていません。
        {
            DatabaseFactory = databaseFactory;

            LazyConnection = new Lazy<IDbConnection>(() => {
                var con = DatabaseFactory.CreateConnection();
                con.Open();
                return con;
            });

            LazyImplementation = new Lazy<IDatabaseImplementation>(DatabaseFactory.CreateImplementation);
        }

        public DatabaseAccessor(IDatabaseFactory databaseFactory, ILogger logger)
            : this(databaseFactory)
        {
            Logger = logger;
        }

        public DatabaseAccessor(IDatabaseFactory databaseFactory, ILoggerFactory loggerFactory)
            : this(databaseFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        Lazy<IDbConnection> LazyConnection { get; }

        Lazy<IDatabaseImplementation> LazyImplementation { get; }
        protected IDatabaseImplementation Implementation => LazyImplementation.Value;

        protected ILogger Logger { get; }

        #endregion

        #region function

        protected virtual IResultFailureValue<Exception> BatchImpl(Func<IDatabaseTransaction> transactionCreator, Func<IDatabaseCommander, bool> function)
        {
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

        protected virtual void LoggingExecuteResult(int result, [Timestamp(DateTimeKind.Local)] DateTime startTime, [Timestamp(DateTimeKind.Local)] DateTime endTime)
        {
            Logger.LogTrace($"result: {result}, {endTime - startTime}", new { startTime, endTime });
        }
        protected virtual void LoggingDataTable(DataTable table, [Timestamp(DateTimeKind.Local)] DateTime startTime, [Timestamp(DateTimeKind.Local)] DateTime endTime)
        {
            Logger.LogTrace($"table: {table.TableName} -> {table.Columns.Count} * {table.Rows.Count}, {endTime - startTime}", new { startTime, endTime });
        }

        #endregion

        #region IDatabaseAccessor

        public IDatabaseFactory DatabaseFactory { get; }

        public virtual IDbConnection BaseConnection => LazyConnection.Value;

        public virtual IEnumerable<T> Query<T>(string statement, object? parameter, IDatabaseTransaction? transaction, bool buffered)
        {
            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);
            return BaseConnection.Query<T>(formattedStatement, parameter, transaction?.Transaction, buffered);
        }

        public IEnumerable<T> Query<T>(string statement, object? parameter = null, bool buffered = true)
        {
            return Query<T>(statement, parameter, null, buffered);
        }

        public virtual IEnumerable<dynamic> Query(string statement, object? parameter, IDatabaseTransaction? transaction, bool buffered)
        {
            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);
            return BaseConnection.Query(formattedStatement, parameter, transaction?.Transaction, buffered);
        }

        public IEnumerable<dynamic> Query(string statement, object? parameter = null, bool buffered = true)
        {
            return Query(statement, parameter, null, buffered);
        }

        public virtual T QueryFirst<T>(string statement, object? parameter, IDatabaseTransaction? transaction)
        {
            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);
            return BaseConnection.QueryFirst<T>(formattedStatement, parameter, transaction?.Transaction);
        }

        public virtual T QueryFirst<T>(string statement, object? parameter = null)
        {
            return QueryFirst<T>(statement, parameter);
        }

        public virtual T QueryFirstOrDefault<T>(string statement, object? parameter, IDatabaseTransaction? transaction)
        {
            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);
            return BaseConnection.QueryFirstOrDefault<T>(formattedStatement, parameter, transaction?.Transaction);
        }

        public T QueryFirstOrDefault<T>(string statement, object? parameter = null)
        {
            return QueryFirstOrDefault<T>(statement, parameter, null);
        }

        public virtual T QuerySingle<T>(string statement, object? parameter, IDatabaseTransaction? transaction)
        {
            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);
            return BaseConnection.QuerySingle<T>(formattedStatement, parameter, transaction?.Transaction);
        }

        public virtual T QuerySingle<T>(string statement, object? parameter = null)
        {
            return QuerySingle<T>(statement, parameter);
        }

        public virtual int Execute(string statement, object? parameter, IDatabaseTransaction? transaction)
        {
            var formattedStatement = Implementation.PreFormatStatement(statement);
            LoggingStatement(formattedStatement, parameter);
            var startTime = DateTime.Now;
            var result = BaseConnection.Execute(formattedStatement, parameter, transaction?.Transaction);
            LoggingExecuteResult(result, startTime, DateTime.Now);
            return result;
        }

        public int Execute(string statement, object? parameter = null)
        {
            return Execute(statement, parameter, null);
        }

        public virtual DataTable GetDataTable(string statement, object? parameter, IDatabaseTransaction? transaction)
        {
            var formattedStatement = Implementation.PreFormatStatement(statement);

            LoggingStatement(formattedStatement, parameter);

            var dataTable = new DataTable();
            var startTime = DateTime.Now;
            dataTable.Load(BaseConnection.ExecuteReader(statement, parameter, transaction?.Transaction));
            LoggingDataTable(dataTable, startTime, DateTime.Now);
            return dataTable;
        }

        public DataTable GetDataTable(string statement, object? parameter = null)
        {
            return GetDataTable(statement, parameter, null);
        }

        public virtual IDatabaseTransaction BeginTransaction()
        {
            return new DatabaseTransaction(this);
        }

        public virtual IDatabaseTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return new DatabaseTransaction(this, isolationLevel);
        }

        public IResultFailureValue<Exception> Batch(Func<IDatabaseCommander, bool> function)
        {
            return BatchImpl(() => new DatabaseTransaction(this), function);
        }

        public IResultFailureValue<Exception> Batch(Func<IDatabaseCommander, bool> function, IsolationLevel isolationLevel)
        {
            return BatchImpl(() => new DatabaseTransaction(this, isolationLevel), function);
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

        public TDbConnection Connection => (TDbConnection)BaseConnection;

        #endregion
    }
}
