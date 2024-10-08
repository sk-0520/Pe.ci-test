using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Models.Database.Vender.Public.SQLite
{
    public class SqliteAccessor: DatabaseAccessor<SQLiteConnection>
    {
        public SqliteAccessor(IDatabaseFactory databaseFactory, ILogger logger)
            : base(databaseFactory, logger)
        { }

        public SqliteAccessor(IDatabaseFactory databaseFactory, ILoggerFactory loggerFactory)
            : base(databaseFactory, loggerFactory)
        { }

        #region function

        /// <summary>
        /// データベースのコピー。
        /// </summary>
        /// <param name="sourceName">コピー元DB名。</param>
        /// <param name="destination">コピー先の<see cref="SqliteAccessor"/></param>
        /// <param name="destinationName">コピー先DB名。</param>
        public void CopyTo(string sourceName, SqliteAccessor destination, string destinationName)
        {
            Connection.BackupDatabase(destination.Connection, destinationName, sourceName, -1, null, -1);
        }

        #endregion

        #region DatabaseAccessor

        public override IDatabaseTransaction BeginReadOnlyTransaction()
        {
            ThrowIfDisposed();

            return new ReadOnlyDatabaseTransaction(false, this);
        }
        public override IDatabaseTransaction BeginReadOnlyTransaction(IsolationLevel isolationLevel)
        {
            ThrowIfDisposed();

            return new ReadOnlyDatabaseTransaction(false, this, isolationLevel);
        }

        #endregion
    }
}
