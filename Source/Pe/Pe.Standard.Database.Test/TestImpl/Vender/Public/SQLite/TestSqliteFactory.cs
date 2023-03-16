using System.Data;
using System.Data.SQLite;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Core.Models.Database.Vender.Public.SQLite;

namespace ContentTypeTextNet.Pe.Standard.Database.Test.TestImpl.Vender.Public.SQLite
{
    internal class TestSqliteFactory: SqliteFactory
    {
        public TestSqliteFactory()
        {
            var builder = CreateConnectionBuilder();
            builder.DataSource = ":memory:";
            builder.ForeignKeys = true;

            ConnectionString = builder.ToString();
        }

        #region property

        string ConnectionString { get; }

        #endregion

        #region IDatabaseFactory

        public override IDbConnection CreateConnection() => new SQLiteConnection(ConnectionString);

        public override IDatabaseImplementation CreateImplementation() => new SqliteImplementation();

        #endregion
    }
}
