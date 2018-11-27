using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Database
{
    public class ApplicationDatabaseConnectionCreator: DatabaseConnectionCreator<SQLiteConnection>
    {
        public ApplicationDatabaseConnectionCreator()
        {
            var builder = new SQLiteConnectionStringBuilder();
            builder.DataSource = ":memory:";

            ConnectionString = builder.ToString();
        }

        public ApplicationDatabaseConnectionCreator(FileInfo file)
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

        #region DatabaseConnectionCreator

        public override SQLiteConnection CreateConnection()
        {
            return new SQLiteConnection(ConnectionString);
        }

        #endregion
    }

    public class ApplicationDatabaseAccessor : SqliteAccessor
    {
        public ApplicationDatabaseAccessor(IDatabaseConnectionCreator<SQLiteConnection> connectionCreator, ILoggerFactory loggerFactory) : base(connectionCreator, loggerFactory)
        {
        }
    }
}
