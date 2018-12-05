using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using Dapper;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database
{
    public interface IDatabaseCommander
    {
        #region function

        IEnumerable<T> Query<T>(string sql, object param = null, bool buffered = true);

        IEnumerable<dynamic> Query(string sql, object param = null, bool buffered = true);

        int Execute(string sql, object param = null);

        DataTable GetDataTable(string sql, object param = null);

        #endregion
    }

    public interface IDatabaseAccessor: IDatabaseCommander
    {
        #region property

        IDbConnection BaseConnection { get; }

        #endregion

        #region function

        IEnumerable<T> Query<T>(string sql, object param, IDatabaseTransaction transaction, bool buffered);
        IEnumerable<dynamic> Query(string sql, object param, IDatabaseTransaction transaction, bool buffered);
        int Execute(string sql, object param, IDatabaseTransaction transaction);
        DataTable GetDataTable(string sql, object param, IDatabaseTransaction transaction);

        IDatabaseTransaction BeginTransaction();
        IDatabaseTransaction BeginTransaction(IsolationLevel isolationLevel);

        IResultFailureValue<Exception> Batch(Action<IDatabaseCommander> action);
        IResultFailureValue<Exception> Batch(Action<IDatabaseCommander> action, IsolationLevel isolationLevel);

        #endregion
    }

    /// <summary>
    /// DBアクセスに対してラップする。
    /// <para>DBまで行く前にプログラム側で制御する目的。</para>
    /// </summary>
    public class DatabaseAccessor : ReaderWriterLocker, IDatabaseAccessor
    {
        public DatabaseAccessor(IDatabaseFactory databaseFactory, ILogger logger)
        {
            DatabaseFactory = databaseFactory;

            Logger = logger;

            LazyConnection = new Lazy<IDbConnection>(() => {
                var con = DatabaseFactory.CreateConnection();
                con.Open();
                return con;
            });

            LazyImplementation = new Lazy<IDatabaseImplementation>(DatabaseFactory.CreateImplementation);
        }

        public DatabaseAccessor(IDatabaseFactory databaseFactory, ILogFactory logFactory)
            : this(databaseFactory, logFactory.CreateCurrentClass())
        { }

        #region property

        Lazy<IDbConnection> LazyConnection { get; }

        Lazy<IDatabaseImplementation> LazyImplementation { get; }
        protected IDatabaseImplementation Implementation => LazyImplementation.Value;

        protected ILogger Logger { get; }

        #endregion

        #region function

        protected virtual IResultFailureValue<Exception> BatchCore(Func<IDatabaseTransaction> transactionCreator, Action<IDatabaseCommander> action)
        {
            var transaction = transactionCreator();
            try {
                action(transaction);
                return ResultFailureValue.Success<Exception>();
            } catch(Exception ex) {
                transaction.Rollback();
                return ResultFailureValue.Failure(ex);
            }
        }

        #endregion

        #region IDatabaseAccessor

        public IDatabaseFactory DatabaseFactory { get; }

        public virtual IDbConnection BaseConnection => LazyConnection.Value;

        public virtual IEnumerable<T> Query<T>(string sql, object param, IDatabaseTransaction transaction, bool buffered)
        {
            var formattedSql = Implementation.PreFormatSql(sql);

            Logger.Debug(formattedSql, param);
            return BaseConnection.Query<T>(formattedSql, param, transaction?.Transaction, buffered);
        }

        public IEnumerable<T> Query<T>(string sql, object param = null, bool buffered = true)
        {
            return Query<T>(sql, param, null, buffered);
        }

        public virtual IEnumerable<dynamic> Query(string sql, object param, IDatabaseTransaction transaction, bool buffered)
        {
            var formattedSql = Implementation.PreFormatSql(sql);

            Logger.Debug(formattedSql, param);
            return BaseConnection.Query(formattedSql, param, transaction?.Transaction, buffered);
        }

        public IEnumerable<dynamic> Query(string sql, object param = null, bool buffered = true)
        {
            return Query(sql, param, null, buffered);
        }

        public virtual int Execute(string sql, object param, IDatabaseTransaction transaction)
        {
            var formattedSql = Implementation.PreFormatSql(sql);

            Logger.Debug(formattedSql, param);
            return BaseConnection.Execute(formattedSql, param, transaction?.Transaction);
        }

        public int Execute(string sql, object param = null)
        {
            return Execute(sql, param, null);
        }

        public virtual DataTable GetDataTable(string sql, object param, IDatabaseTransaction transaction)
        {
            var formattedSql = Implementation.PreFormatSql(sql);

            Logger.Debug(formattedSql, param);

            var dataTable = new DataTable();
            dataTable.Load(BaseConnection.ExecuteReader(sql, param, transaction?.Transaction));
            return dataTable;
        }

        public DataTable GetDataTable(string sql, object param = null)
        {
            return GetDataTable(sql, param, null);
        }

        public virtual IDatabaseTransaction BeginTransaction()
        {
            return new DatabaseTransaction(this);
        }

        public virtual IDatabaseTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return new DatabaseTransaction(this, isolationLevel);
        }

        public IResultFailureValue<Exception> Batch(Action<IDatabaseCommander> action)
        {
            return BatchCore(() => new DatabaseTransaction(this), action);
        }

        public IResultFailureValue<Exception> Batch(Action<IDatabaseCommander> action, IsolationLevel isolationLevel)
        {
            return BatchCore(() => new DatabaseTransaction(this, isolationLevel), action);
        }

        #endregion

        #region ReaderWriterLocker

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

        [Injection]
        public DatabaseAccessor(IDatabaseFactory connectionFactory, ILogFactory logFactory)
            : base(connectionFactory, logFactory.CreateCurrentClass())
        { }

        #region proeprty

        public TDbConnection Connection => (TDbConnection)BaseConnection;

        #endregion
    }
}
