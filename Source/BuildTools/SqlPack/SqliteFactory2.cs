using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Database.Vender.Public.SQLite;

namespace SqlPack
{
    public class SqliteFactory2 : ConnectionStringSqliteFactory
    {
        public SqliteFactory2(string databasePath)
        {
            DatabasePath = databasePath;
        }

        #region property

        string DatabasePath { get; }

        #endregion

        public override IDbConnection CreateConnection()
        {
            var connectionString = CreateConnectionBuilder();
            connectionString.DataSource = DatabasePath;
            return new SQLiteConnection(connectionString.ToString());
        }
    }
}
