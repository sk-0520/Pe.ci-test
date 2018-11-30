using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Database
{
    public class ApplicationDatabaseConnectionFactory : IDatabaseConnectionFactory
    {
        public ApplicationDatabaseConnectionFactory()
        {
            var builder = new SQLiteConnectionStringBuilder();
            builder.DataSource = ":memory:";

            ConnectionString = builder.ToString();
        }

        public ApplicationDatabaseConnectionFactory(FileInfo file)
        {
            var builder = new SQLiteConnectionStringBuilder();
            builder.DataSource = file.FullName;

            ConnectionString = builder.ToString();
        }


        #region property

        string ConnectionString { get; }

        #endregion

        #region function

        public static FileInfo ToSafeFile(FileInfo fileInfo)
        {
            // #66 を考慮
            if(fileInfo.FullName.StartsWith(@"\\")) {
                return new FileInfo(@"\\" + fileInfo.FullName);
            }

            return fileInfo;
        }

        #endregion

        #region IDatabaseConnectionFactory

        public IDbConnection CreateConnection()
        {
            return new SQLiteConnection(ConnectionString);
        }

        #endregion
    }

    public class ApplicationDatabaseAccessor : SqliteAccessor
    {
        public ApplicationDatabaseAccessor(IDatabaseConnectionFactory connectionCreator, ILogger logger)
            : base(connectionCreator, logger)
        { }
    }
}
