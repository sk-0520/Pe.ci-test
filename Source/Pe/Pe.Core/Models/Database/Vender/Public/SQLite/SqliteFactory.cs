#define USE_DB_SQLITE
#if USE_DB_SQLITE

using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using ContentTypeTextNet.Pe.Standard.Database;

namespace ContentTypeTextNet.Pe.Core.Models.Database.Vender.Public.SQLite
{
    public abstract class SqliteFactory: IDatabaseFactory
    {
        #region function

        protected static FileInfo ToSafeFile(FileInfo fileInfo)
        {
            // #66 を考慮
            if(fileInfo.FullName.StartsWith(@"\\")) {
                return new FileInfo(@"\\" + fileInfo.FullName);
            }

            return fileInfo;
        }

        protected static SQLiteConnectionStringBuilder CreateConnectionBuilder()
        {
            return new SQLiteConnectionStringBuilder() {
                DateTimeKind = DateTimeKind.Utc,
                BinaryGUID = false,
            };
        }

        #endregion

        #region IDatabaseFactory

        public abstract IDbConnection CreateConnection();

        public virtual IDbDataAdapter CreateDataAdapter() => new SQLiteDataAdapter();

        public virtual IDatabaseImplementation CreateImplementation() => new SqliteImplementation();

        #endregion
    }
}

#endif
