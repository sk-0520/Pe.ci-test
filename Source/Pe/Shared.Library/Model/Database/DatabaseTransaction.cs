using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database
{
    /// <summary>
    /// データベース実装におけるトランザクション処理。
    /// <para>これが実体化されてればトランザクション中でしょうね。</para>
    /// </summary>
    public interface IDatabaseTransaction : IDatabaseCommander, IDisposable
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
    /// <para>基本的にはユーザーコードでお目にかからない。往々にして<see cref="IDatabaseCommander"/>がすべて上位から良しなに対応する。</para>
    /// </summary>
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

        public T QueryFirst<T>(string sql, object param = null)
        {
            return DatabaseAccessor.QueryFirst<T>(sql, param, this);
        }

        public T QueryFirstOrDefault<T>(string sql, object param = null)
        {
            return DatabaseAccessor.QueryFirstOrDefault<T>(sql, param, this);
        }

        public T QuerySingle<T>(string sql, object param = null)
        {
            return DatabaseAccessor.QuerySingle<T>(sql, param, this);
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
